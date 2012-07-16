namespace Nancy
{
    using Session;

    /// <summary>
    /// Creates NancyContext instances
    /// </summary>
    public class DefaultNancyContextFactory : INancyContextFactory
    {
        private readonly ISessionStore sessionStore;

        public DefaultNancyContextFactory(ISessionStore sessionStore)
        {
            this.sessionStore = sessionStore;
        }

        /// <summary>
        /// Create a new NancyContext
        /// </summary>
        /// <returns>NancyContext instance</returns>
        public NancyContext Create()
        {
            var nancyContext = new NancyContext();

            nancyContext.SessionStore = sessionStore;
            nancyContext.Trace.TraceLog.WriteLog(s => s.AppendLine("New Request Started"));

            return nancyContext;
        }
    }
}