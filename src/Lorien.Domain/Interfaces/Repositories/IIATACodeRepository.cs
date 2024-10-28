using Lorien.Domain.Models.Data;

namespace Lorien.Domain.Interfaces.Repositories
{
    public interface IIATACodeRepository
    {
        IEnumerable<IATACode> Get();
    }
}
