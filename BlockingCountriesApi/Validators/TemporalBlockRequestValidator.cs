using BlockingCountriesApi.Models;
using FluentValidation;

namespace BlockingCountriesApi.Validators
{
    public class TemporalBlockRequestValidator : AbstractValidator<TemporalBlockRequest>
    {
        public TemporalBlockRequestValidator()
        {
            RuleFor(x => x.CountryCode).Must(CountryValidator.IsValid);
            RuleFor(x => x.DurationMinutes).InclusiveBetween(1, 1440);
        }
    }
}
