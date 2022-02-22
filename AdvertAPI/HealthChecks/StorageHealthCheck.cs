using AdvertAPI.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AdvertAPI.HealthChecks
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IAdvertStorageService _storageService;

        public StorageHealthCheck(IAdvertStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var IsOK = await _storageService.CheckHealthAsync();
            return new HealthCheckResult(IsOK ? HealthStatus.Healthy : HealthStatus.Unhealthy);
        }
    }
}
