using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace XSLTEditor.Classes
{
    /// <summary>
    /// XSLT/XML sözdizimi kontrolü.
    /// Hata varsa:
    ///   1) Kırmızı dalgalı alt çizgi (WaveLine marker)
    ///   2) Sol kenar kırmızı çizgi (gutter marker — BookmarkManager)
    ///   3) Mouse hover → tooltip
    /// Hata gidince tüm işaretler temizlenir.
    /// </summary>
    public static class XmlSyntaxChecker
    {
        // Son bulunan hatalar — tooltip için tutulur
        private static readonly List<SyntaxError> _aktifHatalar = new List<SyntaxError>();
        public static IReadOnlyList<SyntaxError> AktifHatalar => _aktifHatalar;
        /// <summary>
        /// Editördeki XML'i parse eder, hataları işaretler.
        /// </summary>
        public static List<SyntaxError> Kontrol(TextEditorControl editor)
        {
            _aktifHatalar.Clear();
            try
            {
                var doc = editor.Document;
                var marker = doc.MarkerStrategy;
                // Önceki tüm hata marker'larını temizle
                marker.RemoveAll(m => m.TextMarkerType == TextMarkerType.WaveLine);
                // Önceki gutter marker'larını temizle
                doc.BookmarkManager.RemoveMarks(b => b is ErrorBookmark);
                string metin = doc.TextContent;
                if (string.IsNullOrWhiteSpace(metin))
                    return _aktifHatalar;
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(metin);
                    // Hatasız — marker yok
                    editor.ActiveTextAreaControl.TextArea.Invalidate();
                    return _aktifHatalar;
                }
                catch (XmlException ex)
                {
                    int satir = Math.Max(ex.LineNumber - 1, 0);
                    int sutun = Math.Max(ex.LinePosition - 1, 0);
                    if (satir < doc.TotalNumberOfLines)
                    {
                        LineSegment lineSegment = doc.GetLineSegment(satir);
                        int offset = lineSegment.Offset + Math.Min(sutun, Math.Max(lineSegment.Length - 1, 0));
                        int uzunluk = Math.Max(lineSegment.Length - sutun, 1);
                        // 1) Kırmızı dalgalı alt çizgi
                        TextMarker waveMarker = new TextMarker(
                            offset, uzunluk,
                            TextMarkerType.WaveLine,
                            Color.Red);
                        marker.AddMarker(waveMarker);
                        // 2) Sol kenar kırmızı gutter marker
                        ErrorBookmark bookmark = new ErrorBookmark(doc, new TextLocation(sutun, satir), ex.Message);
                        doc.BookmarkManager.AddMark(bookmark);
                        SyntaxError hata = new SyntaxError
                        {
                            Satir = satir + 1,
                            Sutun = sutun + 1,
                            Mesaj = ex.Message,
                            Offset = offset
                        };
                        _aktifHatalar.Add(hata);
                        LogManager.Uyari($"XML hata — Satır {hata.Satir}, Sütun {hata.Sutun}: {hata.Mesaj}");
                    }
                }
                editor.ActiveTextAreaControl.TextArea.Invalidate();
            }
            catch (Exception ex)
            {
                LogManager.Hata("Sözdizimi kontrol genel hatası", ex);
            }
            return _aktifHatalar;
        }
        /// <summary>
        /// Verilen offset'e en yakın hatayı döner (tooltip için).
        /// </summary>
        public static SyntaxError HataAl(TextEditorControl editor, Point mousePos)
        {
            try
            {
                TextArea area = editor.ActiveTextAreaControl.TextArea;
                TextLocation logicPos = area.TextView.GetLogicalPosition(
                    mousePos.X - area.TextView.DrawingPosition.Left,
                    mousePos.Y - area.TextView.DrawingPosition.Top);
                foreach (SyntaxError hata in _aktifHatalar)
                {
                    if (hata.Satir - 1 == logicPos.Line)
                        return hata;
                }
            }
            catch { }
            return null;
        }
    }
    // ── Hata Bookmark (gutter kırmızı ikon) ─────────────────────────────────
    public class ErrorBookmark : Bookmark
    {
        public string HataMesaji { get; }
        public ErrorBookmark(IDocument doc, TextLocation loc, string mesaj)
            : base(doc, loc)
        {
            HataMesaji = mesaj;
        }
        public override void Draw(ICSharpCode.TextEditor.IconBarMargin margin,
                                   Graphics g, Point p)
        {
            // Kırmızı dolu daire — gutter'da görünür
            using (SolidBrush brush = new SolidBrush(Color.Red))
                g.FillEllipse(brush, p.X + 1, p.Y + 3, 10, 10);
            using (var pen = new Pen(Color.DarkRed, 1.5f))
                g.DrawEllipse(pen, p.X + 1, p.Y + 3, 10, 10);
            // İçine '!' yaz
            using (Font font = new Font("Arial", 7f, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.White))
                g.DrawString("!", font, brush, p.X + 3, p.Y + 3);
        }
        public override bool CanToggle => false;
    }
    // ── SyntaxError veri sınıfı ──────────────────────────────────────────────
    public class SyntaxError
    {
        public int Satir { get; set; }
        public int Sutun { get; set; }
        public string Mesaj { get; set; }
        public int Offset { get; set; }
    }
}