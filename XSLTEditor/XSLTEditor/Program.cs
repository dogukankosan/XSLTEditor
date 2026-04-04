using System;
using System.Windows.Forms;
using XSLTEditor.Classes;
using XSLTEditor.Forms;

namespace XSLTEditor
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // ── Lisans Kontrol ────────────────────────────────────────────────
            LicenseResult sonuc = LicenseChecker.Kontrol();
            if (sonuc.Gecerli)
            {
                LogManager.Bilgi(string.Format("Lisans geçerli | Firma: {0} | HW: {1}",
                    sonuc.FirmaAdi, sonuc.HardwareId));
                Application.Run(new EditorForm());
            }
            else
            {
                LogManager.Bilgi($"Lisans geçersiz | HW: {sonuc.HardwareId} | Mesaj: {sonuc.Mesaj}");
                Application.Run(new LicenseForm(sonuc));
            }
        }
    }
}