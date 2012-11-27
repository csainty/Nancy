﻿namespace Nancy.Demo.Sessions
{
    using Nancy.Bootstrapper;
    using Nancy.Session;
    using TinyIoC;

    public class SessionBootstrapper : DefaultNancyBootstrapper
    { 
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            InternalConfiguration.SessionStore = typeof(InProcessSessionStore);

            base.ConfigureApplicationContainer(container);
        }
    }
}