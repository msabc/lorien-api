namespace Lorien.Domain.Interfaces.Services.Caching
{
    public interface ICachingService
    {
        void Add<T>(string key, IEnumerable<T> items, bool keepIndefinitely = false);

        IEnumerable<T>? Get<T>(string key);

        void AddInitialMemoryCacheData();

        IEnumerable<(string, int)> GetCacheInfo();
    }
}
