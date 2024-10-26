namespace Lorien.Domain.Models.Response.Flights
{
    public record FlightItinerary
    {
        public required string Duration { get; set; }

        public required IEnumerable<FlightSegment> Segments { get; set; }
    }
}
