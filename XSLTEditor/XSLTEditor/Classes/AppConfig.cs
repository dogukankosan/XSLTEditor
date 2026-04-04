using System.Configuration;

namespace XSLTEditor.Classes
{
    public static class AppConfig
    {
        public static string LicenseApiUrl
        {
            get
            {
                string val = ConfigurationManager.AppSettings["LicenseApiUrl"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException(
                        "app.config içinde 'LicenseApiUrl' tanımlı değil.");
                return val.TrimEnd('/');
            }
        }
    }
}