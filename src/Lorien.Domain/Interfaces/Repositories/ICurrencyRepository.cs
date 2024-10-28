using Lorien.Domain.Models.Data;

namespace Lorien.Domain.Interfaces.Repositories
{
    public interface ICurrencyRepository
    {
        IEnumerable<Currency> Get();
    }
}
