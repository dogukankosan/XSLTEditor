namespace XSLTEditor.Forms
{
    partial class CloudUblDownloadForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CloudUblDownloadForm));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblBaslik = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnIndir = new DevExpress.XtraEditors.SimpleButton();
            this.btnIptal = new DevExpress.XtraEditors.SimpleButton();
            this.tabControl = new DevExpress.XtraTab.XtraTabControl();
            this.tabAyarlar = new DevExpress.XtraTab.XtraTabPage();
            this.lblUsername = new DevExpress.XtraEditors.LabelControl();
            this.txtUsername = new DevExpress.XtraEditors.TextEdit();
            this.lblPassword = new DevExpress.XtraEditors.LabelControl();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.lblClientId = new DevExpress.XtraEditors.LabelControl();
            this.txtClientId = new DevExpress.XtraEditors.TextEdit();
            this.lblClientSecret = new DevExpress.XtraEditors.LabelControl();
            this.txtClientSecret = new DevExpress.XtraEditors.TextEdit();
            this.lblCustomerId = new DevExpress.XtraEditors.LabelControl();
            this.txtCustomerId = new DevExpress.XtraEditors.TextEdit();
            this.lblFirm = new DevExpress.XtraEditors.LabelControl();
            this.txtFirm = new DevExpress.XtraEditors.TextEdit();
            this.btnAyarlariKaydet = new DevExpress.XtraEditors.SimpleButton();
            this.btnYeniToken = new DevExpress.XtraEditors.SimpleButton();
            this.tabFatura = new DevExpress.XtraTab.XtraTabPage();
            this.lblFaturaNo = new DevExpress.XtraEditors.LabelControl();
            this.txtFaturaNo = new DevExpress.XtraEditors.TextEdit();
            this.lblInvoiceType = new DevExpress.XtraEditors.LabelControl();
            this.cmbInvoiceType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblIsOutgoing = new DevExpress.XtraEditors.LabelControl();
            this.cmbIsOutgoing = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblTokenDurum = new DevExpress.XtraEditors.LabelControl();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabAyarlar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientSecret.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFirm.Properties)).BeginInit();
            this.tabFatura.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFaturaNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbInvoiceType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbIsOutgoing.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(95)))), ((int)(((byte)(165)))));
            this.pnlHeader.Controls.Add(this.lblBaslik);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(500, 44);
            this.pnlHeader.TabIndex = 2;
            // 
            // lblBaslik
            // 
            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblBaslik.ForeColor = System.Drawing.Color.White;
            this.lblBaslik.Location = new System.Drawing.Point(0, 0);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(500, 44);
            this.lblBaslik.TabIndex = 0;
            this.lblBaslik.Text = "   Cloud UBL İndir";
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.btnIndir);
            this.pnlFooter.Controls.Add(this.btnIptal);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 342);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(0, 8, 12, 8);
            this.pnlFooter.Size = new System.Drawing.Size(500, 48);
            this.pnlFooter.TabIndex = 1;
            // 
            // btnIndir
            // 
            this.btnIndir.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(95)))), ((int)(((byte)(165)))));
            this.btnIndir.Appearance.ForeColor = System.Drawing.Color.White;
            this.btnIndir.Appearance.Options.UseBackColor = true;
            this.btnIndir.Appearance.Options.UseForeColor = true;
            this.btnIndir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIndir.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnIndir.Location = new System.Drawing.Point(283, 8);
            this.btnIndir.Name = "btnIndir";
            this.btnIndir.Size = new System.Drawing.Size(130, 32);
            this.btnIndir.TabIndex = 0;
            this.btnIndir.Text = "İndir ve Yükle";
            this.btnIndir.Click += new System.EventHandler(this.btnIndir_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIptal.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnIptal.Location = new System.Drawing.Point(413, 8);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(75, 32);
            this.btnIptal.TabIndex = 1;
            this.btnIptal.Text = "İptal";
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 44);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedTabPage = this.tabAyarlar;
            this.tabControl.Size = new System.Drawing.Size(500, 298);
            this.tabControl.TabIndex = 0;
            this.tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabFatura,
            this.tabAyarlar});
            // 
            // tabAyarlar
            // 
            this.tabAyarlar.Controls.Add(this.lblUsername);
            this.tabAyarlar.Controls.Add(this.txtUsername);
            this.tabAyarlar.Controls.Add(this.lblPassword);
            this.tabAyarlar.Controls.Add(this.txtPassword);
            this.tabAyarlar.Controls.Add(this.lblClientId);
            this.tabAyarlar.Controls.Add(this.txtClientId);
            this.tabAyarlar.Controls.Add(this.lblClientSecret);
            this.tabAyarlar.Controls.Add(this.txtClientSecret);
            this.tabAyarlar.Controls.Add(this.lblCustomerId);
            this.tabAyarlar.Controls.Add(this.txtCustomerId);
            this.tabAyarlar.Controls.Add(this.lblFirm);
            this.tabAyarlar.Controls.Add(this.txtFirm);
            this.tabAyarlar.Controls.Add(this.btnAyarlariKaydet);
            this.tabAyarlar.Controls.Add(this.btnYeniToken);
            this.tabAyarlar.Name = "tabAyarlar";
            this.tabAyarlar.Padding = new System.Windows.Forms.Padding(10);
            this.tabAyarlar.Size = new System.Drawing.Size(498, 273);
            this.tabAyarlar.Text = "Ayarlar";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblUsername.Location = new System.Drawing.Point(12, 20);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(110, 13);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Kullanıcı Adı:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(130, 16);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(320, 20);
            this.txtUsername.TabIndex = 0;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblPassword.Location = new System.Drawing.Point(12, 54);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(110, 13);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Şifre:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(130, 50);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(320, 20);
            this.txtPassword.TabIndex = 1;
            // 
            // lblClientId
            // 
            this.lblClientId.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblClientId.Location = new System.Drawing.Point(12, 84);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(110, 13);
            this.lblClientId.TabIndex = 2;
            this.lblClientId.Text = "Client ID:";
            // 
            // txtClientId
            // 
            this.txtClientId.Location = new System.Drawing.Point(130, 80);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Properties.PasswordChar = '*';
            this.txtClientId.Size = new System.Drawing.Size(320, 20);
            this.txtClientId.TabIndex = 2;
            // 
            // lblClientSecret
            // 
            this.lblClientSecret.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblClientSecret.Location = new System.Drawing.Point(12, 122);
            this.lblClientSecret.Name = "lblClientSecret";
            this.lblClientSecret.Size = new System.Drawing.Size(110, 13);
            this.lblClientSecret.TabIndex = 3;
            this.lblClientSecret.Text = "Client Secret:";
            // 
            // txtClientSecret
            // 
            this.txtClientSecret.Location = new System.Drawing.Point(130, 118);
            this.txtClientSecret.Name = "txtClientSecret";
            this.txtClientSecret.Properties.PasswordChar = '*';
            this.txtClientSecret.Size = new System.Drawing.Size(320, 20);
            this.txtClientSecret.TabIndex = 3;
            // 
            // lblCustomerId
            // 
            this.lblCustomerId.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCustomerId.Location = new System.Drawing.Point(12, 156);
            this.lblCustomerId.Name = "lblCustomerId";
            this.lblCustomerId.Size = new System.Drawing.Size(110, 13);
            this.lblCustomerId.TabIndex = 4;
            this.lblCustomerId.Text = "Customer ID:";
            // 
            // txtCustomerId
            // 
            this.txtCustomerId.Location = new System.Drawing.Point(130, 152);
            this.txtCustomerId.Name = "txtCustomerId";
            this.txtCustomerId.Properties.PasswordChar = '*';
            this.txtCustomerId.Size = new System.Drawing.Size(320, 20);
            this.txtCustomerId.TabIndex = 4;
            // 
            // lblFirm
            // 
            this.lblFirm.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblFirm.Location = new System.Drawing.Point(12, 190);
            this.lblFirm.Name = "lblFirm";
            this.lblFirm.Size = new System.Drawing.Size(110, 13);
            this.lblFirm.TabIndex = 5;
            this.lblFirm.Text = "Firma No:";
            // 
            // txtFirm
            // 
            this.txtFirm.Location = new System.Drawing.Point(130, 186);
            this.txtFirm.Name = "txtFirm";
            this.txtFirm.Size = new System.Drawing.Size(100, 20);
            this.txtFirm.TabIndex = 5;
            // 
            // btnAyarlariKaydet
            // 
            this.btnAyarlariKaydet.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(95)))), ((int)(((byte)(165)))));
            this.btnAyarlariKaydet.Appearance.ForeColor = System.Drawing.Color.White;
            this.btnAyarlariKaydet.Appearance.Options.UseBackColor = true;
            this.btnAyarlariKaydet.Appearance.Options.UseForeColor = true;
            this.btnAyarlariKaydet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAyarlariKaydet.Location = new System.Drawing.Point(130, 228);
            this.btnAyarlariKaydet.Name = "btnAyarlariKaydet";
            this.btnAyarlariKaydet.Size = new System.Drawing.Size(150, 28);
            this.btnAyarlariKaydet.TabIndex = 6;
            this.btnAyarlariKaydet.Text = "Ayarları Kaydet";
            this.btnAyarlariKaydet.Click += new System.EventHandler(this.btnAyarlariKaydet_Click);
            // 
            // btnYeniToken
            // 
            this.btnYeniToken.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnYeniToken.Location = new System.Drawing.Point(290, 228);
            this.btnYeniToken.Name = "btnYeniToken";
            this.btnYeniToken.Size = new System.Drawing.Size(150, 28);
            this.btnYeniToken.TabIndex = 7;
            this.btnYeniToken.Text = "Token Yenile";
            this.btnYeniToken.Click += new System.EventHandler(this.btnYeniToken_Click);
            // 
            // tabFatura
            // 
            this.tabFatura.Controls.Add(this.lblFaturaNo);
            this.tabFatura.Controls.Add(this.txtFaturaNo);
            this.tabFatura.Controls.Add(this.lblInvoiceType);
            this.tabFatura.Controls.Add(this.cmbInvoiceType);
            this.tabFatura.Controls.Add(this.lblIsOutgoing);
            this.tabFatura.Controls.Add(this.cmbIsOutgoing);
            this.tabFatura.Controls.Add(this.lblTokenDurum);
            this.tabFatura.Name = "tabFatura";
            this.tabFatura.Padding = new System.Windows.Forms.Padding(10);
            this.tabFatura.Size = new System.Drawing.Size(498, 273);
            this.tabFatura.Text = "Fatura İndir";
            // 
            // lblFaturaNo
            // 
            this.lblFaturaNo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblFaturaNo.Location = new System.Drawing.Point(12, 20);
            this.lblFaturaNo.Name = "lblFaturaNo";
            this.lblFaturaNo.Size = new System.Drawing.Size(95, 13);
            this.lblFaturaNo.TabIndex = 0;
            this.lblFaturaNo.Text = "Fatura No:";
            // 
            // txtFaturaNo
            // 
            this.txtFaturaNo.Location = new System.Drawing.Point(115, 16);
            this.txtFaturaNo.Name = "txtFaturaNo";
            this.txtFaturaNo.Size = new System.Drawing.Size(340, 20);
            this.txtFaturaNo.TabIndex = 0;
            // 
            // lblInvoiceType
            // 
            this.lblInvoiceType.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblInvoiceType.Location = new System.Drawing.Point(12, 54);
            this.lblInvoiceType.Name = "lblInvoiceType";
            this.lblInvoiceType.Size = new System.Drawing.Size(95, 13);
            this.lblInvoiceType.TabIndex = 1;
            this.lblInvoiceType.Text = "Fatura Türü:";
            // 
            // cmbInvoiceType
            // 
            this.cmbInvoiceType.EditValue = "(08) Toptan Satış Faturası";
            this.cmbInvoiceType.Location = new System.Drawing.Point(115, 51);
            this.cmbInvoiceType.Name = "cmbInvoiceType";
            this.cmbInvoiceType.Properties.Items.AddRange(new object[] {
            "(06) Satınalma İade Faturası",
            "(07) Perakende Satış Faturası",
            "(08) Toptan Satış Faturası",
            "(09) Verilen Hizmet Faturası",
            "(22) Varlık Satış Faturası"});
            this.cmbInvoiceType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbInvoiceType.Size = new System.Drawing.Size(340, 20);
            this.cmbInvoiceType.TabIndex = 1;
            // 
            // lblIsOutgoing
            // 
            this.lblIsOutgoing.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblIsOutgoing.Location = new System.Drawing.Point(11, 94);
            this.lblIsOutgoing.Name = "lblIsOutgoing";
            this.lblIsOutgoing.Size = new System.Drawing.Size(75, 13);
            this.lblIsOutgoing.TabIndex = 2;
            this.lblIsOutgoing.Text = "isOutgoing:";
            // 
            // cmbIsOutgoing
            // 
            this.cmbIsOutgoing.EditValue = "true";
            this.cmbIsOutgoing.Location = new System.Drawing.Point(115, 91);
            this.cmbIsOutgoing.Name = "cmbIsOutgoing";
            this.cmbIsOutgoing.Properties.Items.AddRange(new object[] {
            "true",
            "false"});
            this.cmbIsOutgoing.Size = new System.Drawing.Size(90, 20);
            this.cmbIsOutgoing.TabIndex = 2;
            // 
            // lblTokenDurum
            // 
            this.lblTokenDurum.Appearance.Options.UseTextOptions = true;
            this.lblTokenDurum.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.lblTokenDurum.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTokenDurum.Location = new System.Drawing.Point(13, 137);
            this.lblTokenDurum.Name = "lblTokenDurum";
            this.lblTokenDurum.Size = new System.Drawing.Size(443, 36);
            this.lblTokenDurum.TabIndex = 3;
            // 
            // CloudUblDownloadForm
            // 
            this.ClientSize = new System.Drawing.Size(500, 390);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("CloudUblDownloadForm.IconOptions.Image")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CloudUblDownloadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fatura İndir";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CloudUblDownloadForm_KeyDown);
            this.pnlHeader.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabAyarlar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientSecret.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFirm.Properties)).EndInit();
            this.tabFatura.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFaturaNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbInvoiceType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbIsOutgoing.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        // ── Field declarations ───────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Panel pnlFooter;
        private DevExpress.XtraTab.XtraTabControl tabControl;
        private DevExpress.XtraTab.XtraTabPage tabFatura;
        private DevExpress.XtraTab.XtraTabPage tabAyarlar;
        private DevExpress.XtraEditors.LabelControl lblFaturaNo;
        private DevExpress.XtraEditors.TextEdit txtFaturaNo;
        private DevExpress.XtraEditors.LabelControl lblInvoiceType;
        private DevExpress.XtraEditors.ComboBoxEdit cmbInvoiceType;
        private DevExpress.XtraEditors.LabelControl lblIsOutgoing;
        private DevExpress.XtraEditors.ComboBoxEdit cmbIsOutgoing;
        private DevExpress.XtraEditors.LabelControl lblTokenDurum;
        private DevExpress.XtraEditors.LabelControl lblUsername;
        private DevExpress.XtraEditors.TextEdit txtUsername;
        private DevExpress.XtraEditors.LabelControl lblPassword;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.LabelControl lblClientId;
        private DevExpress.XtraEditors.TextEdit txtClientId;
        private DevExpress.XtraEditors.LabelControl lblClientSecret;
        private DevExpress.XtraEditors.TextEdit txtClientSecret;
        private DevExpress.XtraEditors.LabelControl lblCustomerId;
        private DevExpress.XtraEditors.TextEdit txtCustomerId;
        private DevExpress.XtraEditors.LabelControl lblFirm;
        private DevExpress.XtraEditors.TextEdit txtFirm;
        private DevExpress.XtraEditors.SimpleButton btnAyarlariKaydet;
        private DevExpress.XtraEditors.SimpleButton btnYeniToken;
        private DevExpress.XtraEditors.SimpleButton btnIndir;
        private DevExpress.XtraEditors.SimpleButton btnIptal;
    }
}