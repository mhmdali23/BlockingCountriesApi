using BlockingCountriesApi.Helper;
using BlockingCountriesApi.Helpers;
using BlockingCountriesApi.Interfaces;
using BlockingCountriesApi.Models;
using BlockingCountriesApi.Models.Response;
using BlockingCountriesApi.Repositories;
using System.Net;

namespace BlockingCountriesApi.Services
{
    public class LogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void LogAttempt(BlockedAttemptLog blockedAttemptLog)
        {
            _logRepository.Log(new BlockedAttemptLog
            {
                CountryCode = blockedAttemptLog.CountryCode,
                CountryName = blockedAttemptLog.CountryName,
                IpAddress = blockedAttemptLog.IpAddress,
                IsBlocked = blockedAttemptLog.IsBlocked,
                Timestamp = DateTime.UtcNow,
                UserAgent = blockedAttemptLog.UserAgent
            });

        }

        public PagedResult<BlockedAttemptLog> GetBlockedAttemptLogs(int page,int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;


            var logs = _logRepository.GetAll().ToList();

            var totalCount = logs.Count;

            var pagedCountries = logs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return new PagedResult<BlockedAttemptLog>
            {
                Data = logs,
                Page = page,
                PageSize = pageSize,
                TotalCount = logs.Count
            };
        }
    }
}
