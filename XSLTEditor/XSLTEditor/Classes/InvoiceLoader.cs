using System;
using System.IO;
using System.Windows.Forms;

namespace XSLTEditor.Classes
{
    /// <summary>
    /// XML kaynak dosyasını yönetir.
    /// Öncelik: kullanıcının TEMP klasörüne yüklediği XML.
    /// Yoksa default UBL\FATubl.xml kullanılır.
    /// </summary>
    public static class InvoiceLoader
    {
        private static readonly string ExeDir = Path.GetDirectoryName(Application.ExecutablePath);
        public static readonly string UblDir = Path.Combine(ExeDir, "UBL");
        public static readonly string TempDir = Path.Combine(ExeDir, "TEMP");
        // Default e-Fatura XML
        public static readonly string DefaultFaturaXml = Path.Combine(UblDir, "FATubl.xml");
        // Default e-İrsaliye XML (ileride eklenecek)
        public static readonly string DefaultIrsaliyeXml = Path.Combine(UblDir, "IRSubl.xml");
        // Kullanıcı tarafından yüklenen aktif XML yolu
        private static string _aktifXmlYolu = null;
        static InvoiceLoader()
        {
            try
            {
                if (!Directory.Exists(UblDir)) Directory.CreateDirectory(UblDir);
                if (!Directory.Exists(TempDir)) Directory.CreateDirectory(TempDir);
            }
            catch (Exception ex)
            {
                LogManager.Hata("Klasör oluşturma hatası", ex);
            }
        }
        /// <summary>
        /// Belge türüne göre aktif XML yolunu döner.
        /// Kullanıcı XML yüklediyse onu, yoksa default'u döner.
        /// </summary>
        public static string AktifXmlYoluAl(BelgeTuru tur)
        {
            if (!string.IsNullOrEmpty(_aktifXmlYolu) && File.Exists(_aktifXmlYolu))
            {
                LogManager.Bilgi($"Kullanıcı XML'i kullanılıyor: {_aktifXmlYolu}");
                return _aktifXmlYolu;
            }
            string defaultYol = tur == BelgeTuru.EFatura
                ? DefaultFaturaXml
                : DefaultIrsaliyeXml;
            if (!File.Exists(defaultYol))
            {
                LogManager.Uyari($"Default XML bulunamadı: {defaultYol}");
                return null;
            }
           LogManager.Bilgi($"Default XML kullanılıyor: {defaultYol}");
            return defaultYol;
        }
        /// <summary>
        /// Kullanıcının seçtiği XML'i TEMP klasörüne kopyalar ve aktif yapar.
        /// </summary>
        public static bool XmlYukle(string kaynakYol)
        {
            try
            {
                if (!File.Exists(kaynakYol))
                {
                    LogManager.Hata($"XML dosyası bulunamadı: {kaynakYol}");
                    return false;
                }
                string hedefAd = $"user_{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileName(kaynakYol)}";
                string hedefYol = Path.Combine(TempDir, hedefAd);
                File.Copy(kaynakYol, hedefYol, overwrite: true);
                _aktifXmlYolu = hedefYol;
                LogManager.Bilgi($"Kullanıcı XML'i TEMP'e kopyalandı: {hedefYol}");
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Hata("XML yükleme hatası", ex);
                return false;
            }
        }
        /// <summary>
        /// Kullanıcı XML'ini sıfırlar, default'a döner.
        /// </summary>
        public static void XmlSifirla()
        {
            _aktifXmlYolu = null;
            LogManager.Bilgi("Kullanıcı XML'i sıfırlandı, default kullanılacak.");
        }
        /// <summary>
        /// Aktif XML'in dosya adını döner (UI'da göstermek için).
        /// </summary>
        // Loglama olmadan sadece yol döner
        public static string AktifXmlAdi(BelgeTuru tur)
        {
            if (!string.IsNullOrEmpty(_aktifXmlYolu) && File.Exists(_aktifXmlYolu))
                return Path.GetFileName(_aktifXmlYolu);
            string defaultYol = tur == BelgeTuru.EFatura
                ? DefaultFaturaXml
                : DefaultIrsaliyeXml;
            return File.Exists(defaultYol)
                ? Path.GetFileName(defaultYol)
                : "(XML bulunamadı)";
        }
    }
    public enum BelgeTuru
    {
        EFatura,
        EIrsaliye
    }
}