using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace XSLTEditor.Classes
{
    public static class HardwareInfo
    {
        /// <summary>
        /// Makineye özgü tekil donanım ID'si üretir.
        /// CPU SerialNumber + Motherboard SerialNumber + BIOS SerialNumber → SHA256
        /// </summary>
        public static string GetHardwareId()
        {
            try
            {
                string cpu = GetWmiValue("Win32_Processor", "ProcessorId");
                string motherboard = GetWmiValue("Win32_BaseBoard", "SerialNumber");
                string bios = GetWmiValue("Win32_BIOS", "SerialNumber");
                string raw = string.Format("{0}|{1}|{2}", cpu, motherboard, bios)
                    .Replace("To Be Filled By O.E.M.", "")
                    .Replace("Default string", "")
                    .Trim();
                return ToSha256(raw);
            }
            catch
            {
                string fallback = string.Format("{0}|{1}",
                    Environment.MachineName, Environment.UserName);
                return ToSha256(fallback);
            }
        }
        /// <summary>
        /// Ekranda gösterilecek kısa versiyon (ilk 16 karakter)
        /// </summary>
        public static string GetHardwareIdShort()
        {
            return GetHardwareId().Substring(0, 16).ToUpper();
        }
        // ── Yardımcılar ───────────────────────────────────────────────────────
        private static string GetWmiValue(string wmiClass, string property)
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    string.Format("SELECT {0} FROM {1}", property, wmiClass)))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        if (obj[property] == null) continue;
                        string val = obj[property].ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(val))
                            return val;
                    }
                }
            }
            catch { }
            return string.Empty;
        }
        private static string ToSha256(string input)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString(); 
            }
        }
    }
}