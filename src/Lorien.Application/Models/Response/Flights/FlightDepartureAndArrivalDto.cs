using System.Text.Json.Serialization;

namespace Lorien.Application.Models.Response.Flights
{
    public record FlightDepartureAndArrivalDto
    {
        [JsonPropertyName("iataCode")]
        public required string IATACode { get; set; }

        public string? Terminal { get; set; }

        public required DateTime At { get; set; }
    }
}
