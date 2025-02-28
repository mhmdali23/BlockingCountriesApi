using BlockingCountriesApi.Models.Request;
using FluentValidation;

namespace BlockingCountriesApi.Validators
{
    public class BlockCountryRequestValidator : AbstractValidator<BlockCountryRequest>
    {
        public BlockCountryRequestValidator()
        {
            RuleFor(x => x.CountryCode)
                .Must(CountryValidator.IsValid)
                .Length(2);
        }
    }
}
