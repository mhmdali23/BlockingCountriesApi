namespace BlockingCountriesApi.Interfaces
{
    public interface IBlockedCountryRepository
    {
        bool Add(string countryCode);
        bool Remove(string countryCode);
        IEnumerable<string> GetAll();
        bool Exists(string countryCode);
    }
}
