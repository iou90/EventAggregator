using Kant.Tools.EventAggregator;
using Microsoft.VisualStudio.TestTools.UnitTesting; 

namespace TestEventAggregator
{
    [TestClass]
    public class TestEA
    {
        [TestMethod]
        public void Test()
        {
            var test = "";

            EventHubManager.Instance.GetEventWithChannel<string>("test").Subscribe(s =>
                {
                    test = s;
                });

            EventHubManager.Instance.PublishToChannel<string>("test", "hi");
            Assert.AreEqual("hi", test);
        }
    }
}
