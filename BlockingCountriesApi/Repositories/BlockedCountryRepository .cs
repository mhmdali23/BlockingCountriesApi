using BlockingCountriesApi.Interfaces;
using BlockingCountriesApi.Models;
using System.Collections.Concurrent;

namespace BlockingCountriesApi.Repositories
{
    public class BlockedCountryRepository : IBlockedCountryRepository
    {

        private readonly ConcurrentDictionary<string, BlockedCountry> _blockedCountries = new();

        public bool Add(string countryCode) =>
            _blockedCountries.TryAdd(countryCode, new BlockedCountry
            {
                CountryCode = countryCode,
                BlockedAt = DateTime.UtcNow
            });

        public bool Remove(string countryCode) =>
            _blockedCountries.TryRemove(countryCode, out _);

        public IEnumerable<string> GetAll() =>
            _blockedCountries.Keys;

        public bool Exists(string countryCode) =>
            _blockedCountries.ContainsKey(countryCode);
    }
}

