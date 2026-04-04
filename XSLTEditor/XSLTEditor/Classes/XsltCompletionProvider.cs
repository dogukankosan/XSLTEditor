using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace XSLTEditor.Classes
{
    /// <summary>
    /// XSLT ve UBL namespace tag'leri için otomatik tamamlama.
    /// Ctrl+Space veya '<' / ':' yazıldığında tetiklenir.
    /// </summary>
    public class XsltCompletionProvider : ICompletionDataProvider
    {
        // ── ICompletionDataProvider ──────────────────────────────────────────
        public ImageList ImageList => null;
        public string PreSelection => null;
        public int DefaultIndex => 0;
        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            if (key == '\n' || key == '\t' || key == '>' || key == ' ')
                return CompletionDataProviderKeyResult.InsertionKey;
            return CompletionDataProviderKeyResult.NormalKey;
        }
        public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        {
            // Prefix'i sil, tamamlanmış metni yaz
            string oncesi = insertionOffset > 80
                ? textArea.Document.GetText(insertionOffset - 80, 80)
                : textArea.Document.GetText(0, insertionOffset);
            int prefixUzunluk = 0;
            for (int i = oncesi.Length - 1; i >= 0; i--)
            {
                char c = oncesi[i];
                if (c == '<' || c == ' ' || c == '"' || c == '\'' || c == '\n') break;
                prefixUzunluk++;
            }
            int baslangic = insertionOffset - prefixUzunluk;
            textArea.Document.Replace(baslangic, prefixUzunluk, data.Text);
            textArea.Caret.Position = textArea.Document.OffsetToPosition(baslangic + data.Text.Length);
            return false;
        }
        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            int offset = textArea.Caret.Offset;
            string oncesi = offset > 50
                ? textArea.Document.GetText(offset - 50, 50)
                : textArea.Document.GetText(0, offset);
            string prefix = "";
            for (int i = oncesi.Length - 1; i >= 0; i--)
            {
                char c = oncesi[i];
                if (c == '<' || c == ' ' || c == '"' || c == '\'' || c == '\n') break;
                prefix = c + prefix;
            }
            var tumListe = new List<XsltCompletionItem>();
            tumListe.AddRange(XslTaglar);
            tumListe.AddRange(UblTaglar);
            var filtreli = string.IsNullOrEmpty(prefix)
                ? tumListe
                : tumListe.Where(x =>
                    x.Text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList();

            return filtreli.OrderBy(x => x.Text).Cast<ICompletionData>().ToArray();
        }
        // ── XSLT tag listesi ─────────────────────────────────────────────────
        private static readonly List<XsltCompletionItem> XslTaglar = new List<XsltCompletionItem>
        {
            new XsltCompletionItem("xsl:stylesheet",             "XSLT stil sayfası kök elementi"),
            new XsltCompletionItem("xsl:transform",              "Dönüşüm kök elementi"),
            new XsltCompletionItem("xsl:template",               "Şablon tanımı"),
            new XsltCompletionItem("xsl:apply-templates",        "Alt elementlere şablon uygula"),
            new XsltCompletionItem("xsl:call-template",          "İsimli şablonu çağır"),
            new XsltCompletionItem("xsl:param",                  "Parametre tanımla"),
            new XsltCompletionItem("xsl:with-param",             "Şablona parametre geç"),
            new XsltCompletionItem("xsl:variable",               "Değişken tanımla"),
            new XsltCompletionItem("xsl:value-of",               "Node değerini metin olarak çıkar"),
            new XsltCompletionItem("xsl:copy-of",                "Node'u kopyala"),
            new XsltCompletionItem("xsl:copy",                   "Mevcut node'u kopyala"),
            new XsltCompletionItem("xsl:element",                "Dinamik element oluştur"),
            new XsltCompletionItem("xsl:attribute",              "Dinamik attribute oluştur"),
            new XsltCompletionItem("xsl:text",                   "Sabit metin çıktısı"),
            new XsltCompletionItem("xsl:comment",                "XML yorumu oluştur"),
            new XsltCompletionItem("xsl:output",                 "Çıktı formatını belirle"),
            new XsltCompletionItem("xsl:if",                     "Koşullu blok"),
            new XsltCompletionItem("xsl:choose",                 "Çoklu koşul bloğu"),
            new XsltCompletionItem("xsl:when",                   "choose içinde koşul dalı"),
            new XsltCompletionItem("xsl:otherwise",              "choose içinde varsayılan dal"),
            new XsltCompletionItem("xsl:for-each",               "Döngü"),
            new XsltCompletionItem("xsl:sort",                   "Sırala"),
            new XsltCompletionItem("xsl:import",                 "XSLT import et"),
            new XsltCompletionItem("xsl:include",                "XSLT include et"),
            new XsltCompletionItem("xsl:key",                    "Arama anahtarı"),
            new XsltCompletionItem("xsl:number",                 "Numaralandırma"),
            new XsltCompletionItem("xsl:message",                "Hata/uyarı mesajı"),
            new XsltCompletionItem("xsl:strip-space",            "Boşlukları kaldır"),
            new XsltCompletionItem("xsl:preserve-space",         "Boşlukları koru"),
        };

        // ── UBL tag listesi ──────────────────────────────────────────────────
        private static readonly List<XsltCompletionItem> UblTaglar = new List<XsltCompletionItem>
        {
            // Fatura başlık
            new XsltCompletionItem("cbc:ID",                     "Fatura numarası"),
            new XsltCompletionItem("cbc:UUID",                   "Tekil tanımlayıcı"),
            new XsltCompletionItem("cbc:IssueDate",              "Düzenleme tarihi"),
            new XsltCompletionItem("cbc:IssueTime",              "Düzenleme saati"),
            new XsltCompletionItem("cbc:InvoiceTypeCode",        "Fatura tipi"),
            new XsltCompletionItem("cbc:Note",                   "Açıklama notu"),
            new XsltCompletionItem("cbc:DocumentCurrencyCode",   "Para birimi"),
            new XsltCompletionItem("cbc:LineCountNumeric",       "Kalem sayısı"),
            new XsltCompletionItem("cbc:ProfileID",              "Profil ID"),
            new XsltCompletionItem("cbc:CustomizationID",        "Özelleştirme ID"),
            // Taraf
            new XsltCompletionItem("cac:AccountingSupplierParty","Satıcı taraf"),
            new XsltCompletionItem("cac:AccountingCustomerParty","Alıcı taraf"),
            new XsltCompletionItem("cac:Party",                  "Taraf"),
            new XsltCompletionItem("cac:PartyName",              "Taraf adı"),
            new XsltCompletionItem("cac:PartyIdentification",    "Taraf kimlik"),
            new XsltCompletionItem("cac:PartyTaxScheme",         "Vergi dairesi"),
            new XsltCompletionItem("cbc:Name",                   "İsim"),
            new XsltCompletionItem("cbc:WebsiteURI",             "Web sitesi"),
            // Adres
            new XsltCompletionItem("cac:PostalAddress",          "Posta adresi"),
            new XsltCompletionItem("cbc:StreetName",             "Sokak"),
            new XsltCompletionItem("cbc:BuildingNumber",         "Bina no"),
            new XsltCompletionItem("cbc:CitySubdivisionName",    "İlçe"),
            new XsltCompletionItem("cbc:CityName",               "Şehir"),
            new XsltCompletionItem("cbc:PostalZone",             "Posta kodu"),
            new XsltCompletionItem("cac:Country",                "Ülke"),
            new XsltCompletionItem("cbc:IdentificationCode",     "Kimlik kodu"),
            // İletişim
            new XsltCompletionItem("cac:Contact",                "İletişim"),
            new XsltCompletionItem("cbc:Telephone",              "Telefon"),
            new XsltCompletionItem("cbc:ElectronicMail",         "E-posta"),
            // Vergi
            new XsltCompletionItem("cac:TaxTotal",               "Toplam vergi"),
            new XsltCompletionItem("cbc:TaxAmount",              "Vergi tutarı"),
            new XsltCompletionItem("cac:TaxSubtotal",            "Vergi alt toplam"),
            new XsltCompletionItem("cbc:TaxableAmount",          "Vergiye tabi tutar"),
            new XsltCompletionItem("cbc:Percent",                "Yüzde"),
            new XsltCompletionItem("cac:TaxCategory",            "Vergi kategorisi"),
            new XsltCompletionItem("cac:TaxScheme",              "Vergi şeması"),
            new XsltCompletionItem("cbc:TaxTypeCode",            "Vergi tipi kodu"),
            new XsltCompletionItem("cbc:TaxExemptionReasonCode", "İstisna kodu"),
            new XsltCompletionItem("cbc:TaxExemptionReason",     "İstisna nedeni"),
            // Toplamlar
            new XsltCompletionItem("cac:LegalMonetaryTotal",     "Para toplamı"),
            new XsltCompletionItem("cbc:LineExtensionAmount",    "Satır tutarı"),
            new XsltCompletionItem("cbc:TaxExclusiveAmount",     "KDV hariç"),
            new XsltCompletionItem("cbc:TaxInclusiveAmount",     "KDV dahil"),
            new XsltCompletionItem("cbc:PayableAmount",          "Ödenecek tutar"),
            new XsltCompletionItem("cbc:AllowanceTotalAmount",   "Toplam indirim"),
            new XsltCompletionItem("cbc:ChargeTotalAmount",      "Toplam masraf"),
            // Kalem
            new XsltCompletionItem("cac:InvoiceLine",            "Fatura kalemi"),
            new XsltCompletionItem("cbc:InvoicedQuantity",       "Miktar"),
            new XsltCompletionItem("cac:Item",                   "Ürün/hizmet"),
            new XsltCompletionItem("cbc:Description",            "Açıklama"),
            new XsltCompletionItem("cac:SellersItemIdentification","Ürün kodu"),
            new XsltCompletionItem("cac:Price",                  "Fiyat"),
            new XsltCompletionItem("cbc:PriceAmount",            "Birim fiyat"),
            new XsltCompletionItem("cac:ItemInstance",           "Ürün örneği"),
            new XsltCompletionItem("cbc:SerialID",               "Seri/lot no"),
            // İrsaliye ref
            new XsltCompletionItem("cac:DespatchDocumentReference","İrsaliye referansı"),
            new XsltCompletionItem("cac:DespatchLineReference",  "İrsaliye kalem ref"),
            new XsltCompletionItem("cac:DocumentReference",      "Belge referansı"),

            // ── e-İrsaliye spesifik alanlar ─────────────────────────────────
            new XsltCompletionItem("n1:DespatchAdvice",          "İrsaliye kök elementi (n1 ns)"),
            new XsltCompletionItem("cbc:DespatchAdviceTypeCode", "İrsaliye tip kodu (SEVK vb.)"),
            new XsltCompletionItem("cbc:ActualDespatchDate",     "Fiili sevk tarihi"),
            new XsltCompletionItem("cbc:ActualDespatchTime",     "Fiili sevk saati"),
            new XsltCompletionItem("cbc:CopyIndicator",          "Asıl/Suret göstergesi"),
            new XsltCompletionItem("cbc:LineCountNumeric",       "Kalem sayısı"),

            // Taraflar (İrsaliye)
            new XsltCompletionItem("cac:DespatchSupplierParty",  "Malların sevkiyatını sağlayan taraf"),
            new XsltCompletionItem("cac:DeliveryCustomerParty",  "Malları teslim alan taraf"),
            new XsltCompletionItem("cac:BuyerCustomerParty",     "Malları satın alan taraf"),
            new XsltCompletionItem("cac:SellerSupplierParty",    "Malları satan taraf"),
            new XsltCompletionItem("cac:OriginatorCustomerParty","Malların alınmasını başlatan taraf"),
            new XsltCompletionItem("cac:DespatchContact",        "Teslim eden kişi bilgisi"),

            // Gönderi (Shipment)
            new XsltCompletionItem("cac:Shipment",               "Gönderi/kargo bilgileri"),
            new XsltCompletionItem("cac:ShipmentStage",          "Sevkiyat aşaması"),
            new XsltCompletionItem("cac:TransportMeans",         "Taşıma aracı"),
            new XsltCompletionItem("cac:RoadTransport",          "Karayolu taşımacılığı"),
            new XsltCompletionItem("cbc:LicensePlateID",         "Araç plaka numarası"),
            new XsltCompletionItem("cac:DriverPerson",           "Sürücü bilgisi"),
            new XsltCompletionItem("cbc:FirstName",              "Sürücü adı"),
            new XsltCompletionItem("cbc:FamilyName",             "Sürücü soyadı"),
            new XsltCompletionItem("cbc:NationalityID",          "TC kimlik no (TCKN)"),
            new XsltCompletionItem("cbc:Title",                  "Ünvan (Şoför vb.)"),
            new XsltCompletionItem("cac:CarrierParty",           "Taşıyıcı firma bilgisi"),
            new XsltCompletionItem("cac:Delivery",               "Teslimat bilgisi"),
            new XsltCompletionItem("cac:DeliveryAddress",        "Teslimat adresi"),
            new XsltCompletionItem("cac:Despatch",               "Sevk bilgisi"),
            new XsltCompletionItem("cac:TransportHandlingUnit",  "Taşıma birimi (dorse vb.)"),
            new XsltCompletionItem("cac:TransportEquipment",     "Taşıma ekipmanı (dorse plaka)"),
            new XsltCompletionItem("cac:GoodsItem",              "Mal kalemi"),
            new XsltCompletionItem("cbc:ValueAmount",            "Mal değeri"),

            // İrsaliye kalemleri (DespatchLine)
            new XsltCompletionItem("cac:DespatchLine",           "İrsaliye kalemi"),
            new XsltCompletionItem("cbc:DeliveredQuantity",      "Teslim edilen miktar"),
            new XsltCompletionItem("cac:OrderLineReference",     "Sipariş kalem referansı"),
            new XsltCompletionItem("cbc:LineID",                 "Satır ID"),
            new XsltCompletionItem("cac:InvoiceLine",            "Fatura satırı (irsaliye içinde)"),
            new XsltCompletionItem("cac:DespatchAdviceLine",     "İrsaliye satırı (GoodsItem içinde)"),

            // Sipariş referansı
            new XsltCompletionItem("cac:OrderReference",         "Sipariş referansı"),

            // XPath
            new XsltCompletionItem("normalize-space(.)",         "Boşlukları temizle"),
            new XsltCompletionItem("format-number(., '#,##0.00')","Sayı formatla"),
            new XsltCompletionItem("substring(., 1, 10)",        "Alt dize"),
            new XsltCompletionItem("string-length(.)",           "Metin uzunluğu"),
            new XsltCompletionItem("translate(., ',', '.')",     "Karakter dönüştür"),
            new XsltCompletionItem("count(cac:DespatchLine)",    "İrsaliye kalem say"),
            new XsltCompletionItem("count(cac:InvoiceLine)",     "Fatura kalem say"),
            new XsltCompletionItem("sum(cac:InvoiceLine/cbc:LineExtensionAmount)", "Topla"),
        };
    }
    // ── CompletionItem (ICSharpCode 3.2.x uyumlu) ────────────────────────────

    public class XsltCompletionItem : ICompletionData
    {
        private string _text;
        private string _aciklama;
        public XsltCompletionItem(string text, string aciklama = "")
        {
            _text = text;
            _aciklama = aciklama;
        }
        // ICompletionData — get + set zorunlu
        public string Text
        {
            get => _text;
            set => _text = value;
        }
        public string Description => _aciklama;
        public double Priority => 0;
        public int ImageIndex => -1;
        // ICSharpCode 3.2.x — bool dönüş tipi
        public bool InsertAction(TextArea textArea, char ch)
        {
            int offset = textArea.Caret.Offset;
            string oncesi = offset > 80
                ? textArea.Document.GetText(offset - 80, 80)
                : textArea.Document.GetText(0, offset);
            int prefixUzunluk = 0;
            for (int i = oncesi.Length - 1; i >= 0; i--)
            {
                char c = oncesi[i];
                if (c == '<' || c == ' ' || c == '"' || c == '\'' || c == '\n') break;
                prefixUzunluk++;
            }
            int baslangic = offset - prefixUzunluk;
            textArea.Document.Replace(baslangic, prefixUzunluk, _text);
            textArea.Caret.Position =
                textArea.Document.OffsetToPosition(baslangic + _text.Length);
            return false;
        }
    }
}