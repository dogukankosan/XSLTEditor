using System;
using System.Data.SQLite;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace XSLTEditor.Classes
{
    public static class CredentialManager
    {
        private static readonly string DbPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database\\credentials.db");
        private static string ConnStr => $"Data Source={DbPath};Version=3;";
        private static readonly byte[] Key =
            Encoding.UTF8.GetBytes("XSLTEditor_2026_AES256Key!@#$%^&"); // 32 byte
        static CredentialManager() => InitDb();
        private static void InitDb()
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnStr))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS settings (
                            key        TEXT PRIMARY KEY,
                            value      TEXT NOT NULL,
                            updated_at TEXT NOT NULL
                        );
                        CREATE TABLE IF NOT EXISTS tokens (
                            id          INTEGER PRIMARY KEY,
                            token       TEXT NOT NULL,
                            expire_date TEXT NOT NULL,
                            server      TEXT,
                            firm        TEXT,
                            created_at  TEXT NOT NULL
                        );";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // ── AES-GCM ─────────────────────────────────────────────────────
        public static string Encrypt(string plain)
        {
            if (string.IsNullOrEmpty(plain)) return string.Empty;
            byte[] nonce = new byte[12];
            new SecureRandom().NextBytes(nonce);
            byte[] input = Encoding.UTF8.GetBytes(plain);
            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            cipher.Init(true, new AeadParameters(new KeyParameter(Key), 128, nonce));
            byte[] output = new byte[cipher.GetOutputSize(input.Length)];
            int len = cipher.ProcessBytes(input, 0, input.Length, output, 0);
            cipher.DoFinal(output, len);
            byte[] result = new byte[12 + output.Length];
            Buffer.BlockCopy(nonce, 0, result, 0, 12);
            Buffer.BlockCopy(output, 0, result, 12, output.Length);
            return Convert.ToBase64String(result);
        }
        public static string Decrypt(string cipher)
        {
            if (string.IsNullOrEmpty(cipher)) return string.Empty;
            byte[] all = Convert.FromBase64String(cipher);
            byte[] nonce = new byte[12];
            byte[] enc = new byte[all.Length - 12];
            Buffer.BlockCopy(all, 0, nonce, 0, 12);
            Buffer.BlockCopy(all, 12, enc, 0, enc.Length);
            GcmBlockCipher gcm = new GcmBlockCipher(new AesEngine());
            gcm.Init(false, new AeadParameters(new KeyParameter(Key), 128, nonce));
            byte[] output = new byte[gcm.GetOutputSize(enc.Length)];
            int len = gcm.ProcessBytes(enc, 0, enc.Length, output, 0);
            gcm.DoFinal(output, len);
            return Encoding.UTF8.GetString(output);
        }
        // ── Settings ────────────────────────────────────────────────────
        public static void Save(string key, string value)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnStr))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO settings (key, value, updated_at)
                        VALUES (@k, @v, @u)
                        ON CONFLICT(key) DO UPDATE SET value=@v, updated_at=@u";
                    cmd.Parameters.AddWithValue("@k", key);
                    cmd.Parameters.AddWithValue("@v", Encrypt(value));
                    cmd.Parameters.AddWithValue("@u", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static string Load(string key)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnStr))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT value FROM settings WHERE key=@k";
                    cmd.Parameters.AddWithValue("@k", key);
                    var result = cmd.ExecuteScalar();
                    return result == null ? null : Decrypt(result.ToString());
                }
            }
        }
       // ── Token ────────────────────────────────────────────────────────
        public static void SaveToken(string token, DateTime expireDate,
                                     string server, string firm)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnStr))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM tokens";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = @"
                        INSERT INTO tokens (token, expire_date, server, firm, created_at)
                        VALUES (@t, @e, @s, @f, @c)";
                    cmd.Parameters.AddWithValue("@t", Encrypt(token));
                    cmd.Parameters.AddWithValue("@e", expireDate.ToString("o"));
                    cmd.Parameters.AddWithValue("@s", server ?? "");
                    cmd.Parameters.AddWithValue("@f", firm ?? "");
                    cmd.Parameters.AddWithValue("@c", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }
            }
            LogManager.Bilgi("Token kaydedildi.");
        }
        public static TokenInfo GetValidToken()
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnStr))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "SELECT token, expire_date, server, firm FROM tokens LIMIT 1";
                    using (SQLiteDataReader r = cmd.ExecuteReader())
                    {
                        if (!r.Read()) return null;
                        if (!DateTime.TryParse(r.GetString(1), out DateTime exp))
                            return null;
                        // 5 dk erken expire
                        if (DateTime.UtcNow >= exp.ToUniversalTime().AddMinutes(-5))
                        {
                            LogManager.Bilgi("Token süresi dolmuş.");
                            return null;
                        }
                        return new TokenInfo
                        {
                            Token = Decrypt(r.GetString(0)),
                            ExpireDate = exp,
                            Server = r.GetString(2),
                            Firm = r.GetString(3),
                            RemainingMinutes = (int)(exp.ToUniversalTime()
                                               - DateTime.UtcNow).TotalMinutes
                        };
                    }
                }
            }
        }
        public static void ClearToken()
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnStr))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM tokens";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
    public class TokenInfo
    {
        public string Token { get; set; }
        public string Server { get; set; }
        public string Firm { get; set; }
        public DateTime ExpireDate { get; set; }
        public int RemainingMinutes { get; set; }
    }
}