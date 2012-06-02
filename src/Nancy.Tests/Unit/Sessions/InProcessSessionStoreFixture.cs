namespace Nancy.Tests.Unit.Sessions
{
    using System.Collections.Generic;
    using Nancy.Session;
    using Xunit;

    public class IntegrationTests
    {
        private InProcessSessionStore store = InProcessSessionStore.Instance;

        [Fact]
        public void Should_be_able_to_store_and_retrieve_a_value()
        {
            store.Save("Test", new Dictionary<string, object> { { "Key", "Value" } });
            var d = store.Load("Test");
            d.ShouldNotBeNull();
            d["Key"].ShouldEqual("Value");
        }
    }
}