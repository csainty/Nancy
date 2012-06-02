namespace Nancy.Cache
{
    public interface ICacheStore
    {
        T Load<T>(string key);
        void Save<T>(string key, T obj);
    }
}
