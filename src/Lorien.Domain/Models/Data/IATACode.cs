using System.Text.Json.Serialization;

namespace Lorien.Domain.Models.Data
{
    public record IATACode
    {
        [JsonPropertyName("iata")]
        public required string IATA { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }
    }
}
