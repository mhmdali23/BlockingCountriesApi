using BlockingCountriesApi.Helper;
using BlockingCountriesApi.Interfaces;
using BlockingCountriesApi.Models.Response;
using BlockingCountriesApi.Validators;

namespace BlockingCountriesApi.Services
{
    public class BlockedCountryService
    {
        private readonly IBlockedCountryRepository _blockedCountryRepository;

        public BlockedCountryService(IBlockedCountryRepository blockedCountryRepository) 
        {
            _blockedCountryRepository = blockedCountryRepository;
        }

        public Result BlockCountry(string countryCode)
        {
            if (!CountryValidator.IsValid(countryCode))
            {
                return Result.Fail("Invalid country code");
            }

            if (_blockedCountryRepository.Exists(countryCode))
            {
                return Result.Fail("Country is already blocked");
            }

            return _blockedCountryRepository.Add(countryCode)
             ? Result.Ok()
             : Result.Fail("Failed to block country");

        }

        public Result UnblockCountry(string countryCode)
        {
            if (!CountryValidator.IsValid(countryCode))
            {
                return Result.Fail("Invalid country code");
            }

            if (!_blockedCountryRepository.Exists(countryCode))
            {
                return Result.Fail("Country is not blocked");
            }

            return _blockedCountryRepository.Remove(countryCode)
             ? Result.Ok()
             : Result.Fail("Failed to unblock country");
        }
        public bool IsBlocked(string countryCode)
        {
            return _blockedCountryRepository.Exists(countryCode);
        }
        public PagedResult<string> GetBlockedCountries(string searchTerm, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var allCountries = _blockedCountryRepository.GetAll()
                .Where(c => string.IsNullOrEmpty(searchTerm) ||
                            c.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var totalCount = allCountries.Count;

            var pagedCountries = allCountries
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<string>
            {
                Data = pagedCountries,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }


    }
}
