namespace Nancy.Session
{
    using System.Collections.Generic;
    using Bootstrapper;

    public class SessionApplicationStartup : IApplicationStartup
    {
        public void Initialize(IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToEndOfPipeline(LoadSession);
            pipelines.AfterRequest.AddItemToEndOfPipeline(SaveSession);
        }

        public static Response LoadSession(NancyContext ctx)
        {
            if (ctx.Request == null || ctx.SessionStore == null)
            {
                return null;
            }

            IDictionary<string, object> items;

            if (ctx.SessionStore.TryLoadSession(ctx.Request, out items))
            {
                ctx.Request.Session = new DynamicSession(items);
            }
            else
            {
                ctx.Request.Session = new DynamicSession();
            }
            
            return null;
        }

        public static void SaveSession(NancyContext ctx)
        {
            if (ctx.Request == null || ctx.Request.Session == null || ctx.SessionStore == null || !ctx.Request.Session.HasChanged)
            {
                return;
            }

            ctx.SessionStore.SaveSession(ctx, ctx.Request.Session);
        }
    }
}
