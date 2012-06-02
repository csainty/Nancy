namespace Nancy.Session
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class InProcessSessionStore : ISessionStore
    {
        private static ConcurrentDictionary<string, object> store = new ConcurrentDictionary<string, object>();

        public IDictionary<string, object> Load(string key)
        {
            object o;
            if (!store.TryGetValue(key, out o))
                return null;
            return o as IDictionary<string, object>;
        }

        public void Save(string key, IDictionary<string, object> value)
        {
            store.AddOrUpdate(key, value, (k, v) => v);
        }
    }
}