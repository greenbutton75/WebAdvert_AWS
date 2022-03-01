using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebAdvert.SearchApi.Services;

namespace AdvertAPI.HealthChecks
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly ISearchService _storageService;

        public StorageHealthCheck(ISearchService storageService)
        {
            _storageService = storageService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var IsOK = _storageService.CheckHealth();
            return new HealthCheckResult(IsOK ? HealthStatus.Healthy : HealthStatus.Unhealthy);
        }
    }
}
