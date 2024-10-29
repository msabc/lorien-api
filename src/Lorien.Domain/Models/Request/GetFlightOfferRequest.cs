namespace Lorien.Domain.Models.Request
{
    public record GetFlightOfferRequest
    {
        public required string OriginLocationCode { get; set; }

        public required string DestinationLocationCode { get; set; }

        public required DateTime DepartureDate { get; set; }

        public required byte Adults { get; set; }

        public override string ToString()
        {
            return $"{OriginLocationCode}-{DestinationLocationCode}-{DepartureDate:yyyy-MM-dd}-{Adults}";
        }
    }
}
