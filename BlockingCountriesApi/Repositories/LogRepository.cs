using BlockingCountriesApi.Interfaces;
using BlockingCountriesApi.Models;
using System.Collections.Concurrent;

namespace BlockingCountriesApi.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly ConcurrentQueue<BlockedAttemptLog> _logs = new();
        public IEnumerable<BlockedAttemptLog> GetAll() => _logs;

        public void Log(BlockedAttemptLog entry) => _logs.Enqueue(entry);
    }
}
