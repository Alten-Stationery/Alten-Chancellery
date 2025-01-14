namespace ServiceLayer.Cache
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
        void Set<T>(string key, T value, TimeSpan duration);
    }
}
