namespace XSLTEditor.Forms
{
    partial class LicenseForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseForm));
            this.txtHardwareId = new DevExpress.XtraEditors.MemoEdit();
            this.lblBaslik = new DevExpress.XtraEditors.LabelControl();
            this.lblAciklama = new DevExpress.XtraEditors.LabelControl();
            this.lblMesaj = new DevExpress.XtraEditors.LabelControl();
            this.lblHardwareBaslik = new DevExpress.XtraEditors.LabelControl();
            this.btnKopyala = new DevExpress.XtraEditors.SimpleButton();
            this.btnTekrarDene = new DevExpress.XtraEditors.SimpleButton();
            this.btnCikis = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtHardwareId.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtHardwareId
            // 
            this.txtHardwareId.Location = new System.Drawing.Point(20, 188);
            this.txtHardwareId.Name = "txtHardwareId";
            this.txtHardwareId.Properties.Appearance.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtHardwareId.Properties.Appearance.Options.UseFont = true;
            this.txtHardwareId.Properties.ReadOnly = true;
            this.txtHardwareId.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtHardwareId.Size = new System.Drawing.Size(460, 60);
            this.txtHardwareId.TabIndex = 4;
            // 
            // lblBaslik
            // 
            this.lblBaslik.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblBaslik.Appearance.Options.UseFont = true;
            this.lblBaslik.Appearance.Options.UseForeColor = true;
            this.lblBaslik.Location = new System.Drawing.Point(20, 20);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(251, 25);
            this.lblBaslik.TabIndex = 0;
            this.lblBaslik.Text = "Lisans Doğrulaması Başarısız";
            // 
            // lblAciklama
            // 
            this.lblAciklama.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAciklama.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblAciklama.Appearance.Options.UseFont = true;
            this.lblAciklama.Appearance.Options.UseForeColor = true;
            this.lblAciklama.Location = new System.Drawing.Point(20, 60);
            this.lblAciklama.Name = "lblAciklama";
            this.lblAciklama.Size = new System.Drawing.Size(306, 51);
            this.lblAciklama.TabIndex = 1;
            this.lblAciklama.Text = "Bu bilgisayar için lisans kaydı bulunamadı.\r\nLisans kaydı yaptırmak için aşağıdak" +
    "i Donanım ID\'sini\r\nyetkili ile paylaşın.";
            // 
            // lblMesaj
            // 
            this.lblMesaj.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblMesaj.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblMesaj.Appearance.Options.UseFont = true;
            this.lblMesaj.Appearance.Options.UseForeColor = true;
            this.lblMesaj.Location = new System.Drawing.Point(20, 135);
            this.lblMesaj.Name = "lblMesaj";
            this.lblMesaj.Size = new System.Drawing.Size(0, 15);
            this.lblMesaj.TabIndex = 2;
            // 
            // lblHardwareBaslik
            // 
            this.lblHardwareBaslik.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHardwareBaslik.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblHardwareBaslik.Appearance.Options.UseFont = true;
            this.lblHardwareBaslik.Appearance.Options.UseForeColor = true;
            this.lblHardwareBaslik.Location = new System.Drawing.Point(20, 165);
            this.lblHardwareBaslik.Name = "lblHardwareBaslik";
            this.lblHardwareBaslik.Size = new System.Drawing.Size(151, 15);
            this.lblHardwareBaslik.TabIndex = 3;
            this.lblHardwareBaslik.Text = "Donanım ID (Hardware ID):";
            // 
            // btnKopyala
            // 
            this.btnKopyala.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Question;
            this.btnKopyala.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnKopyala.Appearance.Options.UseBackColor = true;
            this.btnKopyala.Appearance.Options.UseFont = true;
            this.btnKopyala.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKopyala.Location = new System.Drawing.Point(20, 262);
            this.btnKopyala.Name = "btnKopyala";
            this.btnKopyala.Size = new System.Drawing.Size(120, 32);
            this.btnKopyala.TabIndex = 5;
            this.btnKopyala.Text = "Kopyala";
            this.btnKopyala.Click += new System.EventHandler(this.BtnKopyala_Click);
            // 
            // btnTekrarDene
            // 
            this.btnTekrarDene.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btnTekrarDene.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTekrarDene.Appearance.Options.UseBackColor = true;
            this.btnTekrarDene.Appearance.Options.UseFont = true;
            this.btnTekrarDene.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTekrarDene.Location = new System.Drawing.Point(240, 340);
            this.btnTekrarDene.Name = "btnTekrarDene";
            this.btnTekrarDene.Size = new System.Drawing.Size(120, 36);
            this.btnTekrarDene.TabIndex = 6;
            this.btnTekrarDene.Text = "Tekrar Dene";
            this.btnTekrarDene.Click += new System.EventHandler(this.BtnTekrarDene_Click);
            // 
            // btnCikis
            // 
            this.btnCikis.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger;
            this.btnCikis.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCikis.Appearance.Options.UseBackColor = true;
            this.btnCikis.Appearance.Options.UseFont = true;
            this.btnCikis.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCikis.Location = new System.Drawing.Point(375, 340);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(105, 36);
            this.btnCikis.TabIndex = 7;
            this.btnCikis.Text = "Çıkış";
            this.btnCikis.Click += new System.EventHandler(this.BtnCikis_Click);
            // 
            // LicenseForm
            // 
            this.ClientSize = new System.Drawing.Size(520, 420);
            this.Controls.Add(this.lblBaslik);
            this.Controls.Add(this.lblAciklama);
            this.Controls.Add(this.lblMesaj);
            this.Controls.Add(this.lblHardwareBaslik);
            this.Controls.Add(this.txtHardwareId);
            this.Controls.Add(this.btnKopyala);
            this.Controls.Add(this.btnTekrarDene);
            this.Controls.Add(this.btnCikis);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("LicenseForm.IconOptions.Image")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LicenseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mutlu Yazılım — Lisans Gerekli";
            ((System.ComponentModel.ISupportInitialize)(this.txtHardwareId.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}