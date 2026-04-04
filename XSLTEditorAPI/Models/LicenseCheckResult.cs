namespace XSLTEditorAPI.Models
{
    public class LicenseCheckResult
    {
        public bool Gecerli { get; set; }
        public string Mesaj { get; set; } = string.Empty;
        public string? FirmaAdi { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public DateTime? SonKullanmaTarihi { get; set; }
    }
}