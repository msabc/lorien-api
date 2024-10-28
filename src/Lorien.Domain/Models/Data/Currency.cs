using System.Text.Json.Serialization;

namespace Lorien.Domain.Models.Data
{
    public record Currency
    {
        [JsonPropertyName("currency")]
        public required string Name { get; set; }

        [JsonPropertyName("alphabeticcode")]
        public required string Code { get; set; }
    }
}
