using BlockingCountriesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockingCountriesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController(LogService logService) : ControllerBase
    {
        [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedAttempts(int page, int pageSize)
        {
            var result = logService.GetBlockedAttemptLogs(page, pageSize);
            return Ok(result);
        }
    }
}
