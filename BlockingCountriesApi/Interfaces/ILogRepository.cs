using BlockingCountriesApi.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace BlockingCountriesApi.Interfaces
{
    public interface ILogRepository
    {
        void Log(BlockedAttemptLog entry);
        IEnumerable<BlockedAttemptLog> GetAll();
    }
}
