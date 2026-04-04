using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.Utils;
using ICSharpCode.TextEditor;

namespace XSLTEditor.Forms
{
    partial class EditorForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnYeni = new DevExpress.XtraBars.BarButtonItem();
            this.btnAc = new DevExpress.XtraBars.BarButtonItem();
            this.btnKaydet = new DevExpress.XtraBars.BarButtonItem();
            this.btnFarkliKaydet = new DevExpress.XtraBars.BarButtonItem();
            this.btnXmlYukle = new DevExpress.XtraBars.BarButtonItem();
            this.btnXmlSifirla = new DevExpress.XtraBars.BarButtonItem();
            this.btnYenile = new DevExpress.XtraBars.BarButtonItem();
            this.btnDevTools = new DevExpress.XtraBars.BarButtonItem();
            this.btnHakkinda = new DevExpress.XtraBars.BarButtonItem();
            this.btnCikis = new DevExpress.XtraBars.BarButtonItem();
            this.btnEFatura = new DevExpress.XtraBars.BarCheckItem();
            this.btnEIrsaliye = new DevExpress.XtraBars.BarCheckItem();
            this.bgBelgeTuru = new DevExpress.XtraBars.BarButtonGroup();
            this.rpHome = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rpgDosya = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgXml = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgBelgeTuru = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgOnizleme = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgUygulama = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.applicationMenu = new DevExpress.XtraBars.Ribbon.ApplicationMenu(this.components);
            this.splitMain = new DevExpress.XtraEditors.SplitContainerControl();
            this.tcEditor = new DevExpress.XtraTab.XtraTabControl();
            this.tpEditor = new DevExpress.XtraTab.XtraTabPage();
            this.textEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this.tcOnizleme = new DevExpress.XtraTab.XtraTabControl();
            this.tpOnizleme = new DevExpress.XtraTab.XtraTabPage();
            this.lblStatus = new System.Windows.Forms.Label();
            this.dlgAc = new System.Windows.Forms.OpenFileDialog();
            this.dlgKaydet = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcEditor)).BeginInit();
            this.tcEditor.SuspendLayout();
            this.tpEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcOnizleme)).BeginInit();
            this.tcOnizleme.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.ribbonControl.SearchEditItem,
            this.btnYeni,
            this.btnAc,
            this.btnKaydet,
            this.btnFarkliKaydet,
            this.btnXmlYukle,
            this.btnXmlSifirla,
            this.btnYenile,
            this.btnDevTools,
            this.btnHakkinda,
            this.btnCikis,
            this.btnEFatura,
            this.btnEIrsaliye,
            this.bgBelgeTuru});
            this.ribbonControl.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl.MaxItemId = 61;
            this.ribbonControl.Name = "ribbonControl";
            this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpHome});
            this.ribbonControl.QuickToolbarItemLinks.Add(this.btnAc);
            this.ribbonControl.QuickToolbarItemLinks.Add(this.btnKaydet);
            this.ribbonControl.QuickToolbarItemLinks.Add(this.btnXmlYukle);
            this.ribbonControl.QuickToolbarItemLinks.Add(this.btnYenile);
            this.ribbonControl.QuickToolbarItemLinks.Add(this.btnDevTools);
            this.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2019;
            this.ribbonControl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl.Size = new System.Drawing.Size(1280, 158);
            // 
            // btnYeni
            // 
            this.btnYeni.Id = 60;
            this.btnYeni.Name = "btnYeni";
            // 
            // btnAc
            // 
            this.btnAc.Caption = "XSLT Aç";
            this.btnAc.Hint = "XSLT tasarım dosyası aç (Ctrl+O)";
            this.btnAc.Id = 2;
            this.btnAc.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAc.ImageOptions.Image")));
            this.btnAc.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnAc.ImageOptions.LargeImage")));
            this.btnAc.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O));
            this.btnAc.Name = "btnAc";
            this.btnAc.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnAc.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAc_ItemClick);
            // 
            // btnKaydet
            // 
            this.btnKaydet.Caption = "Kaydet";
            this.btnKaydet.Enabled = false;
            this.btnKaydet.Hint = "Dosyayı kaydet (Ctrl+S)";
            this.btnKaydet.Id = 3;
            this.btnKaydet.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnKaydet.ImageOptions.Image")));
            this.btnKaydet.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnKaydet.ImageOptions.LargeImage")));
            this.btnKaydet.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S));
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnKaydet.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnKaydet_ItemClick);
            // 
            // btnFarkliKaydet
            // 
            this.btnFarkliKaydet.Caption = "Farklı Kaydet";
            this.btnFarkliKaydet.Enabled = false;
            this.btnFarkliKaydet.Hint = "Farklı konuma kaydet";
            this.btnFarkliKaydet.Id = 4;
            this.btnFarkliKaydet.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnFarkliKaydet.ImageOptions.Image")));
            this.btnFarkliKaydet.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnFarkliKaydet.ImageOptions.LargeImage")));
            this.btnFarkliKaydet.Name = "btnFarkliKaydet";
            this.btnFarkliKaydet.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            this.btnFarkliKaydet.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnFarkliKaydet_ItemClick);
            // 
            // btnXmlYukle
            // 
            this.btnXmlYukle.Caption = "XML Yükle";
            this.btnXmlYukle.Hint = "Önizleme için kaynak XML yükle";
            this.btnXmlYukle.Id = 5;
            this.btnXmlYukle.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnXmlYukle.ImageOptions.Image")));
            this.btnXmlYukle.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnXmlYukle.ImageOptions.LargeImage")));
            this.btnXmlYukle.Name = "btnXmlYukle";
            this.btnXmlYukle.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnXmlYukle.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnXmlYukle_ItemClick);
            // 
            // btnXmlSifirla
            // 
            this.btnXmlSifirla.Caption = "XML Sıfırla";
            this.btnXmlSifirla.Hint = "Default UBL XML\'e dön";
            this.btnXmlSifirla.Id = 6;
            this.btnXmlSifirla.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnXmlSifirla.ImageOptions.Image")));
            this.btnXmlSifirla.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnXmlSifirla.ImageOptions.LargeImage")));
            this.btnXmlSifirla.Name = "btnXmlSifirla";
            this.btnXmlSifirla.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            this.btnXmlSifirla.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnXmlSifirla_ItemClick);
            // 
            // btnYenile
            // 
            this.btnYenile.Caption = "Yenile";
            this.btnYenile.Enabled = false;
            this.btnYenile.Hint = "Önizlemeyi yenile (F5)";
            this.btnYenile.Id = 7;
            this.btnYenile.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnYenile.ImageOptions.Image")));
            this.btnYenile.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnYenile.ImageOptions.LargeImage")));
            this.btnYenile.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.btnYenile.Name = "btnYenile";
            this.btnYenile.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnYenile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnYenile_ItemClick);
            // 
            // btnDevTools
            // 
            this.btnDevTools.Caption = "Dev Tools";
            this.btnDevTools.Enabled = false;
            this.btnDevTools.Hint = "Geliştirici araçlarını aç (F12)";
            this.btnDevTools.Id = 8;
            this.btnDevTools.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDevTools.ImageOptions.Image")));
            this.btnDevTools.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnDevTools.ImageOptions.LargeImage")));
            this.btnDevTools.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F12);
            this.btnDevTools.Name = "btnDevTools";
            this.btnDevTools.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            this.btnDevTools.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDevTools_ItemClick);
            // 
            // btnHakkinda
            // 
            this.btnHakkinda.Caption = "Hakkında";
            this.btnHakkinda.Hint = "Lisans ve sürüm bilgileri";
            this.btnHakkinda.Id = 9;
            this.btnHakkinda.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnHakkinda.ImageOptions.Image")));
            this.btnHakkinda.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnHakkinda.ImageOptions.LargeImage")));
            this.btnHakkinda.Name = "btnHakkinda";
            this.btnHakkinda.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            this.btnHakkinda.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnHakkinda_ItemClick);
            // 
            // btnCikis
            // 
            this.btnCikis.Caption = "Çıkış";
            this.btnCikis.Hint = "Programı kapat (Alt+F4)";
            this.btnCikis.Id = 10;
            this.btnCikis.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCikis.ImageOptions.Image")));
            this.btnCikis.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnCikis.ImageOptions.LargeImage")));
            this.btnCikis.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4));
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            this.btnCikis.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCikis_ItemClick);
            // 
            // btnEFatura
            // 
            this.btnEFatura.BindableChecked = true;
            this.btnEFatura.Caption = "e-Fatura";
            this.btnEFatura.Checked = true;
            this.btnEFatura.GroupIndex = 1;
            this.btnEFatura.Hint = "e-Fatura / e-Arşiv XSLT şablonu";
            this.btnEFatura.Id = 11;
            this.btnEFatura.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnEFatura.ImageOptions.Image")));
            this.btnEFatura.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnEFatura.ImageOptions.LargeImage")));
            this.btnEFatura.Name = "btnEFatura";
            this.btnEFatura.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnEFatura.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.btnBelgeTuru_CheckedChanged);
            // 
            // btnEIrsaliye
            // 
            this.btnEIrsaliye.Caption = "e-İrsaliye";
            this.btnEIrsaliye.GroupIndex = 1;
            this.btnEIrsaliye.Hint = "e-İrsaliye XSLT şablonu";
            this.btnEIrsaliye.Id = 12;
            this.btnEIrsaliye.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnEIrsaliye.ImageOptions.Image")));
            this.btnEIrsaliye.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnEIrsaliye.ImageOptions.LargeImage")));
            this.btnEIrsaliye.Name = "btnEIrsaliye";
            this.btnEIrsaliye.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnEIrsaliye.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.btnBelgeTuru_CheckedChanged);
            // 
            // bgBelgeTuru
            // 
            this.bgBelgeTuru.Caption = "Belge Türü";
            this.bgBelgeTuru.Id = 13;
            this.bgBelgeTuru.ItemLinks.Add(this.btnEFatura);
            this.bgBelgeTuru.ItemLinks.Add(this.btnEIrsaliye);
            this.bgBelgeTuru.Name = "bgBelgeTuru";
            // 
            // rpHome
            // 
            this.rpHome.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rpgDosya,
            this.rpgXml,
            this.rpgBelgeTuru,
            this.rpgOnizleme,
            this.rpgUygulama});
            this.rpHome.Name = "rpHome";
            this.rpHome.Text = "Araçlar";
            // 
            // rpgDosya
            // 
            this.rpgDosya.ItemLinks.Add(this.btnYeni);
            this.rpgDosya.ItemLinks.Add(this.btnAc);
            this.rpgDosya.ItemLinks.Add(this.btnKaydet);
            this.rpgDosya.ItemLinks.Add(this.btnFarkliKaydet);
            this.rpgDosya.Name = "rpgDosya";
            this.rpgDosya.Text = "XSLT Dosya";
            // 
            // rpgXml
            // 
            this.rpgXml.ItemLinks.Add(this.btnXmlYukle);
            this.rpgXml.ItemLinks.Add(this.btnXmlSifirla);
            this.rpgXml.Name = "rpgXml";
            this.rpgXml.Text = "Kaynak XML";
            // 
            // rpgBelgeTuru
            // 
            this.rpgBelgeTuru.ItemLinks.Add(this.bgBelgeTuru);
            this.rpgBelgeTuru.Name = "rpgBelgeTuru";
            this.rpgBelgeTuru.Text = "Belge Türü";
            // 
            // rpgOnizleme
            // 
            this.rpgOnizleme.ItemLinks.Add(this.btnYenile);
            this.rpgOnizleme.ItemLinks.Add(this.btnDevTools);
            this.rpgOnizleme.Name = "rpgOnizleme";
            this.rpgOnizleme.Text = "Önizleme";
            // 
            // rpgUygulama
            // 
            this.rpgUygulama.ItemLinks.Add(this.btnHakkinda);
            this.rpgUygulama.ItemLinks.Add(this.btnCikis);
            this.rpgUygulama.Name = "rpgUygulama";
            this.rpgUygulama.Text = "Uygulama";
            // 
            // applicationMenu
            // 
            this.applicationMenu.ItemLinks.Add(this.btnAc);
            this.applicationMenu.ItemLinks.Add(this.btnKaydet);
            this.applicationMenu.ItemLinks.Add(this.btnFarkliKaydet);
            this.applicationMenu.ItemLinks.Add(this.btnHakkinda);
            this.applicationMenu.ItemLinks.Add(this.btnCikis);
            this.applicationMenu.Name = "applicationMenu";
            this.applicationMenu.Ribbon = this.ribbonControl;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 158);
            this.splitMain.Name = "splitMain";
            this.splitMain.Panel1.Controls.Add(this.tcEditor);
            this.splitMain.Panel1.Text = "Editör";
            this.splitMain.Panel2.Controls.Add(this.tcOnizleme);
            this.splitMain.Panel2.Text = "Önizleme";
            this.splitMain.Size = new System.Drawing.Size(1280, 568);
            this.splitMain.SplitterPosition = 700;
            this.splitMain.TabIndex = 1;
            // 
            // tcEditor
            // 
            this.tcEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcEditor.Location = new System.Drawing.Point(0, 0);
            this.tcEditor.Name = "tcEditor";
            this.tcEditor.SelectedTabPage = this.tpEditor;
            this.tcEditor.Size = new System.Drawing.Size(700, 568);
            this.tcEditor.TabIndex = 0;
            this.tcEditor.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tpEditor});
            // 
            // tpEditor
            // 
            this.tpEditor.Controls.Add(this.textEditor);
            this.tpEditor.Name = "tpEditor";
            this.tpEditor.Size = new System.Drawing.Size(698, 543);
            this.tpEditor.Text = "(dosya açılmadı)";
            // 
            // textEditor
            // 
            this.textEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditor.IsReadOnly = false;
            this.textEditor.Location = new System.Drawing.Point(0, 0);
            this.textEditor.Name = "textEditor";
            this.textEditor.ShowVRuler = false;
            this.textEditor.Size = new System.Drawing.Size(698, 543);
            this.textEditor.TabIndex = 0;
            this.textEditor.TextChanged += new System.EventHandler(this.textEditor_TextChanged);
            // 
            // tcOnizleme
            // 
            this.tcOnizleme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcOnizleme.Location = new System.Drawing.Point(0, 0);
            this.tcOnizleme.Name = "tcOnizleme";
            this.tcOnizleme.SelectedTabPage = this.tpOnizleme;
            this.tcOnizleme.Size = new System.Drawing.Size(570, 568);
            this.tcOnizleme.TabIndex = 0;
            this.tcOnizleme.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tpOnizleme});
            // 
            // tpOnizleme
            // 
            this.tpOnizleme.Name = "tpOnizleme";
            this.tpOnizleme.Size = new System.Drawing.Size(568, 543);
            this.tpOnizleme.Text = "Önizleme";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblStatus.Location = new System.Drawing.Point(0, 726);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1280, 22);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Hazır";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dlgAc
            // 
            this.dlgAc.Filter = "XSLT Dosyası (*.xslt;*.xsl)|*.xslt;*.xsl|Tüm Dosyalar (*.*)|*.*";
            this.dlgAc.Title = "XSLT Dosyası Seç";
            // 
            // dlgKaydet
            // 
            this.dlgKaydet.DefaultExt = "xslt";
            this.dlgKaydet.Filter = "XSLT Dosyası (*.xslt)|*.xslt|XSL Dosyası (*.xsl)|*.xsl";
            this.dlgKaydet.Title = "Farklı Kaydet";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 748);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.ribbonControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("EditorForm.IconOptions.Image")));
            this.IconOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("EditorForm.IconOptions.LargeImage")));
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "EditorForm";
            this.Ribbon = this.ribbonControl;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mutlu Yazılım - XSLT Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditorForm_FormClosed);
            this.Load += new System.EventHandler(this.EditorForm_Load);
            this.Resize += new System.EventHandler(this.EditorForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tcEditor)).EndInit();
            this.tcEditor.ResumeLayout(false);
            this.tpEditor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tcOnizleme)).EndInit();
            this.tcOnizleme.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RibbonControl ribbonControl;
        private ApplicationMenu applicationMenu;
        private RibbonPage rpHome;
        private RibbonPageGroup rpgDosya;
        private RibbonPageGroup rpgXml;
        private RibbonPageGroup rpgBelgeTuru;
        private RibbonPageGroup rpgOnizleme;
        private RibbonPageGroup rpgUygulama;
        private BarButtonItem btnYeni;
        private BarButtonItem btnAc;
        private BarButtonItem btnKaydet;
        private BarButtonItem btnFarkliKaydet;
        private BarButtonItem btnXmlYukle;
        private BarButtonItem btnXmlSifirla;
        private BarButtonItem btnYenile;
        private BarButtonItem btnDevTools;
        private BarButtonItem btnHakkinda;
        private BarButtonItem btnCikis;
        private BarCheckItem btnEFatura;
        private BarCheckItem btnEIrsaliye;
        private BarButtonGroup bgBelgeTuru;
        private SplitContainerControl splitMain;
        private XtraTabControl tcEditor;
        private XtraTabPage tpEditor;
        private XtraTabControl tcOnizleme;
        private XtraTabPage tpOnizleme;
        private TextEditorControl textEditor;
        private Label lblStatus;
        private OpenFileDialog dlgAc;
        private SaveFileDialog dlgKaydet;
    }
}