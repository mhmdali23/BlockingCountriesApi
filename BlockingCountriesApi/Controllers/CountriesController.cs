using BlockingCountriesApi.Models;
using BlockingCountriesApi.Models.Request;
using BlockingCountriesApi.Models.Response;
using BlockingCountriesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockingCountriesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly BlockedCountryService _blockedCountryService;
        private readonly TemporalBlockService _temporalBlockService;

        public CountriesController(BlockedCountryService blockedCountryService,TemporalBlockService temporalBlockService)
        {
            _blockedCountryService = blockedCountryService;
            _temporalBlockService = temporalBlockService;
        }

        [HttpPost("block")]
        public IActionResult BlockCountry(BlockCountryRequest blockCountryRequest)
        {
            if (string.IsNullOrWhiteSpace(blockCountryRequest.CountryCode))
            {
                return BadRequest("Country code cannot be empty.");
            }
            var result = _blockedCountryService.BlockCountry(blockCountryRequest.CountryCode);
           
            return result.IsSuccess ? Ok() : Conflict("Country is already blocked");
        }


        [HttpDelete("unblock")]
        public  IActionResult UnblockCountry(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                return BadRequest("Country code cannot be empty.");
            }
            var result = _blockedCountryService.UnblockCountry(countryCode);
            return result.IsSuccess ? Ok() : NotFound("Country is not blocked");
        }

        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries(string? searchTerm, int page, int pageSize)
        {
            var countries = _blockedCountryService.GetBlockedCountries(searchTerm, page, pageSize);
            return Ok(new PagedResult<string>
            {
                Data = countries.Data,
                Page = countries.Page,
                PageSize = countries.PageSize,
                TotalCount = countries.TotalCount
            });
        }

        [HttpPost("temporal-block")]
        public IActionResult TemporalBlockCountry(TemporalBlockRequest temporalBlockRequest)
        {
            if (string.IsNullOrWhiteSpace(temporalBlockRequest.CountryCode))
            {
                return BadRequest("Country code cannot be empty.");
            }
            var result = _temporalBlockService.AddTemporalBlock(temporalBlockRequest.CountryCode, temporalBlockRequest.DurationMinutes);
            return result.IsSuccess ? Ok() : Conflict("Country is already temporarily blocked");
        }

    }
}
