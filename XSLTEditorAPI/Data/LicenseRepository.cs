using Dapper;
using Microsoft.Data.SqlClient;
using XSLTEditorAPI.Models;

namespace XSLTEditorAPI.Data
{
    public class LicenseRepository
    {
        private readonly string _connStr;
        public LicenseRepository(string connStr)
        {
            _connStr = connStr;
        }
        public async Task<LicenseCheckResult> CheckAsync(string hardwareId, string? ipAddress)
        {
            using SqlConnection conn = new SqlConnection(_connStr);
            var result = await conn.QueryFirstOrDefaultAsync<LicenseCheckResult>(
                "lic.usp_CheckLicense",
                new { HardwareId = hardwareId, IpAddress = ipAddress },
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: 10
            );
            return result ?? new LicenseCheckResult
            {
                Gecerli = false,
                Mesaj = "Sunucu hatası oluştu.",
                FirmaAdi = null,
                KayitTarihi = null,
                SonKullanmaTarihi = null
            };
        }
    }
}