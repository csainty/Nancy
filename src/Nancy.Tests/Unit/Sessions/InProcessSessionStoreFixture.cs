namespace Nancy.Tests.Unit.Sessions
{
    using System.Collections.Generic;
    using Nancy.Session;
    using Xunit;

    public class IntegrationTests
    {
        private InProcessSessionStore store = new InProcessSessionStore();

        [Fact]
        public void Should_be_able_to_store_and_retrieve_a_value()
        {
            store.Save("Test", new Dictionary<string, object> { { "Key", "Value" } });
            var d = store.Load("Test");
            d.ShouldNotBeNull();
            d["Key"].ShouldEqual("Value");
        }

        [Fact]
        public void Should_work_across_instances()
        {
            store.Save("T", new Dictionary<string, object> { { "Key", "Value" } });

            var s = new InProcessSessionStore();
            s.Load("T").ShouldNotBeNull();
        }

        [Fact]
        public void Should_return_null_when_not_found()
        {
            store.Load("Foo").ShouldBeNull();
        }
    }
}