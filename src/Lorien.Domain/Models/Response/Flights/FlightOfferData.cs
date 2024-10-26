namespace Lorien.Domain.Models.Response.Flights
{
    public record FlightOfferData
    {
        public required string Type { get; set; }

        public required string Id { get; set; }

        public required string Source { get; set; }

        public required bool OneWay { get; set; }

        public required int NumberOfBookableSeats { get; set; }

        public required IEnumerable<FlightItinerary> Itineraries { get; set; }

        public required FlightPrice Price { get; set; }
    }
}
