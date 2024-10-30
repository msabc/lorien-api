using Lorien.Domain.Interfaces.Repositories;
using Lorien.Domain.Models.Data;
using Lorien.Infrastructure.Properties;
using System.Text.Json;

namespace Lorien.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        public IEnumerable<Currency> Get()
        {
            var currencies = JsonSerializer.Deserialize<IEnumerable<Currency>>(Resources.currency_codes);
            return currencies!.Where(x => x.Code is not null && x.WithdrawalDate is null).DistinctBy(x => x.Code);
        }
    }
}
