using BlockingCountriesApi.Models;

namespace BlockingCountriesApi.Interfaces
{
    public interface ITemporalBlockRepository
    {
        bool Add(string countryCode, DateTime expiryTime);
        bool Remove(string countryCode);
        IEnumerable<TemporalBlock> GetAll();
    }
}
