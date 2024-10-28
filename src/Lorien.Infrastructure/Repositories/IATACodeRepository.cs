using Lorien.Domain.Interfaces.Repositories;
using Lorien.Domain.Models.Data;
using Lorien.Infrastructure.Properties;
using System.Text.Json;

namespace Lorien.Infrastructure.Repositories
{
    public class IATACodeRepository : IIATACodeRepository
    {
        public IEnumerable<IATACode> Get()
        {
            return JsonSerializer.Deserialize<IEnumerable<IATACode>>(Resources.airports).Where(x => !string.IsNullOrEmpty(x.Name));
        }
    }
}
