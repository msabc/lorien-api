using Lorien.Configuration;
using Lorien.Domain.Interfaces.Services.Flights;
using Lorien.Domain.Models.Request;
using Lorien.Domain.Models.Response.Auth;
using Lorien.Domain.Models.Response.Flights;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace Lorien.Infrastructure.Services.Flights
{
    public class AmadeusCRSHttpService : IAmadeusCRSHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AmadeusCRSHttpService> _logger;
        private readonly LorienSettings _settings;


        private const string AUTHORIZATION_PARAMETER_TYPE_NAME = "Bearer";
        private const string AUTHORIZATION_GRANT_TYPE_PARAMETER_NAME = "grant_type";
        private const string AUTHORIZATION_CLIENT_CREDENTIALS_PARAMETER_NAME = "client_credentials";
        private const string AUTHORIZATION_CLIENT_ID_PARAMETER_NAME = "client_id";
        private const string AUTHORIZATION_CLIENT_SECRET_PARAMETER_NAME = "client_secret";
        private const string AUTHORIZATION_CONTENT_TYPE = "application/x-www-form-urlencoded";

        private static string accessToken = string.Empty;

        private const string ORIGIN_LOCATION_CODE_PARAMETER_KEY_NAME = "originLocationCode";
        private const string DESTINATION_LOCATION_CODE_PARAMETER_KEY_NAME = "destinationLocationCode";
        private const string DEPARTURE_DATE_PARAMETER_KEY_NAME = "departureDate";
        private const string ADULTS_PARAMETER_KEY_NAME = "adults";

        private const string DEPARTURE_DATE_FORMAT = "yyyy-MM-dd";

        public AmadeusCRSHttpService(HttpClient httpClient,
                                     ILogger<AmadeusCRSHttpService> logger,
                                     IOptions<LorienSettings> settings)
        {
            _httpClient = httpClient;
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<GetFlightOfferResponse> GetFlightOffers(GetFlightOfferRequest request)
        {
            _httpClient.DefaultRequestHeaders.Clear();

            // 1. empty token => represents the inital request
            if (string.IsNullOrEmpty(accessToken))
            {
                var authResponse = await RequestAccessTokenAsync();
                accessToken = authResponse.AccessToken;
            }

            // 3. valid token
            string flightPath = $"{_settings.AmadeusCRSClient.FlightOffersPath}?{ORIGIN_LOCATION_CODE_PARAMETER_KEY_NAME}={request.OriginLocationCode}&{DESTINATION_LOCATION_CODE_PARAMETER_KEY_NAME}={request.DestinationLocationCode}&{DEPARTURE_DATE_PARAMETER_KEY_NAME}={request.DepartureDate.ToString(DEPARTURE_DATE_FORMAT)}&{ADULTS_PARAMETER_KEY_NAME}={request.Adults}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AUTHORIZATION_PARAMETER_TYPE_NAME, accessToken);

            var primaryResponse = await _httpClient.GetAsync(flightPath);

            if (primaryResponse.StatusCode == HttpStatusCode.OK)
            {
                string wow = await primaryResponse.Content.ReadAsStringAsync();
                return await primaryResponse.Content.ReadFromJsonAsync<GetFlightOfferResponse>();
            }
            else if (primaryResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                var authResponse = await RequestAccessTokenAsync();
                accessToken = authResponse.AccessToken;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AUTHORIZATION_PARAMETER_TYPE_NAME, accessToken);
                var secondaryResponse = await _httpClient.GetAsync(flightPath);

                if (secondaryResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("mi scusi");
                }

                return await secondaryResponse.Content.ReadFromJsonAsync<GetFlightOfferResponse>();
            }
            else
            {
                throw new Exception("messed up");
            }
        }

        private async Task<AmadeusAuthResponse> RequestAccessTokenAsync()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();

                var formValues = new Dictionary<string, string>()
                {
                    { AUTHORIZATION_GRANT_TYPE_PARAMETER_NAME, AUTHORIZATION_CLIENT_CREDENTIALS_PARAMETER_NAME },
                    { AUTHORIZATION_CLIENT_ID_PARAMETER_NAME, _settings.AmadeusCRSClient.APIKey },
                    { AUTHORIZATION_CLIENT_SECRET_PARAMETER_NAME, _settings.AmadeusCRSClient.APISecret }
                };

                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    Headers = {
                            { HttpRequestHeader.ContentType.ToString(), AUTHORIZATION_CONTENT_TYPE },
                    },
                    Content = new FormUrlEncodedContent(formValues),
                    RequestUri = new Uri(_settings.AmadeusCRSClient.BaseUrl + _settings.AmadeusCRSClient.RequestAccessTokenPath)
                };

                var httpResponse = await _httpClient.SendAsync(request);

                httpResponse.EnsureSuccessStatusCode();

                return await httpResponse.Content.ReadFromJsonAsync<AmadeusAuthResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                // TODO: throw a custom exception here
                throw;
            }
        }
    }
}
