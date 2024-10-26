namespace Lorien.Domain.Models.Response.Flights
{
    public record GetFlightOfferResponse
    {
        public required IEnumerable<FlightOfferData> Data { get; set; }
    }
}
