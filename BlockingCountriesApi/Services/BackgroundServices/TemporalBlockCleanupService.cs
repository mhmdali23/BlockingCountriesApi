using BlockingCountriesApi.Interfaces;
using BlockingCountriesApi.Models;

namespace BlockingCountriesApi.Services.BackgroundServices
{
    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly ITemporalBlockRepository _temporalBlockRepository;

        public TemporalBlockCleanupService(ITemporalBlockRepository temporalBlockRepository)
        {
            _temporalBlockRepository = temporalBlockRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var expired = _temporalBlockRepository.GetAll()
                    .Where(x => x.ExpiryTime < DateTime.UtcNow)
                    .ToList();

                foreach (var block in expired)
                {
                    _temporalBlockRepository.Remove(block.CountryCode);
                }
                await Task.Delay(TimeSpan.FromMinutes(5),stoppingToken);
            }
        }
    }
}
