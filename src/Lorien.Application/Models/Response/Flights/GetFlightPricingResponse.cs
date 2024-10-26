namespace Lorien.Application.Models.Response.Flights
{
    public record GetFlightPricingResponse
    {
        public required IEnumerable<FlightOfferDto> FlightOffers { get; set; }
    }
}
