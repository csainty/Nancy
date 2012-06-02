namespace Nancy.Tests.Unit.Sessions
{
    using System;
    using System.Collections.Generic;
    using FakeItEasy;
    using Nancy.Bootstrapper;
    using Nancy.Session;
    using Xunit;

    public class StoredSessionsFixture
    {
        private readonly StoredSessions storedSessions;
        private readonly ISessionStore sessionStore;

        public StoredSessionsFixture()
        {
            sessionStore = A.Fake<ISessionStore>();
            storedSessions = new StoredSessions(sessionStore);
        }

        [Fact]
        public void Should_add_beforerequest_hook()
        {
            var pipelines = A.Fake<IPipelines>();
            StoredSessions.Enable(pipelines, A.Fake<ISessionStore>());
            A.CallTo(() => pipelines.BeforeRequest.AddItemToEndOfPipeline(A<Func<NancyContext, Response>>.That.Not.IsNull())).MustHaveHappened();
        }

        [Fact]
        public void Should_add_afterrequest_hook()
        {
            var pipelines = A.Fake<IPipelines>();
            StoredSessions.Enable(pipelines, A.Fake<ISessionStore>());
            A.CallTo(() => pipelines.AfterRequest.AddItemToEndOfPipeline(A<Action<NancyContext>>.That.Not.IsNull())).MustHaveHappened();
        }

        [Fact]
        public void Should_have_a_cookie_name()
        {
            StoredSessions.GetCookieName().ShouldNotBeNull();
            StoredSessions.GetCookieName().ShouldNotBeEmpty();
        }

        [Fact]
        public void Should_create_an_empty_session_when_no_id_is_present()
        {
            var request = new Request("GET", "/", "http");
            storedSessions.Load(request);
            request.Session.ShouldNotBeNull();
            request.Session.Count.ShouldEqual(0);
            A.CallTo(() => sessionStore.Load(A<string>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public void Should_create_an_empty_session_when_an_empty_id_is_present()
        {
            var request = new Request("GET", "/", "http");
            request.Cookies.Add(StoredSessions.GetCookieName(), "");
            storedSessions.Load(request);
            request.Session.ShouldNotBeNull();
            request.Session.Count.ShouldEqual(0);
            A.CallTo(() => sessionStore.Load(A<string>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public void Should_create_an_empty_session_when_a_missing_id_is_present()
        {
            A.CallTo(() => sessionStore.Load("12345")).Returns(null);
            var request = new Request("GET", "/", "http");
            request.Cookies.Add(StoredSessions.GetCookieName(), "12345");
            storedSessions.Load(request);
            request.Session.ShouldNotBeNull();
            request.Session.Count.ShouldEqual(0);
            A.CallTo(() => sessionStore.Load("12345")).MustHaveHappened();
        }

        [Fact]
        public void Should_load_a_session_from_the_store()
        {
            A.CallTo(() => sessionStore.Load("12345")).Returns(new Dictionary<string, object> { { "Hello", "World" }, { "Number", 1 } });
            var request = new Request("GET", "/", "http");
            request.Cookies.Add(StoredSessions.GetCookieName(), "12345");
            storedSessions.Load(request);
            request.Session["Hello"].ShouldEqual("World");
            request.Session["Number"].ShouldEqual(1);
        }

        [Fact]
        public void Should_not_act_on_an_unchanged_session()
        {
            var session = A.Fake<ISession>();
            A.CallTo(() => session.HasChanged).Returns(false);

            var ctx = new NancyContext();
            ctx.Response = new Response();
            ctx.Request = new Request("GET", "/", "http");
            ctx.Request.Session = session;
            storedSessions.Save(ctx);

            A.CallTo(() => sessionStore.Save(A<string>.Ignored, A<IDictionary<string, object>>.Ignored)).MustNotHaveHappened();
            ctx.Response.Cookies.Count.ShouldEqual(0);
        }

        [Fact]
        public void It_should_create_a_session_id_if_required()
        {
            var session = A.Fake<ISession>();
            A.CallTo(() => session.HasChanged).Returns(true);

            var ctx = new NancyContext();
            ctx.Response = new Response();
            ctx.Request = new Request("GET", "/", "http");
            ctx.Request.Session = session;
            storedSessions.Save(ctx);

            ctx.Response.Cookies.Count.ShouldEqual(1);
        }


        [Fact]
        public void It_should_save_a_changed_session()
        {
            var session = A.Fake<ISession>();
            A.CallTo(() => session.HasChanged).Returns(true);

            var ctx = new NancyContext();
            ctx.Response = new Response();
            ctx.Request = new Request("GET", "/", "http");
            ctx.Request.Session = session;
            ctx.Request.Cookies.Add(StoredSessions.GetCookieName(), "12345");
            storedSessions.Save(ctx);

            A.CallTo(() => sessionStore.Save("12345", A<IDictionary<string, object>>.Ignored)).MustHaveHappened();
        }
    }
}
