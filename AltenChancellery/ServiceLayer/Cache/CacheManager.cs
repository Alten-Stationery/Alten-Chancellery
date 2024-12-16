using Microsoft.Extensions.Caching.Memory;

namespace ServiceLayer.Cache
{
    public class CacheManager : ICacheManager
    {
        private readonly IMemoryCache cache;
        private readonly TimeSpan cacheDuration = TimeSpan.FromMinutes(5);

        public CacheManager(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public T Get<T>(string key)
        {
            return (T)cache.Get(key)!;
        }

        public void Set<T>(string key, T value)
        {
            cache.Set(key, value, cacheDuration);
        }

        public void Set<T>(string key, T value, TimeSpan duration)
        {
            cache.Set(key, value, duration);
        }
    }
}
