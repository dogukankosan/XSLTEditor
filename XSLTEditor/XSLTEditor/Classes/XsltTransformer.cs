using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using Saxon.Api;

namespace XSLTEditor.Classes
{
    /// <summary>
    /// XSLT 1.0 ve 2.0 dönüşümlerini otomatik algılayarak işler.
    /// 1.0 → System.Xml.Xsl.XslCompiledTransform
    /// 2.0 → Saxon-HE (Saxon.Api.Processor)
    /// </summary>
    public static class XsltTransformer
    {
        private static readonly string ExeDir = Path.GetDirectoryName(Application.ExecutablePath);
        private static readonly string OutputFile = Path.Combine(ExeDir, "result.html");

        /// <summary>
        /// XML + XSLT dönüşümü yapar, result.html üretir.
        /// </summary>
        /// <param name="xmlYolu">Kaynak XML dosya yolu</param>
        /// <param name="xsltYolu">XSLT şablon dosya yolu</param>
        /// <returns>Üretilen HTML dosyasının yolu</returns>
        public static string Donustur(string xmlYolu, string xsltYolu)
        {
            if (!File.Exists(xmlYolu))
            {
                string hata = $"XML dosyası bulunamadı: {xmlYolu}";
                LogManager.Hata(hata);
                throw new FileNotFoundException(hata);
            }
            if (!File.Exists(xsltYolu))
            {
                string hata = $"XSLT dosyası bulunamadı: {xsltYolu}";
                LogManager.Hata(hata);
                throw new FileNotFoundException(hata);
            }
            string versiyon = XsltVersiyonuAl(xsltYolu);
            LogManager.Bilgi($"XSLT versiyon algılandı: {versiyon} | XSLT: {Path.GetFileName(xsltYolu)} | XML: {Path.GetFileName(xmlYolu)}");
            if (versiyon == "2.0" || versiyon == "3.0")
                DonusturSaxon(xmlYolu, xsltYolu);
            else
                DonusturNative(xmlYolu, xsltYolu);
            LogManager.Bilgi($"Dönüşüm başarılı → {OutputFile}");
            return OutputFile;
        }
        // ── XSLT 1.0 — .NET Native ──────────────────────────────────────────
        private static void DonusturNative(string xmlYolu, string xsltYolu)
        {
            try
            {
                XslCompiledTransform xsl = new XslCompiledTransform();
                XsltSettings xsltSettings = new XsltSettings(enableDocumentFunction: true, enableScript: true);
                XmlUrlResolver xmlResolver = new XmlUrlResolver();
                xsl.Load(xsltYolu, xsltSettings, xmlResolver);
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
                // Hataları yakala
                List<StaticError> hatalar = new List<StaticError>();
                compiler.ErrorList = hatalar;
                XsltExecutable executable = null;
                try
                {
                    executable = compiler.Compile(new Uri("file:///" +
                        new FileInfo(xsltYolu).FullName.Replace("\\", "/")));
                }
                catch (Exception compileEx)
                {
                    // Saxon hata listesini logla
                    if (hatalar.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (StaticError h in hatalar)
                            sb.AppendLine(string.Format("Satır {0}, Sütun {1}: {2}",
                                h.LineNumber, h.ColumnNumber, h.Message));
                        LogManager.Hata("Saxon XSLT hataları:\n" + sb.ToString());
                        throw new Exception("XSLT derleme hatası:\n" + sb.ToString());
                    }
                    throw;
                }
                var transformer = executable.Load();
                using (FileStream xmlStream = new FileStream(xmlYolu, FileMode.Open, FileAccess.Read))
                {
                    transformer.SetInputStream(xmlStream,
                        new Uri("file:///" + new FileInfo(xmlYolu).FullName.Replace("\\", "/")));
                    using (FileStream outStream = new FileStream(OutputFile, FileMode.Create, FileAccess.Write))
                    {
                        Serializer serializer = processor.NewSerializer();
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
        // ── Versiyon algılama ────────────────────────────────────────────────
        private static string XsltVersiyonuAl(string xsltYolu)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xsltYolu);
                // xsl:stylesheet veya xsl:transform root element'ini bul
                XmlElement root = doc.DocumentElement;
                if (root != null)
                {
                    string versiyon = root.GetAttribute("version");
                    if (!string.IsNullOrEmpty(versiyon))
                        return versiyon.Trim();
                }
            }
            catch (Exception ex)
            {
                LogManager.Uyari($"XSLT versiyon okunamadı, 1.0 varsayılıyor: {ex.Message}");
            }
            return "1.0"; // varsayılan
        }
        /// <summary>
        /// Üretilen HTML dosyasının tam yolunu döner.
        /// </summary>
        public static string OutputDosyaYolu => OutputFile;
    }
}