using System.Globalization;

namespace BlockingCountriesApi.Validators
{
    public static class CountryValidator
    {
        private static readonly HashSet<string> ValidCodes = new(CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(c => new RegionInfo(c.Name).TwoLetterISORegionName));

        public static bool IsValid(string code) =>
            ValidCodes.Contains(code.ToUpper());
    }
}
