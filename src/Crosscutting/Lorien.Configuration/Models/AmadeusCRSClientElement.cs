namespace Lorien.Configuration.Models
{
    public class AmadeusCRSClientElement
    {
        public required string BaseUrl { get; set; }

        public required string APIKey { get; set; }

        public required string APISecret { get; set; }

        public required string RequestAccessTokenPath { get; set; }

        public required string FlightOffersPath { get; set; }

        public required int RequestTimeoutInSeconds { get; set; }
    }
}
