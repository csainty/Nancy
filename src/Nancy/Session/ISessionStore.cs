namespace Nancy.Session
{
    using System.Collections.Generic;

    public interface ISessionStore
    {
        bool TryLoadSession(Request request, out IDictionary<string, object> items);
        
        void SaveSession(NancyContext request, IDictionary<string, object> items);
    }
}
