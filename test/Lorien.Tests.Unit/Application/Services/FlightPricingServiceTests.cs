using Lorien.Application.Models.Request;
using Lorien.Application.Services.Flights;
using Lorien.Configuration;
using Lorien.Domain.Interfaces.Services.Caching;
using Lorien.Domain.Interfaces.Services.Flights;
using Lorien.Domain.Models.Request;
using Lorien.Domain.Models.Response.Flights;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Lorien.Tests.Unit.Application.Services
{
    public class FlightPricingServiceTests
    {
        private readonly Mock<IAmadeusCRSHttpService> _amadeusCRSHttpServiceMock;
        private readonly Mock<ICachingService> _cachingServiceMock;
        private readonly Mock<ILogger<FlightPricingService>> _loggerMock;
        private readonly Mock<IOptions<LorienSettings>> _settingsMock;

        private readonly IFlightPricingService _flightPricingService;

        public FlightPricingServiceTests()
        {
            _amadeusCRSHttpServiceMock = new Mock<IAmadeusCRSHttpService>();
            _cachingServiceMock = new Mock<ICachingService>();
            _loggerMock = new Mock<ILogger<FlightPricingService>>();
            _settingsMock = new Mock<IOptions<LorienSettings>>();

            _flightPricingService = new FlightPricingService(
                _amadeusCRSHttpServiceMock.Object,
                _cachingServiceMock.Object,
                _loggerMock.Object,
                _settingsMock.Object
            );
        }

        [Fact]
        public void GetFlightPricingAsync_ItemIsCached_ExternalApiIsNotCalled()
        {
            SetupMocks();

            var request = GetTestRequest();

            _flightPricingService.GetFlightPricingAsync(request);

            _amadeusCRSHttpServiceMock.Verify(x => x.GetFlightOffers(It.IsAny<GetFlightOfferRequest>()), Times.Never);
        }

        [Fact]
        public void GetFlightPricingAsync_ItemIsNotCached_ExternalApiIsCalled()
        {
            var request = GetTestRequest();

            _cachingServiceMock
                .Setup(x => x.Get<IEnumerable<FlightOfferData>>(It.IsAny<string>()))
                .Returns(() => []);

            _flightPricingService.GetFlightPricingAsync(request);

            _amadeusCRSHttpServiceMock.Verify(x => x.GetFlightOffers(It.IsAny<GetFlightOfferRequest>()), Times.Once);
        }

        [Fact]
        public void GetFlightPricingAsync_AnyRequest_CacheIsAlwaysRetrievedUsingSpecificKey()
        {
            var request = GetTestRequest();

            _cachingServiceMock
                .Setup(x => x.Get<IEnumerable<FlightOfferData>>(It.IsAny<string>()))
                .Returns(() => []);

            _flightPricingService.GetFlightPricingAsync(request);

            _amadeusCRSHttpServiceMock.Verify(x => x.GetFlightOffers(It.IsAny<GetFlightOfferRequest>()), Times.Once);
        }

        [Fact]
        public void GetFlightPricingAsync_ValidRequest_CacheIsAlwaysAddedToUsingSpecificKey()
        {
            SetupMocks();

            var request = GetTestRequest();

            _cachingServiceMock
                .Setup(x => x.Get<IEnumerable<FlightOfferData>>(It.IsAny<string>()))
                .Returns(() => []);

            _flightPricingService.GetFlightPricingAsync(request);

            _cachingServiceMock
                .Verify(x => x.Add(request.ToString(), It.IsAny<IEnumerable<FlightOfferData>>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void GetFlightPricingAsync_ValidRequest_CacheIsAlwaysRetrievedFromUsingSpecificKey()
        {
            var request = GetTestRequest();

            _flightPricingService.GetFlightPricingAsync(request);

            _cachingServiceMock
                .Verify(x => x.Get<IEnumerable<FlightOfferData>>(request.ToString()), Times.Once);
        }

        private static GetFlightPricingRequest GetTestRequest()
        {
            return new GetFlightPricingRequest()
            {
                DestinationLocationCode = "SYD",
                OriginLocationCode = "PAR",
                DepartureDate = DateTime.UtcNow,
                Adults = 1
            };
        }

        private void SetupMocks()
        {
            var testFlightOffers = new List<FlightOfferData>()
            {
                new()
                {
                    Id = "1",
                    NumberOfBookableSeats = 1,
                    OneWay = true,
                    Source = "something",
                    Type = "something",
                    Price = new FlightPrice(){
                        Base = "150",
                        Currency = "EUR",
                        Fees = [],
                        GrandTotal = "150",
                        Total = "150"
                    },
                    Itineraries = []
                },
                new()
                {
                    Id = "2",
                    NumberOfBookableSeats = 1,
                    OneWay = true,
                    Source = "something",
                    Type = "something",
                    Price = new FlightPrice(){
                        Base = "150",
                        Currency = "EUR",
                        Fees = [],
                        GrandTotal = "150",
                        Total = "150"
                    },
                    Itineraries = []
                }
            };

            _cachingServiceMock
                .Setup(x => x.Get<IEnumerable<FlightOfferData>>(It.IsAny<string>()))
                .Returns(() => testFlightOffers);

            _amadeusCRSHttpServiceMock
                .Setup(x => x.GetFlightOffers(It.IsAny<GetFlightOfferRequest>()))
                .ReturnsAsync(() => new GetFlightOfferResponse() { Data = testFlightOffers });
        }
    }
}
