using Lorien.Application.Exceptions;
using Lorien.Application.Models.Response.Internal;
using Lorien.Domain.Interfaces.Services.Caching;
using Microsoft.Extensions.Logging;

namespace Lorien.Application.Services.Internal
{
    public class InternalService(ILogger<InternalService> logger, ICachingService cachingService) : IInternalService
    {
        public Task<GetCachedItemsResponse> GetCachedItemsAsync()
        {
            try
            {
                IEnumerable<(string, int)> cacheInfoList = cachingService.GetCacheInfo();

                return Task.FromResult(new GetCachedItemsResponse
                {
                    CachedItems = cacheInfoList.Select(x => new CachedItem()
                    {
                        ItemName = x.Item1,
                        NumberOfRecords = x.Item2
                    })
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new CustomHttpException("An unexpected error has occurred: " + ex.Message);
            }
        }
    }
}
