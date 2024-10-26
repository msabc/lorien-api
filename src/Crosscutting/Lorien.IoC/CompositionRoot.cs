using Lorien.Application.Services;
using Lorien.Configuration;
using Lorien.Domain.Interfaces.Services.Flights;
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
                    .RegisterHttpClients(configuration)
                    .RegisterApplicationServices();

            return services;
        }

        private static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LorienSettings>(options => configuration.GetSection(nameof(LorienSettings)).Bind(options));

            return services;
        }

        private static IServiceCollection RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
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
            });

            return services;
        }

        private static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFlightPricingService, FlightPricingService>();

            return services;
        }
    }
}
