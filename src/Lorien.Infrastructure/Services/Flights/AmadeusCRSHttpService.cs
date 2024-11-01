﻿using Lorien.Configuration;
using Lorien.Domain.Interfaces.Services.Flights;
using Lorien.Domain.Models.Request;
using Lorien.Domain.Models.Response.Auth;
using Lorien.Domain.Models.Response.Flights;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace Lorien.Infrastructure.Services.Flights
{
    public class AmadeusCRSHttpService(HttpClient httpClient, IOptions<LorienSettings> settings) : IAmadeusCRSHttpService
    {
        private readonly LorienSettings _settings = settings.Value;

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

        public async Task<GetFlightOfferResponse> GetFlightOffers(GetFlightOfferRequest request)
        {
            httpClient.DefaultRequestHeaders.Clear();

            // 1. empty token => represents the inital request
            if (string.IsNullOrEmpty(accessToken))
            {
                var authResponse = await RequestAccessTokenAsync();
                accessToken = authResponse.AccessToken;
            }

            // 3. valid token
            string flightPath = $"{_settings.AmadeusCRSClient.FlightOffersPath}?{ORIGIN_LOCATION_CODE_PARAMETER_KEY_NAME}={request.OriginLocationCode}&{DESTINATION_LOCATION_CODE_PARAMETER_KEY_NAME}={request.DestinationLocationCode}&{DEPARTURE_DATE_PARAMETER_KEY_NAME}={request.DepartureDate.ToString(DEPARTURE_DATE_FORMAT)}&{ADULTS_PARAMETER_KEY_NAME}={request.Adults}";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AUTHORIZATION_PARAMETER_TYPE_NAME, accessToken);

            var primaryResponse = await httpClient.GetAsync(flightPath);

            if (primaryResponse.StatusCode == HttpStatusCode.OK)
            {
                return await primaryResponse.Content.ReadFromJsonAsync<GetFlightOfferResponse>();
            }
            else if (primaryResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                // 2. invalid token
                var authResponse = await RequestAccessTokenAsync();
                accessToken = authResponse.AccessToken;

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AUTHORIZATION_PARAMETER_TYPE_NAME, accessToken);
                var secondaryResponse = await httpClient.GetAsync(flightPath);

                secondaryResponse.EnsureSuccessStatusCode();

                return await secondaryResponse.Content.ReadFromJsonAsync<GetFlightOfferResponse>();
            }

            string response = await primaryResponse.Content.ReadAsStringAsync();
            throw new Exception($"Unexpected server response received from an external API. External API status code: {primaryResponse.StatusCode}. Response: {response}");
        }

        private async Task<AmadeusAuthResponse> RequestAccessTokenAsync()
        {
            httpClient.DefaultRequestHeaders.Clear();

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

            var httpResponse = await httpClient.SendAsync(request);

            httpResponse.EnsureSuccessStatusCode();

            return await httpResponse.Content.ReadFromJsonAsync<AmadeusAuthResponse>();
        }
    }
}
