using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using XSLTEditor.Classes;

namespace XSLTEditor.Forms
{
    public partial class CloudUblDownloadForm : XtraForm
    {
        public string XmlSonuc { get; private set; }
        public string KaydedilenYol { get; private set; }
        private TokenInfo _token;
        public CloudUblDownloadForm()
        {
            InitializeComponent();
            AyarlariYukle();
            TokenDurumGuncelle();
        }
        private void AyarlariYukle()
        {
            txtUsername.EditValue = CredentialManager.Load("username") ?? "";
            txtPassword.EditValue = CredentialManager.Load("password") ?? "";
            txtClientId.EditValue = CredentialManager.Load("client_id") ?? "";
            txtClientSecret.EditValue = CredentialManager.Load("client_secret") ?? "";
            txtCustomerId.EditValue = CredentialManager.Load("customer_id") ?? "";
            txtFirm.EditValue = CredentialManager.Load("firm") ?? "";
        }
        private void TokenDurumGuncelle()
        {
            _token = CredentialManager.GetValidToken();
            if (_token != null)
            {
                lblTokenDurum.Text = $"Token geçerli — {_token.RemainingMinutes} dk kaldı  |  Sunucu: {_token.Server}";
                lblTokenDurum.Appearance.ForeColor = System.Drawing.Color.FromArgb(59, 109, 17);
                lblTokenDurum.Appearance.BackColor = System.Drawing.Color.FromArgb(234, 243, 222);
                lblTokenDurum.Appearance.Options.UseBackColor = true;
                lblTokenDurum.Appearance.Options.UseForeColor = true;
            }
            else
            {
                lblTokenDurum.Text = "Token yok veya süresi dolmuş — İndir butonuna basınca otomatik alınır";
                lblTokenDurum.Appearance.ForeColor = System.Drawing.Color.FromArgb(156, 0, 6);
                lblTokenDurum.Appearance.BackColor = System.Drawing.Color.FromArgb(255, 235, 238);
                lblTokenDurum.Appearance.Options.UseBackColor = true;
                lblTokenDurum.Appearance.Options.UseForeColor = true;
            }
        }
        private void btnYeniToken_Click(object sender, EventArgs e)
        {
            AyarlariKaydetIc();
            CredentialManager.ClearToken();
            _token = TokenManager.GetOrFetchToken();
            if (_token == null)
                XtraMessageBox.Show("Token alınamadı. Ayarları kontrol edin.", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                XtraMessageBox.Show($"Token alındı! Kalan süre: {_token.RemainingMinutes} dk", "Başarılı",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            TokenDurumGuncelle();
        }
        private void btnAyarlariKaydet_Click(object sender, EventArgs e)
        {
            AyarlariKaydetIc();
            XtraMessageBox.Show("Ayarlar kaydedildi.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void AyarlariKaydetIc()
        {
            if (!string.IsNullOrWhiteSpace(txtUsername.Text))
                CredentialManager.Save("username", txtUsername.Text.Trim());
            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                CredentialManager.Save("password", txtPassword.Text.Trim());
            if (!string.IsNullOrWhiteSpace(txtClientId.Text))
                CredentialManager.Save("client_id", txtClientId.Text.Trim());
            if (!string.IsNullOrWhiteSpace(txtClientSecret.Text))
                CredentialManager.Save("client_secret", txtClientSecret.Text.Trim());
            if (!string.IsNullOrWhiteSpace(txtCustomerId.Text))
                CredentialManager.Save("customer_id", txtCustomerId.Text.Trim());
            if (!string.IsNullOrWhiteSpace(txtFirm.Text))
                CredentialManager.Save("firm", txtFirm.Text.Trim());
        }
        private void btnIndir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFaturaNo.Text))
            {
                XtraMessageBox.Show("Fatura No gerekli.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Token yoksa otomatik al
            if (_token == null)
            {
                AyarlariKaydetIc();
                _token = TokenManager.GetOrFetchToken();
                TokenDurumGuncelle();
                if (_token == null)
                {
                    XtraMessageBox.Show("Token alınamadı. Ayarlar sekmesini kontrol edin.", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tabControl.SelectedTabPage = tabAyarlar;
                    return;
                }
            }
            try
            {
                btnIndir.Enabled = false;
                btnIndir.Text = "İndiriliyor...";
                string faturaNo = Uri.EscapeDataString(txtFaturaNo.Text.Trim());
                string invoiceTypeStr = cmbInvoiceType.EditValue?.ToString() ?? "(08) Toptan Satış Faturası";
                string invoiceType = invoiceTypeStr.Substring(1, 2).TrimStart('0');
                if (string.IsNullOrEmpty(invoiceType)) invoiceType = "8";
                string isOutgoing = cmbIsOutgoing.EditValue?.ToString() ?? "true";
                string url = $"https://apigateway.logo.cloud/{_token.Server}" +
                             $"/logo/restservices/rest/v2.0/einvoice/receiveUblFile" +
                             $"?invoiceNo={faturaNo}&invoiceType={invoiceType}&isOutgoing={isOutgoing}";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.Headers.Add("access-token", _token.Token);
                req.Headers.Add("firm", _token.Firm);
                req.Headers.Add("lang", "TRTR");
                req.Accept = "application/json";
                string responseText;
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    responseText = sr.ReadToEnd();
                // fileContent parse
                int start = responseText.IndexOf("\"fileContent\":");
                int arrStart = responseText.IndexOf('[', start);
                int arrEnd = responseText.IndexOf(']', arrStart);
                string arrStr = responseText.Substring(arrStart + 1, arrEnd - arrStart - 1);
                string[] parts = arrStr.Split(',');
                byte[] bytes = new byte[parts.Length];
                for (int i = 0; i < parts.Length; i++)
                {
                    int val = int.Parse(parts[i].Trim());
                    if (val < 0) val += 256;
                    bytes[i] = (byte)val;
                }
                string xml = Encoding.UTF8.GetString(bytes);
                string klasor = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CLOUDUBL");
                if (!Directory.Exists(klasor)) Directory.CreateDirectory(klasor);
                string dosyaAdi = txtFaturaNo.Text.Trim()
                    .Replace("/", "_").Replace("\\", "_").Replace(":", "_") + ".xml";
                string kayitYolu = Path.Combine(klasor, dosyaAdi);
                File.WriteAllText(kayitYolu, xml, Encoding.UTF8);
                XmlSonuc = xml;
                KaydedilenYol = kayitYolu;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (WebException wex)
            {
                string hata = wex.Message;
                if (wex.Response != null)
                    using (StreamReader sr = new StreamReader(wex.Response.GetResponseStream()))
                        hata += "\n\n" + sr.ReadToEnd();
                XtraMessageBox.Show("API Hatası:\n" + hata, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Hata:\n" + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnIndir.Enabled = true;
                btnIndir.Text = "İndir ve Yükle";
            }
        }
        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void CloudUblDownloadForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
                this.Close();
        }
    }
}