using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace XSLTEditor.Classes
{
    public static class LogManager
    {
        private static readonly string LogDir = Path.Combine(
            Path.GetDirectoryName(Application.ExecutablePath), "LOG");
        static LogManager()
        {
            try
            {
                if (!Directory.Exists(LogDir))
                    Directory.CreateDirectory(LogDir);
            }
            catch { }
        }
        // Her çağrıda günün dosyasını hesapla
        private static string LogFile =>
            Path.Combine(LogDir, string.Format("XSLTEditor_{0:yyyyMMdd}.log", DateTime.Now));
        public static void Bilgi(string mesaj) => Yaz("BİLGİ", mesaj);
        public static void Uyari(string mesaj) => Yaz("UYARI", mesaj);
        public static void Hata(string mesaj) => Yaz("HATA", mesaj);
        public static void Hata(string mesaj, Exception ex)
            => Yaz("HATA", string.Format("{0} | {1}: {2}{3}{4}",
                mesaj, ex.GetType().Name, ex.Message,
                Environment.NewLine, ex.StackTrace));
        private static void Yaz(string seviye, string mesaj)
        {
            try
            {
                string satir = string.Format("[{0:yyyy-MM-dd HH:mm:ss}] [{1}] {2}",
                    DateTime.Now, seviye, mesaj);
                File.AppendAllText(LogFile, satir + Environment.NewLine,
                    Encoding.UTF8);
            }
            catch { }
        }
    }
}