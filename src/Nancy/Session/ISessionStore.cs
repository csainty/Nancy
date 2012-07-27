namespace Nancy.Session
{
    using System.Collections.Generic;

    public interface ISessionStore
    {
        bool TryLoadSession(Request request, out IDictionary<string, object> items);
        
        void SaveSession(NancyContext context, IDictionary<string, object> items);
    }
}
