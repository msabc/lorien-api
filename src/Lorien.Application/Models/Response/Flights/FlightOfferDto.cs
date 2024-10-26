namespace Lorien.Application.Models.Response.Flights
{
    public record FlightOfferDto
    {
        public required string Type { get; set; }

        public required string Id { get; set; }

        public required string Source { get; set; }

        public required bool OneWay { get; set; }

        public required int NumberOfBookableSeats { get; set; }

        public required IEnumerable<FlightItineraryDto> Itineraries { get; set; }

        public required string GrandTotal { get; set; }

        public required string Currency { get; set; }
    }
}
