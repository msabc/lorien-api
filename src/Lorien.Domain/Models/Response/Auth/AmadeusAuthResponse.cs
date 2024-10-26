using System.Text.Json.Serialization;

namespace Lorien.Domain.Models.Response.Auth
{
    public record AmadeusAuthResponse
    {
        public required string Type { get; set; }

        public required string Username { get; set; }

        [JsonPropertyName("application_name")]
        public required string ApplicationName { get; set; }

        [JsonPropertyName("client_id")]
        public required string ClientId { get; set; }

        [JsonPropertyName("token_type")]
        public required string TokenType { get; set; }

        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public required short ExpiresIn { get; set; }

        public required string State { get; set; }
    }
}
