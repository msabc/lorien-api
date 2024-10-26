namespace Lorien.Application.Models.Response.Flights
{
    public record FlightItineraryDto
    {
        public required string Duration { get; set; }

        public required IEnumerable<FlightSegmentDto> Segments { get; set; }
    }
}
