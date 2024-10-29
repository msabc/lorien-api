using Lorien.Configuration.Models;

namespace Lorien.Configuration
{
    public record LorienSettings
    {
        public required AmadeusCRSClientElement AmadeusCRSClient { get; set; }

        public required int FlightPricingRequestCachingTimeToLiveInMinutes { get; set; }

        public required int MaxIATACodeResponseSize { get; set; }
    }
}
