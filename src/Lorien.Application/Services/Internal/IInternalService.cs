using Lorien.Application.Models.Response.Internal;

namespace Lorien.Application.Services.Internal
{
    public interface IInternalService
    {
        Task<GetCachedItemsResponse> GetCachedItemsAsync();
    }
}
