using System;
using System.Collections.Generic;
using Nancy.Bootstrapper;

namespace Nancy.Session
{
    public class StoredSessions
    {
        private readonly ISessionStore store;

        public StoredSessions(ISessionStore store)
        {
            this.store = store;
        }

        public void Load(Request request)
        {
            IDictionary<string, object> items = null;
            if (request.Cookies.ContainsKey(GetCookieName()))
            {
                var id = request.Cookies[GetCookieName()];
                if (!string.IsNullOrEmpty(id))
                {
                    items = store.Load(id);
                }
            }
            request.Session = new Session(items ?? new Dictionary<string, object>());
        }

        public void Save(NancyContext ctx)
        {
            if (!ctx.Request.Session.HasChanged)
                return;

            string id;
            if (ctx.Request.Cookies.ContainsKey(GetCookieName()))
            {
                id = ctx.Request.Cookies[GetCookieName()];
            }
            else
            {
                // TODO: Should we give a way to override how the id is generated?
                // TODO: Should we encrypt / hash the id so people can not just try out other values?
                id = Guid.NewGuid().ToString();
                ctx.Response.AddCookie(GetCookieName(), id);
            }

            IDictionary<string, object> items = new Dictionary<string, object>();
            foreach (var item in ctx.Request.Session)
            {
                items.Add(item.Key, item.Value);
            }

            store.Save(id, items);
        }

        private static string cookieName = "_nsid";

        public static string GetCookieName()
        {
            return cookieName;
        }

        public static void Enable(IPipelines pipelines, ISessionStore store)
        {
            var handler = new StoredSessions(store);
            pipelines.BeforeRequest.AddItemToEndOfPipeline(ctx => LoadSession(ctx.Request, handler));
            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx => SaveSession(ctx, handler));
        }

        private static Response LoadSession(Request request, StoredSessions handler)
        {
            if (request == null)
            {
                return null;
            }

            handler.Load(request);

            return null;
        }

        private static void SaveSession(NancyContext ctx, StoredSessions handler)
        {
            if (ctx.Request == null || ctx.Request.Session == null)
            {
                return;
            }

            handler.Save(ctx);
        }
    }
}