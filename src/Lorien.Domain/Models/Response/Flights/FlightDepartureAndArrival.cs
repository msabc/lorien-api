using System.Text.Json.Serialization;

namespace Lorien.Domain.Models.Response.Flights
{
    public record FlightDepartureAndArrival
    {
        [JsonPropertyName("iataCode")]
        public required string IATACode { get; set; }

        public string? Terminal { get; set; }

        public required DateTime At { get; set; }
    }
}
