using Lorien.Application.Services.Flights;
using Lorien.Application.Services.Internal;
using Lorien.Configuration;
using Lorien.Domain.Interfaces.Repositories;
using Lorien.Domain.Interfaces.Services.Caching;
using Lorien.Domain.Interfaces.Services.Flights;
using Lorien.Infrastructure.Repositories;
using Lorien.Infrastructure.Services.Caching;
using Lorien.Infrastructure.Services.Flights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Lorien.IoC
{
    public static class CompositionRoot
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterSettings(configuration)
                    .RegisterInfrastructureServices(configuration)
                    .RegisterRepositories()
                    .RegisterApplicationServices();

            return services;
        }

        private static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LorienSettings>(options => configuration.GetSection(nameof(LorienSettings)).Bind(options));

            return services;
        }

        private static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using IServiceScope scope = serviceProvider.CreateScope();

            var settings = scope.ServiceProvider.GetService<IOptions<LorienSettings>>().Value;

            if (string.IsNullOrEmpty(settings.AmadeusCRSClient.APIKey) || string.IsNullOrEmpty(settings.AmadeusCRSClient.APISecret))
            {
                throw new Exception($"Missing credentials for {nameof(LorienSettings.AmadeusCRSClient)}.");
            }

            services.AddHttpClient<IAmadeusCRSHttpService, AmadeusCRSHttpService>(client =>
            {
                client.BaseAddress = new Uri(settings.AmadeusCRSClient.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(settings.AmadeusCRSClient.RequestTimeoutInSeconds);
            });

            services.AddScoped<ICachingService, CachingService>();

            return services;
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IIATACodeRepository, IATACodeRepository>();

            return services;
        }

        private static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFlightPricingService, FlightPricingService>();
            services.AddScoped<IInternalService, InternalService>();

            return services;
        }
    }
}
