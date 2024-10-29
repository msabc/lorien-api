namespace Lorien.Domain.Models.Caching
{
    public class CacheObject<T>(IEnumerable<T> items) : ICacheObject
    {
        public IEnumerable<T> Items { get; } = items;

        public int CachedItemsCount { get => Items.Count(); }
    }

    public interface ICacheObject
    {
        public int CachedItemsCount { get; }
    }
}
