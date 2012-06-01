using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Nancy.Session
{
    public class InProcessSessionStore : ISessionStore
    {
        private static InProcessSessionStore _Instance = new InProcessSessionStore();

        public static InProcessSessionStore Instance { get { return _Instance; } }

        private ConcurrentDictionary<string, object> _Store = new ConcurrentDictionary<string, object>();

        private InProcessSessionStore() { }

        public void Clear()
        {
            _Store.Clear();
        }

        public IDictionary<string, object> Load(string key)
        {
            object o;
            if (!_Store.TryGetValue(key, out o))
                return null;
            return o as IDictionary<string, object>;
        }

        public void Save(string key, IDictionary<string, object> value)
        {
            _Store.AddOrUpdate(key, value, (k, v) => v);
        }
    }
}