using Lorien.Configuration.Models;

namespace Lorien.Configuration
{
    public class LorienSettings
    {
        public required AmadeusCRSClientElement AmadeusCRSClient { get; set; }

        public required int RequestCachingTimeToLiveInMinutes { get; set; }
    }
}
