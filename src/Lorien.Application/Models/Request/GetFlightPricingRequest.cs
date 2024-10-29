namespace Lorien.Application.Models.Request
{
    public record GetFlightPricingRequest
    {
        public required string OriginLocationCode { get; set; }

        public required string DestinationLocationCode { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime DestinationDate { get; set; }

        public byte NumberOfPassengers { get; set; }

        public required string CurrencyCode { get; set; }
    }
}
