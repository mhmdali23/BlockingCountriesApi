using BlockingCountriesApi.Interfaces;
using BlockingCountriesApi.Models;
using System.Collections.Concurrent;

namespace BlockingCountriesApi.Repositories
{
    public class TemporalBlockRepository : ITemporalBlockRepository
    {

        private readonly ConcurrentDictionary<string, DateTime> _temporalBlocks = new();
        public bool Add(string countryCode, DateTime expiryTime) => _temporalBlocks.TryAdd(countryCode, expiryTime);

        public IEnumerable<TemporalBlock> GetAll() => 
            _temporalBlocks.Select(kvp => new TemporalBlock { CountryCode = kvp.Key, ExpiryTime = kvp.Value });


        public bool Remove(string countryCode) => _temporalBlocks.TryRemove(countryCode, out _);
    }
}
