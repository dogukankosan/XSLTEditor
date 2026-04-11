using System;
using System.IO;
using System.Net;
using System.Text;

namespace XSLTEditor.Classes
{
    public static class TokenManager
    {
        public static TokenInfo GetOrFetchToken()
        {
            // 1. Cache'de geçerli token var mı?
            TokenInfo cached = CredentialManager.GetValidToken();
            if (cached != null)
            {
                LogManager.Bilgi($"Token cache'den alındı. Kalan: {cached.RemainingMinutes} dk");
                return cached;
            }
            // 2. Ayarlardan bilgileri al
            string username = CredentialManager.Load("username");
            string password = CredentialManager.Load("password");
            string clientId = CredentialManager.Load("client_id");
            string clientSecret = CredentialManager.Load("client_secret");
            string customerId = CredentialManager.Load("customer_id");
            string firm = CredentialManager.Load("firm") ?? "1";
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                LogManager.Hata("Giriş bilgileri eksik.");
                return null;
            }
            // 3. Token al
            LogManager.Bilgi("Yeni token alınıyor...");
            string token = FetchToken(username, password, clientId, clientSecret,
                                           out DateTime expireDate);
            if (string.IsNullOrEmpty(token)) return null;
            // 4. Sunucu al
            string server = FetchServer(token, customerId) ?? "ERP-10";
            // 5. Kaydet ve döndür
            CredentialManager.SaveToken(token, expireDate, server, firm);
            return new TokenInfo
            {
                Token = token,
                Server = server,
                Firm = firm,
                ExpireDate = expireDate,
                RemainingMinutes = (int)(expireDate.ToUniversalTime() - DateTime.UtcNow).TotalMinutes
            };
        }
        private static string FetchToken(string username, string password,
                                          string clientId, string clientSecret,
                                          out DateTime expireDate)
        {
            expireDate = DateTime.UtcNow.AddHours(1);
            try
            {
                string body = string.Format(
                    "grant_type=password&username={0}&password={1}" +
                    "&client_id={2}&client_secret={3}&lang=TRTR",
                    Uri.EscapeDataString(username),
                    Uri.EscapeDataString(password),
                    Uri.EscapeDataString(clientId),
                    Uri.EscapeDataString(clientSecret));
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(
                    "https://idm.logo.cloud/legacy/sts/api/oauth/token");
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] data = Encoding.UTF8.GetBytes(body);
                req.ContentLength = data.Length;
                using (Stream s = req.GetRequestStream()) s.Write(data, 0, data.Length);
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                {
                    string json = sr.ReadToEnd();
                    string token = ParseJson(json, "access_token");
                    string exp = ParseJson(json, "expire_date");
                    if (DateTime.TryParse(exp, out DateTime dt)) expireDate = dt;
                    return token;
                }
            }
            catch (Exception ex)
            {
                LogManager.Hata("Token alma hatası", ex);
                return null;
            }
        }
        private static string FetchServer(string token, string customerId)
        {
            try
            {
                if (string.IsNullOrEmpty(customerId)) return null;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(
                    $"https://cloudcontrol.logo.cloud/api/tenants/gettenantappintanceids/{customerId}");
                req.Method = "GET";
                req.Accept = "application/json";
                req.Headers.Add("Authorization", token);
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    return ParseJson(sr.ReadToEnd(), "ApplicationServerTag");
            }
            catch (Exception ex)
            {
                LogManager.Hata("Sunucu alma hatası", ex);
                return null;
            }
        }
        private static string ParseJson(string json, string key)
        {
            string search = $"\"{key}\":";
            int i = json.IndexOf(search);
            if (i < 0) return null;
            i += search.Length;
            while (i < json.Length && (json[i] == ' ' || json[i] == '"')) i++;
            int end = json.IndexOfAny(new[] { '"', ',', '}', ']' }, i);
            return end < 0 ? null : json.Substring(i, end - i).Trim('"', ' ');
        }
    }
}