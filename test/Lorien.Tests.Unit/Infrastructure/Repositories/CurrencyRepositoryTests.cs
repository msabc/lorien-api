using Lorien.Domain.Interfaces.Repositories;
using Moq;

namespace Lorien.Tests.Unit.Infrastructure.Repositories
{
    public class CurrencyRepositoryTests
    {
        private readonly Mock<ICurrencyRepository> _currencyRepositoryMock;

        public CurrencyRepositoryTests() 
        {
            _currencyRepositoryMock = new Mock<ICurrencyRepository>();
        }

        public void Get_AlwaysFiltersInvalidCurrencies()
        {
        }
    }
}
