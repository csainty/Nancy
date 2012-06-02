namespace Nancy.Tests.Unit.Cache
{
    using Nancy.Cache;
    using Xunit;

    public class InProcessCacheStoreFixture
    {
        private InProcessCacheStore store = new InProcessCacheStore();

        [Fact]
        public void Can_store_and_retrieve_an_object()
        {
            store.Save("Key", "Value");
            store.Save("Number", 1);

            store.Load<string>("Key").ShouldEqual("Value");
            store.Load<int>("Number").ShouldEqual(1);
        }

        [Fact]
        public void Should_work_across_instances()
        {
            store.Save("Test", "Hello World!");

            var s = new InProcessCacheStore();
            s.Load<string>("Test").ShouldEqual("Hello World!");
        }

        [Fact]
        public void Should_not_error()
        {
            store.Load<int>("foo").ShouldEqual(0);
            store.Load<object>("foo").ShouldBeNull();
        }
    }
}
