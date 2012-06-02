namespace Nancy.Cache
{
    using System.Collections.Concurrent;

    public class InProcessCacheStore : ICacheStore
    {
        private static ConcurrentDictionary<string, object> store = new ConcurrentDictionary<string, object>();
        
        public T Load<T>(string key)
        {
            object o;
            if (!store.TryGetValue(key, out o) || !(o is T))
            {
                return default(T);
            }
            return (T)o;
        }

        public void Save<T>(string key, T obj)
        {
            store.AddOrUpdate(key, obj, (k, v) => v);
        }
    }
}
