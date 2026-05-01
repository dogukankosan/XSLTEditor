using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using Saxon.Api;

namespace XSLTEditor.Classes
{
    /// <summary>
    /// XSLT 1.0 ve 2.0 dönüşümlerini otomatik algılayarak işler.
    ///
    /// Çift tıklama köprüsü:
    ///   Dönüşümden ÖNCE XSLT'deki literal HTML elementlerine
    ///   data-xslt-line="N" attribute enjekte edilir.
    ///   Böylece çıktı HTML'de her hücre hangi XSLT satırından geldiğini bilir.
    ///   dblclick → data-xslt-line oku → "GOTO_LINE:N" → EditorForm → JumpTo(N)
    /// </summary>
    public static class XsltTransformer
    {
        private static readonly string ExeDir =
            Path.GetDirectoryName(Application.ExecutablePath);
        private static readonly string OutputFile =
            Path.Combine(ExeDir, "result.html");
        // ── Ana dönüşüm ──────────────────────────────────────────────────────
        public static string Donustur(string xmlYolu, string xsltYolu)
        {
            if (!File.Exists(xmlYolu))
                throw new FileNotFoundException($"XML dosyası bulunamadı: {xmlYolu}");
            if (!File.Exists(xsltYolu))
                throw new FileNotFoundException($"XSLT dosyası bulunamadı: {xsltYolu}");
            string versiyon = XsltVersiyonuAl(xsltYolu);
            LogManager.Bilgi(
                $"XSLT versiyon: {versiyon} | " +
                $"{Path.GetFileName(xsltYolu)} | {Path.GetFileName(xmlYolu)}");
            // 1) Satır numarası attribute'larını enjekte et → geçici XSLT
            string geciciXslt = XslteSatirNoEnjekte(xsltYolu);
            try
            {
                // 2) Dönüşüm
                if (versiyon == "2.0" || versiyon == "3.0")
                    DonusturSaxon(xmlYolu, geciciXslt);
                else
                    DonusturNative(xmlYolu, geciciXslt);
            }
            finally
            {
                try
                {
                    if (File.Exists(geciciXslt) && geciciXslt != xsltYolu)
                        File.Delete(geciciXslt);
                }
                catch { }
            }
            // 3) dblclick script'i ekle
            HtmleSayfaScriptEkle(OutputFile);
            LogManager.Bilgi($"Dönüşüm başarılı → {OutputFile}");
            return OutputFile;
        }
        // ── XSLT'ye data-xslt-line enjeksiyonu ──────────────────────────────
        private static string XslteSatirNoEnjekte(string xsltYolu)
        {
            try
            {
                string[] satirlar = File.ReadAllLines(xsltYolu, Encoding.UTF8);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < satirlar.Length; i++)
                    sb.AppendLine(HtmlElemanAcilisEnjekte(satirlar[i], i + 1));
                string gecici = Path.Combine(
                    Path.GetDirectoryName(xsltYolu),
                    "_instr_" + Path.GetFileName(xsltYolu));
                File.WriteAllText(gecici, sb.ToString(), Encoding.UTF8);
                return gecici;
            }
            catch (Exception ex)
            {
                LogManager.Hata("XSLT enstrümantasyon hatası", ex);
                return xsltYolu; // hata olursa orijinal
            }
        }
        /// <summary>
        /// Satırdaki literal HTML açılış tag'lerine data-xslt-line="N" ekler.
        /// xsl:* tag'lerine, kapalı tag'lere ve self-closing tag'lere dokunmaz.
        ///
        /// Giriş  (satır 47): "  &lt;td class=""etiket""&gt;"
        /// Çıkış           : "  &lt;td class=""etiket"" data-xslt-line=""47""&gt;"
        /// </summary>
        private static string HtmlElemanAcilisEnjekte(string satir, int satirNo)
        {
            // Literal HTML element açılışları — closing (</x>) ve xsl: hariç
            // Hedef tag'ler: blok ve tablo elementleri
            return Regex.Replace(
                satir,
                @"<(?!/|xsl:|!)(td|tr|th|thead|tbody|tfoot|table|div|span|p|h[1-6]|li|ul|ol|section|article|header|footer|main|aside|label|a\b|strong|em|b\b|i\b|small|sup|sub)((?:\s+[^>]*?)?)(?<!/)>",
                m =>
                {
                    string tag = m.Groups[1].Value;
                    string attrs = m.Groups[2].Value;
                    if (attrs.Contains("data-xslt-line")) return m.Value; // zaten var
                    return $"<{tag}{attrs} data-xslt-line=\"{satirNo}\">";
                },
                RegexOptions.IgnoreCase);
        }
        // ── XSLT 1.0 — .NET Native ──────────────────────────────────────────
        private static void DonusturNative(string xmlYolu, string xsltYolu)
        {
            try
            {
                XslCompiledTransform xsl = new XslCompiledTransform();
                XsltSettings xsltSettings = new XsltSettings(
                    enableDocumentFunction: true, enableScript: true);
                xsl.Load(xsltYolu, xsltSettings, new XmlUrlResolver());
                xsl.Transform(xmlYolu, OutputFile);
            }
            catch (Exception ex)
            {
                LogManager.Hata("XSLT 1.0 dönüşüm hatası", ex);
                throw;
            }
        }
        // ── XSLT 2.0/3.0 — Saxon-HE ─────────────────────────────────────────
        private static void DonusturSaxon(string xmlYolu, string xsltYolu)
        {
            try
            {
                Processor processor = new Processor();
                var compiler = processor.NewXsltCompiler();
                List<XmlProcessingError> hatalar = new List<XmlProcessingError>();
                compiler.SetErrorList(hatalar);
                XsltExecutable executable;
                try
                {
                    executable = compiler.Compile(new Uri("file:///" +
                        new FileInfo(xsltYolu).FullName.Replace("\\", "/")));
                }
                catch (Exception)
                {
                    if (hatalar.Count > 0)
                    {
                        var sb = new StringBuilder();
                        foreach (var h in hatalar) sb.AppendLine($"  {h.Message}");
                        LogManager.Hata("Saxon XSLT hataları:\n" + sb);
                        throw new Exception("XSLT derleme hatası:\n" + sb);
                    }
                    throw;
                }
                var transformer = executable.Load();
                using (FileStream xmlStream = new FileStream(xmlYolu, FileMode.Open, FileAccess.Read))
                {
                    transformer.SetInputStream(xmlStream,
                        new Uri("file:///" +
                            new FileInfo(xmlYolu).FullName.Replace("\\", "/")));
                    using (FileStream outStream =
                        new FileStream(OutputFile, FileMode.Create, FileAccess.Write))
                    {
                        var serializer = processor.NewSerializer();
                        serializer.SetOutputStream(outStream);
                        serializer.SetOutputProperty(Serializer.METHOD, "html");
                        serializer.SetOutputProperty(Serializer.ENCODING, "UTF-8");
                        transformer.Run(serializer);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Hata("XSLT 2.0/3.0 (Saxon) dönüşüm hatası", ex);
                throw;
            }
        }
        // ── HTML'e dblclick script'i enjekte et ──────────────────────────────
        private static void HtmleSayfaScriptEkle(string htmlYolu)
        {
            try
            {
                if (!File.Exists(htmlYolu)) return;
                string html = File.ReadAllText(htmlYolu, Encoding.UTF8);
                if (html.Contains("data-xslt-bridge")) return;
                // Tek sorumluluk: data-xslt-line'ı oku, GOTO_LINE gönder
                // Hiçbir tahmin / tag adı çıkarma yok — satır numarası zaten HTML'de
                const string script = @"
<!-- data-xslt-bridge -->
<style>
  .xslt-hl {
    outline: 2px solid #0078d7 !important;
    background: rgba(0,120,215,0.10) !important;
    border-radius: 2px;
  }
</style>
<script>
(function(){
  var sonVurgu = null;

  function vurgula(el){
    if(sonVurgu) sonVurgu.classList.remove('xslt-hl');
    sonVurgu = el;
    if(el) el.classList.add('xslt-hl');
  }

  function cefGonder(mesaj){
    if(window.CefSharp && window.CefSharp.PostMessage){
      window.CefSharp.PostMessage(mesaj);
    } else {
      window.location.hash = encodeURIComponent(mesaj);
    }
  }

  // Hiyerarşide data-xslt-line attribute'unu yukarı çıkarak bul
  function satirBul(el){
    for(var i = 0; i < 15 && el && el !== document.body; i++){
      var v = el.getAttribute && el.getAttribute('data-xslt-line');
      if(v) return v;
      el = el.parentElement;
    }
    return null;
  }

  document.addEventListener('dblclick', function(e){
    e.stopPropagation();
    vurgula(e.target);
    var no = satirBul(e.target);
    if(no) cefGonder('GOTO_LINE:' + no);
  }, true);
})();
</script>";
                string lower = html.ToLower();
                int idx = lower.LastIndexOf("</body>");
                if (idx >= 0)
                    html = html.Insert(idx, script + "\n");
                else
                    html += "\n" + script;
                File.WriteAllText(htmlYolu, html, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LogManager.Hata("HTML script enjekte hatası", ex);
            }
        }
        // ── Versiyon algılama ────────────────────────────────────────────────
        private static string XsltVersiyonuAl(string xsltYolu)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xsltYolu);
                string v = doc.DocumentElement?.GetAttribute("version");
                if (!string.IsNullOrEmpty(v)) return v.Trim();
            }
            catch (Exception ex)
            {
                LogManager.Uyari($"XSLT versiyon okunamadı: {ex.Message}");
            }
            return "1.0";
        }
        public static string OutputDosyaYolu => OutputFile;
    }
}