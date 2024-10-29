using Lorien.Domain.Interfaces.Services.Caching;

namespace Lorien.Api.Jobs
{
    public class CachingJob(ILogger<CachingJob> logger, IServiceProvider services) : IHostedService
    {

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Background caching job started.");

            try
            {
                ExecuteMemoryCachingTask();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error executing background caching job. {ex.Message} {ex.InnerException?.Message}");
            }

            logger.LogInformation("Background caching job completed.");

            return Task.CompletedTask;
        }

        private void ExecuteMemoryCachingTask()
        {
            using (IServiceScope scope = services.CreateScope())
            {
                var cachingService = scope.ServiceProvider.GetRequiredService<ICachingService>();

                Task.Run(cachingService.AddInitialMemoryCacheData).GetAwaiter().GetResult();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
