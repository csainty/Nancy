namespace Nancy.Session
{
    using System.Collections.Generic;

    public interface ISessionStore
    {
        IDictionary<string,object> Load(string key);

        void Save(string key, IDictionary<string, object> value);
    }
}