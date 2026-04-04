using Microsoft.Data.SqlClient;
using XSLTEditorAPI.Data;

namespace XSLTEditorAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            // ── Windows Service desteği ───────────────────────────────────────
            builder.Host.UseWindowsService();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new()
                {
                    Title = "XSLT Editor — Lisans API",
                    Version = "v1",
                    Description = "Mutlu Yazılım | Donanım bazlı lisans doğrulama servisi"
                });
            });
            string connStr = builder.Configuration.GetConnectionString("LicenseDb")
                ?? throw new InvalidOperationException("LicenseDb connection string bulunamadı.");
            // ── SQL testi servis başladıktan sonra yap ────────────────────────
            builder.Services.AddScoped<LicenseRepository>(_ => new LicenseRepository(connStr));
            builder.WebHost.UseUrls("http://0.0.0.0:9922");
            WebApplication app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "License API v1");
                    c.RoutePrefix = string.Empty;
                    c.DocumentTitle = "Mutlu Yazılım — Lisans API";
                });
            }
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}