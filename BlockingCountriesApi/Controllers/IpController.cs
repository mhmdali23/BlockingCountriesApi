using BlockingCountriesApi.Helpers;
using BlockingCountriesApi.Models;
using BlockingCountriesApi.Models.Response;
using BlockingCountriesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlockingCountriesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly GeoLocationService _geoLocationService;
        private readonly BlockedCountryService _blockedCountryService;
        private readonly LogService _logService;

        public IpController(GeoLocationService geoLocationService,
            BlockedCountryService blockedCountryService,
            LogService logService)
        {
            _geoLocationService = geoLocationService;
            _blockedCountryService = blockedCountryService;
            _logService = logService;
        }


        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                return BadRequest("IP cannot be empty.");
            }


            var ip = ipAddress ?? HttpContext.Connection.RemoteIpAddress?.ToString();
            if (!IpAddressHelper.IsIpAddressValid(ip))
            {
                return BadRequest("Invalid IP address");
            }
            var geoData = await _geoLocationService.GetGeoLocationAsync(ip);
            return Ok(geoData);

        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {


            string ip = GetClientIpAddress();

            var geoData = await _geoLocationService.GetGeoLocationAsync(ip);
            bool isBlocked = _blockedCountryService.IsBlocked(geoData.CountryCode);

            _logService.LogAttempt(new BlockedAttemptLog
            {
                IpAddress = geoData.IpAddress,
                CountryCode = geoData.CountryCode,
                Timestamp = DateTime.UtcNow,
                UserAgent = Request.Headers["User-Agent"],
                CountryName = geoData.CountryName,
                IsBlocked = isBlocked
            });

            return Ok(new BlockCheckResponse
            {
                IsBlocked = isBlocked,
                CountryCode = geoData.CountryCode,
            });

        }

        private string GetClientIpAddress()
        {
            string ip = null;

            var forwardedHeaders = new[] { "X-Forwarded-For", "X-Real-IP", "CF-Connecting-IP", "True-Client-IP" };

            foreach (var header in forwardedHeaders)
            {
                if (HttpContext.Request.Headers.TryGetValue(header, out var value) && !string.IsNullOrEmpty(value))
                {
                    ip = value.ToString().Split(',')[0].Trim();
                    break;
                }
            }

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            }

            if (string.IsNullOrEmpty(ip) || ip == "::1" || ip == "127.0.0.1")
            {       
                ip = "8.8.8.8"; 
            }

            return ip;
        }
    }
}
