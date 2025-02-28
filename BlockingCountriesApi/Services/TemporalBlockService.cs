using BlockingCountriesApi.Helper;
using BlockingCountriesApi.Interfaces;
using BlockingCountriesApi.Validators;

namespace BlockingCountriesApi.Services
{
    public class TemporalBlockService
    {
        private readonly ITemporalBlockRepository _temporalBlockRepository;

        public TemporalBlockService(ITemporalBlockRepository temporalBlockRepository)
        {
            _temporalBlockRepository = temporalBlockRepository;
        }

        public Result AddTemporalBlock(string countryCode, int durationMinutes)
        {
            if(!CountryValidator.IsValid(countryCode))
            {
                return Result.Fail("Invalid country code");
            }

            var expiry = DateTime.UtcNow.AddMinutes(durationMinutes);
            return _temporalBlockRepository.Add(countryCode, expiry)
                ? Result.Ok()
                : Result.Fail("Failed to add temporal block");
        }
    }
}
