using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using XSLTEditor.Classes;

namespace XSLTEditor.Forms
{
    public partial class EditorForm : RibbonForm
    {
        private ChromiumWebBrowser browser;
        private string aktifXsltYolu = string.Empty;
        private Timer syntaxTimer;
        private CodeCompletionWindow completionWindow;
        private ToolTip editorTooltip;
        private string sonTooltipMesaj = string.Empty;
        public EditorForm()
        {
            InitializeComponent();
            LogManager.Bilgi("Uygulama başlatıldı.");
        }
        // ── Form events ──────────────────────────────────────────────────────
        private void EditorForm_Load(object sender, EventArgs e)
        {
            EditorAyarla();
            BrowserBaslat();
            SetStatus("Hazır  |  " + BelgeTuruAdi() + "  |  Aktif XML: " + InvoiceLoader.AktifXmlAdi(AktifBelgeTuru()));
            LogManager.Bilgi("EditorForm yüklendi.");
        }
        private void EditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogManager.Bilgi("Uygulama kapatıldı.");
            syntaxTimer?.Stop();
            syntaxTimer?.Dispose();
            editorTooltip?.Dispose();
            try { Cef.Shutdown(); } catch { }
            Application.Exit();
        }
        private void EditorForm_Resize(object sender, EventArgs e)
        {
            BrowserYenidenBoyutlandir();
        }
        private void BrowserYenidenBoyutlandir()
        {
            if (browser != null && !browser.IsDisposed)
            {
                browser.Invalidate();
                try { browser.GetBrowserHost()?.NotifyMoveOrResizeStarted(); } catch { }
            }
        }
        // ── ICSharpCode Editör Ayarları ──────────────────────────────────────
        private void EditorAyarla()
        {
            try
            {
                this.textEditor.SetHighlighting("XML");
                this.textEditor.ShowLineNumbers = true;
                this.textEditor.ShowVRuler = false;
                this.textEditor.ShowInvalidLines = false;
                this.textEditor.ShowSpaces = false;
                this.textEditor.ShowTabs = false;
                this.textEditor.EnableFolding = true;
                this.textEditor.Font = new Font("Consolas", 11F);
                this.textEditor.Document.FoldingManager.FoldingStrategy =
                    new ICSharpCode.TextEditor.Document.IndentFoldingStrategy();
                this.textEditor.ActiveTextAreaControl.TextArea.KeyDown
                    += new System.Windows.Forms.KeyEventHandler(this.textEditor_KeyDown);
                this.textEditor.ActiveTextAreaControl.TextArea.KeyPress
                    += new KeyPressEventHandler(this.textEditor_KeyPress);
                editorTooltip = new ToolTip
                {
                    AutomaticDelay = 400,
                    AutoPopDelay = 6000,
                    InitialDelay = 400,
                    ReshowDelay = 200,
                    ShowAlways = true,
                    ToolTipIcon = ToolTipIcon.Error,
                    ToolTipTitle = "Sözdizimi Hatası"
                };
                this.textEditor.ActiveTextAreaControl.TextArea.MouseMove
                    += new MouseEventHandler(this.textEditor_MouseMove);
                syntaxTimer = new Timer();
                syntaxTimer.Interval = 800;
                syntaxTimer.Tick += (s, ev) =>
                {
                    syntaxTimer.Stop();
                    var hatalar = XmlSyntaxChecker.Kontrol(this.textEditor);
                    if (hatalar.Count > 0)
                        SetStatus(string.Format("⚠  Satır {0}, Sütun {1}: {2}",
                            hatalar[0].Satir, hatalar[0].Sutun, hatalar[0].Mesaj));
                    else if (!string.IsNullOrEmpty(aktifXsltYolu))
                        SetStatus(string.Format("✓ Geçerli XML  |  {0}  |  {1}  |  Aktif XML: {2}",
                            BelgeTuruAdi(), Path.GetFileName(aktifXsltYolu),
                            InvoiceLoader.AktifXmlAdi(AktifBelgeTuru())));
                };
                LogManager.Bilgi("Editör ayarları uygulandı.");
            }
            catch (Exception ex)
            {
                LogManager.Hata("Editör ayarlama hatası", ex);
            }
        }
        // ── Tooltip — mouse hover ─────────────────────────────────────────────
        private void textEditor_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                SyntaxError hata = XmlSyntaxChecker.HataAl(this.textEditor, e.Location);
                if (hata != null)
                {
                    string mesaj = string.Format("Satır {0}, Sütun {1}:\n{2}",
                        hata.Satir, hata.Sutun, hata.Mesaj);
                    if (mesaj != sonTooltipMesaj)
                    {
                        sonTooltipMesaj = mesaj;
                        editorTooltip.Show(mesaj,
                            this.textEditor.ActiveTextAreaControl.TextArea,
                            e.X + 14, e.Y + 14, 6000);
                    }
                }
                else if (!string.IsNullOrEmpty(sonTooltipMesaj))
                {
                    sonTooltipMesaj = string.Empty;
                    editorTooltip.Hide(this.textEditor.ActiveTextAreaControl.TextArea);
                }
            }
            catch { }
        }
        // ── CefSharp ─────────────────────────────────────────────────────────
        private void BrowserBaslat()
        {
            try
            {
                if (!Cef.IsInitialized)
                {
                    CefSettings settings = new CefSettings();
                    settings.Locale = "tr";
                    settings.CefCommandLineArgs["disable-gpu-compositing"] = "1";
                    settings.CefCommandLineArgs["disable-gpu"] = "1";
                    Cef.Initialize(settings);
                }
                browser = new ChromiumWebBrowser("about:blank");
                browser.Dock = DockStyle.Fill;
                this.tpOnizleme.Controls.Clear();
                this.tpOnizleme.Controls.Add(browser);
                this.btnDevTools.Enabled = true;
                LogManager.Bilgi("CefSharp başlatıldı.");
            }
            catch (Exception ex)
            {
                LogManager.Hata("CefSharp başlatma hatası", ex);
            }
        }
        // ── Otomatik Tamamlama ────────────────────────────────────────────────
        private void textEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '<' || e.KeyChar == ':')
                TamamlamaAc();
        }
        private void TamamlamaAc()
        {
            try
            {
                if (completionWindow != null) { completionWindow.Close(); completionWindow = null; }
                completionWindow = CodeCompletionWindow.ShowCompletionWindow(
                    this, this.textEditor,
                    aktifXsltYolu ?? "untitled.xslt",
                    new XsltCompletionProvider(), '\0');
                if (completionWindow != null)
                    completionWindow.Closed += (s, e) => completionWindow = null;
            }
            catch (Exception ex) { LogManager.Hata("Tamamlama hatası", ex); }
        }
        // ── Kısayollar ────────────────────────────────────────────────────────
        private void textEditor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S) { e.Handled = true; btnKaydet_ItemClick(sender, null); }
            else if (e.KeyCode == Keys.F5) { e.Handled = true; DonusumYapVeGoster(); }
            else if (e.Control && e.KeyCode == Keys.Space) { e.Handled = true; TamamlamaAc(); }
            else if (e.Control && e.KeyCode == Keys.F) { e.Handled = true; AraDialogGoster(); }
        }
        // ── XSLT Aç ──────────────────────────────────────────────────────────
        private void btnAc_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.dlgAc.ShowDialog() != DialogResult.OK) return;
            try
            {
                aktifXsltYolu = this.dlgAc.FileName;
                this.textEditor.LoadFile(aktifXsltYolu, false, true);
                this.tpEditor.Text = Path.GetFileName(aktifXsltYolu);
                this.btnKaydet.Enabled = false;
                this.btnFarkliKaydet.Enabled = true;
                this.btnYenile.Enabled = true;
                GuncelleFolding();
                BelgeTuru algilanan = XsltBelgeTuruAlgila(aktifXsltYolu);
                if (algilanan == BelgeTuru.EFatura)
                {
                    this.btnEFatura.Checked = true;
                    this.btnEIrsaliye.Checked = false;
                }
                else
                {
                    this.btnEFatura.Checked = false;
                    this.btnEIrsaliye.Checked = true;
                }
                var hatalar = XmlSyntaxChecker.Kontrol(this.textEditor);
                LogManager.Bilgi(string.Format("XSLT açıldı: {0}  |  Algılanan: {1}", aktifXsltYolu, algilanan));
                DonusumYapVeGoster();
                SetStatus(hatalar.Count > 0
                    ? string.Format("⚠  Satır {0}: {1}", hatalar[0].Satir, hatalar[0].Mesaj)
                    : string.Format("✓ Açıldı: {0}  |  {1} (otomatik algılandı)  |  Aktif XML: {2}",
                        Path.GetFileName(aktifXsltYolu), BelgeTuruAdi(),
                        InvoiceLoader.AktifXmlAdi(AktifBelgeTuru())));
            }
            catch (Exception ex)
            {
                LogManager.Hata("XSLT açma hatası", ex);
                MessageBox.Show("Dosya açılamadı:\n" + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private BelgeTuru XsltBelgeTuruAlgila(string xsltYolu)
        {
            try
            {
                string icerik = File.ReadAllText(xsltYolu);
                if (icerik.Contains("DespatchAdvice-2") ||
                    icerik.Contains("DespatchAdvice") ||
                    icerik.Contains("n1:DespatchAdvice") ||
                    icerik.Contains("IRSubl") ||
                    icerik.IndexOf("irsaliye", StringComparison.OrdinalIgnoreCase) >= 0)
                    return BelgeTuru.EIrsaliye;
                if (icerik.Contains("Invoice-2") ||
                    icerik.Contains("n1:Invoice") ||
                    icerik.Contains("FATubl") ||
                    icerik.IndexOf("fatura", StringComparison.OrdinalIgnoreCase) >= 0)
                    return BelgeTuru.EFatura;
            }
            catch (Exception ex)
            {
                LogManager.Hata("XSLT belge türü algılama hatası", ex);
            }
            return BelgeTuru.EFatura;
        }
        // ── XML Yükle ────────────────────────────────────────────────────────
        private void btnXmlYukle_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "XML Dosyası (*.xml)|*.xml|Tüm Dosyalar (*.*)|*.*";
                dlg.Title = "Kaynak XML Seç";
                if (dlg.ShowDialog() != DialogResult.OK) return;
                if (InvoiceLoader.XmlYukle(dlg.FileName))
                {
                    BelgeTuru algilanan = XmlBelgeTuruAlgila(dlg.FileName);
                    if (algilanan == BelgeTuru.EFatura)
                    {
                        this.btnEFatura.Checked = true;
                        this.btnEIrsaliye.Checked = false;
                    }
                    else
                    {
                        this.btnEFatura.Checked = false;
                        this.btnEIrsaliye.Checked = true;
                    }
                    SetStatus(string.Format("✓ XML yüklendi: {0}  |  Otomatik algılandı: {1}",
                        Path.GetFileName(dlg.FileName), BelgeTuruAdi()));
                    LogManager.Bilgi(string.Format("XML belge türü algılandı: {0}", algilanan));
                    if (!string.IsNullOrEmpty(aktifXsltYolu)) DonusumYapVeGoster();
                }
                else
                    XtraMessageBox.Show("XML yüklenemedi.\nLOG klasörünü kontrol edin.",
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private BelgeTuru XmlBelgeTuruAlgila(string xmlYolu)
        {
            try
            {
                using (var reader = System.Xml.XmlReader.Create(xmlYolu,
                    new System.Xml.XmlReaderSettings
                    {
                        IgnoreWhitespace = true,
                        IgnoreComments = true
                    }))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == System.Xml.XmlNodeType.Element)
                        {
                            string localName = reader.LocalName;
                            if (localName.Equals("DespatchAdvice", StringComparison.OrdinalIgnoreCase))
                                return BelgeTuru.EIrsaliye;
                            if (localName.Equals("Invoice", StringComparison.OrdinalIgnoreCase))
                                return BelgeTuru.EFatura;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Hata("Belge türü algılama hatası", ex);
            }
            return BelgeTuru.EFatura;
        }
        // ── XML Sıfırla ──────────────────────────────────────────────────────
        private void btnXmlSifirla_ItemClick(object sender, ItemClickEventArgs e)
        {
            InvoiceLoader.XmlSifirla();
            SetStatus(string.Format("Default XML'e döndü  |  Aktif XML: {0}",
                InvoiceLoader.AktifXmlAdi(AktifBelgeTuru())));
            if (!string.IsNullOrEmpty(aktifXsltYolu)) DonusumYapVeGoster();
        }
        // ── Kaydet ───────────────────────────────────────────────────────────
        private void btnKaydet_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (string.IsNullOrEmpty(aktifXsltYolu)) { btnFarkliKaydet_ItemClick(sender, null); return; }
            try
            {
                this.textEditor.SaveFile(aktifXsltYolu);
                this.btnKaydet.Enabled = false;
                LogManager.Bilgi(string.Format("Kaydedildi: {0}", aktifXsltYolu));
                DonusumYapVeGoster();
                SetStatus(string.Format("Kaydedildi: {0}  |  {1}  |  Aktif XML: {2}",
                    Path.GetFileName(aktifXsltYolu), BelgeTuruAdi(),
                    InvoiceLoader.AktifXmlAdi(AktifBelgeTuru())));
            }
            catch (Exception ex)
            {
                LogManager.Hata("Kaydetme hatası", ex);
                MessageBox.Show("Kaydetme hatası:\n" + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // ── Farklı Kaydet ────────────────────────────────────────────────────
        private void btnFarkliKaydet_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.dlgKaydet.ShowDialog() != DialogResult.OK) return;
            try
            {
                this.textEditor.SaveFile(this.dlgKaydet.FileName);
                aktifXsltYolu = this.dlgKaydet.FileName;
                this.tpEditor.Text = Path.GetFileName(aktifXsltYolu);
                this.btnKaydet.Enabled = false;
                LogManager.Bilgi(string.Format("Farklı kaydedildi: {0}", aktifXsltYolu));
                SetStatus(string.Format("Farklı kaydedildi: {0}", Path.GetFileName(aktifXsltYolu)));
            }
            catch (Exception ex)
            {
                LogManager.Hata("Farklı kaydetme hatası", ex);
                MessageBox.Show("Kaydetme hatası:\n" + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // ── Önizleme ─────────────────────────────────────────────────────────
        private void btnYenile_ItemClick(object sender, ItemClickEventArgs e) => DonusumYapVeGoster();
        private void btnDevTools_ItemClick(object sender, ItemClickEventArgs e)
        {
            try { browser?.ShowDevTools(); }
            catch (Exception ex) { LogManager.Hata("DevTools hatası", ex); }
        }
        // ── Belge türü ────────────────────────────────────────────────────────
        private void btnBelgeTuru_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            SetStatus(string.Format("Belge türü: {0}  |  Aktif XML: {1}",
                BelgeTuruAdi(), InvoiceLoader.AktifXmlAdi(AktifBelgeTuru())));
            if (!string.IsNullOrEmpty(aktifXsltYolu)) DonusumYapVeGoster();
        }
        // ── Hakkında ─────────────────────────────────────────────────────────
        private void btnHakkinda_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraMessageBox.Show(
                "Mutlu Yazılım - XSLT Editor\n\n" +
                "e-Belge (e-Fatura / e-İrsaliye) XSLT Tasarım Aracı\n\n" +
                "Sürüm       : 1.0.0\n" +
                "Platform    : .NET Framework 4.8\n" +
                "XSLT Destek : 1.0 (Native) + 2.0 (Saxon-HE)\n\n" +
                "www.mutluyazilim.com.tr",
                "Mutlu Yazılım - Hakkında", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // ── Çıkış ────────────────────────────────────────────────────────────
        private void btnCikis_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Uygulamadan çıkmak istediğinize emin misiniz?",
                    "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LogManager.Bilgi("Kullanıcı çıkış yaptı.");
                try { Cef.Shutdown(); } catch { }
                Application.Exit();
            }
        }
        // ── TextChanged ──────────────────────────────────────────────────────
        private void textEditor_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(aktifXsltYolu)) this.btnKaydet.Enabled = true;
            GuncelleFolding();
            syntaxTimer?.Stop();
            syntaxTimer?.Start();
        }
        // ── Dönüşüm ──────────────────────────────────────────────────────────
        private void DonusumYapVeGoster()
        {
            if (string.IsNullOrEmpty(aktifXsltYolu)) return;
            try
            {
                BelgeTuru tur = AktifBelgeTuru();
                string xmlYolu = InvoiceLoader.AktifXmlYoluAl(tur);
                if (xmlYolu == null)
                {
                    SetStatus(string.Format("✗ XML bulunamadı  |  {0}", BelgeTuruAdi()));
                    return;
                }
                string htmlYolu = XsltTransformer.Donustur(xmlYolu, aktifXsltYolu);
                if (browser != null && !browser.IsDisposed)
                    browser.Load("file:///" + htmlYolu.Replace("\\", "/"));
                SetStatus(string.Format("✓ Dönüşüm başarılı  |  {0}  |  {1}  |  Aktif XML: {2}",
                    BelgeTuruAdi(), Path.GetFileName(aktifXsltYolu), Path.GetFileName(xmlYolu)));
            }
            catch (Exception ex)
            {
                SetStatus("✗ Dönüşüm hatası: " + ex.Message);
                LogManager.Hata("Dönüşüm hatası", ex);
                XtraMessageBox.Show("XSL dönüşüm hatası:\n" + ex.Message,
                    "Dönüşüm Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // ── Ara (Ctrl+F) ──────────────────────────────────────────────────────
        private string _sonArama = string.Empty;
        private int _sonAramaOffset = 0;
        private void AraDialogGoster()
        {
            using (Form frm = new Form())
            {
                frm.Text = "Ara";
                frm.Size = new Size(380, 110);
                frm.FormBorderStyle = FormBorderStyle.FixedDialog;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                TextBox txtAra = new TextBox { Left = 10, Top = 12, Width = 250, Text = _sonArama };
                Button btnBul = new Button { Left = 270, Top = 10, Width = 85, Height = 25, Text = "Bul" };
                CheckBox chkCase = new CheckBox { Left = 10, Top = 44, Text = "Büyük/Küçük Harf Duyarlı" };
                frm.Controls.AddRange(new Control[] { txtAra, btnBul, chkCase });
                frm.AcceptButton = btnBul;
                btnBul.Click += (s, ev) =>
                {
                    string aranan = txtAra.Text;
                    if (string.IsNullOrEmpty(aranan)) return;
                    _sonArama = aranan;
                    var comp = chkCase.Checked ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
                    string icerik = this.textEditor.Document.TextContent;
                    int idx = icerik.IndexOf(aranan, _sonAramaOffset, comp);
                    if (idx < 0 && _sonAramaOffset > 0)
                    {
                        _sonAramaOffset = 0;
                        idx = icerik.IndexOf(aranan, 0, comp);
                    }
                    if (idx >= 0)
                    {
                        _sonAramaOffset = idx + aranan.Length;
                        var doc = this.textEditor.Document;
                        TextLocation bas = doc.OffsetToPosition(idx);
                        TextLocation son = doc.OffsetToPosition(idx + aranan.Length);
                        this.textEditor.ActiveTextAreaControl.JumpTo(bas.Line, bas.Column);
                        this.textEditor.ActiveTextAreaControl.SelectionManager.SetSelection(
                            new ICSharpCode.TextEditor.Document.DefaultSelection(doc,
                                new ICSharpCode.TextEditor.TextLocation(bas.Column, bas.Line),
                                new ICSharpCode.TextEditor.TextLocation(son.Column, son.Line)));
                        this.textEditor.ActiveTextAreaControl.TextArea.Focus();
                    }
                    else
                    {
                        _sonAramaOffset = 0;
                        XtraMessageBox.Show(string.Format("'{0}' bulunamadı.", aranan),
                            "Ara", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                };
                frm.KeyDown += (s, ev) => { if (ev.KeyCode == Keys.Escape) frm.Close(); };
                frm.ShowDialog(this);
            }
        }
        // ── Yardımcılar ──────────────────────────────────────────────────────
        private void GuncelleFolding()
        {
            try { this.textEditor.Document.FoldingManager.UpdateFoldings(null, null); } catch { }
        }
        private void SetStatus(string mesaj)
        {
            if (this.lblStatus.InvokeRequired)
                this.lblStatus.Invoke(new Action(() => this.lblStatus.Text = "  " + mesaj));
            else
                this.lblStatus.Text = "  " + mesaj;
        }
        private BelgeTuru AktifBelgeTuru()
            => this.btnEFatura.Checked ? BelgeTuru.EFatura : BelgeTuru.EIrsaliye;
        private string BelgeTuruAdi()
            => this.btnEFatura.Checked ? "e-Fatura" : "e-İrsaliye";
    }
}