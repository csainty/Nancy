using System.Collections.Generic;

namespace Nancy
{
    public interface ISessionStore
    {
        IDictionary<string,object> Load(string key);

        void Save(string key, IDictionary<string, object> value);
    }
}