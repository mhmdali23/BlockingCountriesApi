using System.ComponentModel.DataAnnotations;

namespace BlockingCountriesApi.Models.Request
{
    public class BlockCountryRequest
    {
        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string CountryCode { get; set; }
    }
}
