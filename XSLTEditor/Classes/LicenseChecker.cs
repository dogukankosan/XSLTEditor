using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace XSLTEditor.Classes
{
    public static class LicenseChecker
    {
        public static LicenseResult Kontrol()
        {
            string hardwareId = string.Empty;
            try
            {
                hardwareId = HardwareInfo.GetHardwareId();
                string url = string.Format("{0}/api/license/check?hardwareId={1}",
                    AppConfig.LicenseApiUrl, hardwareId);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 10000; // 10 saniye
                request.Accept = "application/json";
                request.Method = "GET";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    string json = reader.ReadToEnd();
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    LicenseApiResponse result = serializer.Deserialize<LicenseApiResponse>(json);
                    return new LicenseResult
                    {
                        Gecerli = result.Gecerli,
                        Mesaj = result.Mesaj,
                        FirmaAdi = result.FirmaAdi,
                        HardwareId = hardwareId
                    };
                }
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.ConnectFailure ||
                                          ex.Status == WebExceptionStatus.Timeout ||
                                          ex.Status == WebExceptionStatus.NameResolutionFailure)
            {
                return new LicenseResult
                {
                    Gecerli = false,
                    Mesaj = "Lisans sunucusuna bağlanılamadı. İnternet bağlantınızı kontrol edin.",
                    HardwareId = hardwareId
                };
            }
            catch (WebException ex)
            {
                return new LicenseResult
                {
                    Gecerli = false,
                    Mesaj = "Sunucu hatası: " + ex.Message,
                    HardwareId = hardwareId
                };
            }
            catch (Exception ex)
            {
                LogManager.Hata("Lisans kontrol hatası", ex);
                return new LicenseResult
                {
                    Gecerli = false,
                    Mesaj = "Lisans kontrolü sırasında hata oluştu: " + ex.Message,
                    HardwareId = hardwareId
                };
            }
        }
    }
    public class LicenseApiResponse
    {
        public bool Gecerli { get; set; }
        public string Mesaj { get; set; }
        public string FirmaAdi { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public DateTime? SonKullanmaTarihi { get; set; }
    }
    public class LicenseResult
    {
        public bool Gecerli { get; set; }
        public string Mesaj { get; set; }
        public string FirmaAdi { get; set; }
        public string HardwareId { get; set; }
    }
}