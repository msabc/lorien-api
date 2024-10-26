namespace Lorien.Domain.Models.Request
{
    public class GetFlightOfferRequest
    {
        public required string OriginLocationCode { get; set; }

        public required string DestinationLocationCode { get; set; }

        public required DateTime DepartureDate { get; set; }

        public required byte Adults { get; set; }
    }
}
