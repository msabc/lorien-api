using Lorien.Configuration;
using Lorien.Domain.Interfaces.Repositories;
using Lorien.Domain.Interfaces.Services.Caching;
using Lorien.Domain.Models.Caching;
using Lorien.Domain.Models.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lorien.Infrastructure.Services.Caching
{
    public class CachingService(
        IMemoryCache memoryCache,
        IOptions<LorienSettings> options,
        ICurrencyRepository currencyRepository,
        IIATACodeRepository iataCodeRepository,
        ILogger<CachingService> logger) : ICachingService
    {
        public void Add<T>(string key, IEnumerable<T> items, bool keepIndefinitely = false)
        {
            memoryCache.Set(key, items,
                keepIndefinitely ?
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(int.MaxValue)) :
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(options.Value.RequestCachingTimeToLiveInMinutes)));
        }

        public IEnumerable<T>? Get<T>(string key)
        {
            if (memoryCache.TryGetValue(key, out IEnumerable<T>? value))
                return value;

            return null;
        }

        public void AddInitialMemoryCacheData()
        {
            try
            {
                var infiniteExpirationOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(int.MaxValue));

                memoryCache.Set(nameof(CacheKeys.Currencies), new CacheObject<Currency>(currencyRepository.Get()), infiniteExpirationOptions);
                memoryCache.Set(nameof(CacheKeys.IATACodes), new CacheObject<IATACode>(iataCodeRepository.Get()), infiniteExpirationOptions);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error saving data to memory cache. {ex.Message}");
            }

        }

        public IEnumerable<(string, int)> GetCacheInfo()
        {
            var cacheInfoList = new List<(string, int)>();
            foreach (CacheKeys key in (CacheKeys[])Enum.GetValues(typeof(CacheKeys)))
            {
                bool cacheKeyFound = memoryCache.TryGetValue(key.ToString(), out object cachedItem);

                if (cacheKeyFound && cachedItem is ICacheObject cachedObject)
                    cacheInfoList.Add((key.ToString(), cachedObject.CachedItemsCount));
            }

            return cacheInfoList;
        }
    }
}
