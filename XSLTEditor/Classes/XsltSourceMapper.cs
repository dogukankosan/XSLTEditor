using System;
using System.Collections.Generic;
using System.IO;

namespace XSLTEditor.Classes
{
    /// <summary>
    /// Önizlemede çift tıklanan HTML elemanının kaynak XML tag adını alır,
    /// aktif XSLT dosyası içinde bu tag'i üreten satırı bulur ve satır numarasını döner.
    ///
    /// Arama önceliği (ilk eşleşen satır döner):
    ///   1) xsl:value-of select="...[tagAdi]..."
    ///   2) xsl:for-each select="...[tagAdi]..."
    ///   3) xsl:apply-templates select="...[tagAdi]..."
    ///   4) xsl:template match="...[tagAdi]..."
    ///   5) cbc:tagAdi veya cac:tagAdi literal element
    ///   6) Herhangi bir yerde tagAdi geçen satır
    /// </summary>
    public static class XsltSourceMapper
    {
        /// <summary>
        /// XSLT dosyasında tagAdi için en uygun satırı arar.
        /// </summary>
        /// <param name="xsltYolu">XSLT dosyasının tam yolu</param>
        /// <param name="tagAdi">
        ///   HTML elementinden çıkarılan tag adı.
        ///   Örnek: "cbc:ID", "InvoiceLine", "TaxAmount"
        /// </param>
        /// <returns>
        ///   0-bazlı satır numarası; bulunamazsa -1.
        /// </returns>
        public static int SatirBul(string xsltYolu, string tagAdi)
        {
            if (string.IsNullOrEmpty(xsltYolu) || string.IsNullOrEmpty(tagAdi))
                return -1;
            if (!File.Exists(xsltYolu))
            {
                LogManager.Uyari($"XsltSourceMapper: Dosya bulunamadı: {xsltYolu}");
                return -1;
            }
            try
            {
                string[] satirlar = File.ReadAllLines(xsltYolu);
                // Arama kalıplarını öncelik sırasına göre diz
                // Her kalıp için satır listesi — ilk eşleşeni döneceğiz
                var oncelikliKaliplar = new List<Func<string, bool>>
                {
                    // 1) value-of select içinde
                    line => line.Contains("xsl:value-of") &&
                            TagGeciyorMu(line, tagAdi),
                    // 2) for-each select içinde
                    line => line.Contains("xsl:for-each") &&
                            TagGeciyorMu(line, tagAdi),
                    // 3) apply-templates select içinde
                    line => line.Contains("xsl:apply-templates") &&
                            TagGeciyorMu(line, tagAdi),
                    // 4) template match içinde
                    line => line.Contains("xsl:template") &&
                            TagGeciyorMu(line, tagAdi),
                    // 5) Literal element (cbc:X, cac:X, n1:X, xsl:X ...)
                    line => SatirLiteralElemanIceriyorMu(line, tagAdi),
                    // 6) Fallback — herhangi bir yerde geçiyor
                    line => TagGeciyorMu(line, tagAdi),
                };
                foreach (var kalip in oncelikliKaliplar)
                {
                    for (int i = 0; i < satirlar.Length; i++)
                    {
                        if (kalip(satirlar[i]))
                        {
                            LogManager.Bilgi(
                                $"XsltSourceMapper: '{tagAdi}' → Satır {i + 1} " +
                                $"(0-bazlı: {i})");
                            return i; // 0-bazlı
                        }
                    }
                }
                LogManager.Uyari($"XsltSourceMapper: '{tagAdi}' hiçbir satırda bulunamadı.");
                return -1;
            }
            catch (Exception ex)
            {
                LogManager.Hata("XsltSourceMapper.SatirBul hatası", ex);
                return -1;
            }
        }
        // ── Yardımcılar ──────────────────────────────────────────────────────
        /// <summary>
        /// Satırda tagAdi'nin tam kelime veya XPath parçası olarak geçip geçmediğini kontrol eder.
        /// "cbc:ID" ve "ID" ikisi de eşleşir; "IDREF" eşleşmez.
        /// </summary>
        private static bool TagGeciyorMu(string satir, string tagAdi)
        {
            if (string.IsNullOrEmpty(satir)) return false;
            // Tam eşleşme (büyük/küçük harf duyarsız)
            int idx = satir.IndexOf(tagAdi, StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return false;
            // Kelime sınırı kontrolü — önceki ve sonraki karaktere bak
            char onceki = idx > 0 ? satir[idx - 1] : '\0';
            char sonraki = idx + tagAdi.Length < satir.Length
                ? satir[idx + tagAdi.Length]
                : '\0';
            bool oncekiOk = onceki == '\0' || onceki == '/' || onceki == ':' ||
                            onceki == '"' || onceki == '\'' || onceki == '(' ||
                            onceki == '[' || onceki == ' ' || onceki == '\t' ||
                            onceki == '<' || onceki == '*';
            bool sonrakiOk = sonraki == '\0' || sonraki == '/' || sonraki == '"' ||
                             sonraki == '\'' || sonraki == ')' || sonraki == ']' ||
                             sonraki == ' ' || sonraki == '\t' || sonraki == '>' ||
                             sonraki == '[';
            return oncekiOk && sonrakiOk;
        }
        /// <summary>
        /// Satırın literal bir XML/XSLT element açılış tag'i içerip içermediğini kontrol eder.
        /// Örnek: &lt;cbc:TaxAmount  veya  &lt;/cbc:TaxAmount
        /// </summary>
        private static bool SatirLiteralElemanIceriyorMu(string satir, string tagAdi)
        {
            // <tagAdi veya </tagAdi biçiminde geçmeli
            return satir.Contains("<" + tagAdi) ||
                   satir.Contains("</" + tagAdi);
        }
    }
}