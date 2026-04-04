using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using XSLTEditor.Classes;

namespace XSLTEditor.Forms
{
    public partial class LicenseForm : XtraForm
    {
        private LabelControl lblBaslik;
        private LabelControl lblAciklama;
        private LabelControl lblMesaj;
        private LabelControl lblHardwareBaslik;
        private MemoEdit txtHardwareId;
        private SimpleButton btnKopyala;
        private SimpleButton btnTekrarDene;
        private SimpleButton btnCikis;
        private readonly LicenseResult _sonuc;
        public LicenseForm(LicenseResult sonuc)
        {
            _sonuc = sonuc;
            InitializeComponent();
            DoldurBilgiler();
        }
        private void DoldurBilgiler()
        {
            txtHardwareId.Text = _sonuc.HardwareId;
            if (!string.IsNullOrWhiteSpace(_sonuc.Mesaj))
                lblMesaj.Text = "⚠  " + _sonuc.Mesaj;
        }
        private void BtnKopyala_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(_sonuc.HardwareId);
                btnKopyala.Text = "Kopyalandı!";
                System.Windows.Forms.Timer t = new System.Windows.Forms.Timer { Interval = 2000 };
                t.Tick += (s, ev) => { btnKopyala.Text = "Kopyala"; t.Stop(); t.Dispose(); };
                t.Start();
            }
            catch { }
        }
        private void BtnCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void BtnTekrarDene_Click(object sender, EventArgs e)
        {
            btnTekrarDene.Enabled = false;
            btnTekrarDene.Text = "Kontrol ediliyor...";
            LicenseResult sonuc = LicenseChecker.Kontrol();
            if (sonuc.Gecerli)
            {
                LogManager.Bilgi(string.Format("Tekrar dene: Lisans geçerli | Firma: {0}", sonuc.FirmaAdi));
                this.Hide();
                EditorForm editor = new EditorForm();
                editor.FormClosed += (s, ev) => Application.Exit();
                editor.Show();
            }
            else
            {
                lblMesaj.Text = "⚠  " + sonuc.Mesaj;
                btnTekrarDene.Enabled = true;
                btnTekrarDene.Text = "Tekrar Dene";
            }
        }
    }
}