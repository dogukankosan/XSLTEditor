using Microsoft.AspNetCore.Mvc;
using XSLTEditorAPI.Data;
using XSLTEditorAPI.Models;

namespace XSLTEditorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        private readonly LicenseRepository _repo;
        private readonly ILogger<LicenseController> _logger;
        public LicenseController(LicenseRepository repo, ILogger<LicenseController> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        /// <summary>
        /// Donanım ID'sine göre lisans kontrolü yapar.
        /// </summary>
        [HttpGet("check")]
        [ProducesResponseType(typeof(LicenseCheckResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Check([FromQuery] string hardwareId)
        {
            if (string.IsNullOrWhiteSpace(hardwareId))
                return BadRequest(new { Mesaj = "hardwareId parametresi zorunludur." });
            string? ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation("Lisans sorgusu | HardwareId: {hw} | IP: {ip}", hardwareId, ip);
            try
            {
                LicenseCheckResult result = await _repo.CheckAsync(hardwareId, ip);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lisans kontrol hatası | HardwareId: {hw}", hardwareId);
                return StatusCode(500, new { Mesaj = "Sunucu hatası. Lütfen tekrar deneyin." });
            }
        }
    }
}