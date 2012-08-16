using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.AutomatedTrading;

namespace Test.Sonneville.PriceTools.AutomatedTrading
{
    [TestClass]
    public class ContinuousTradingEngineTest
    {
        [TestMethod]
        public void Debug()
        {
            var engine = new ContinuousTradingEngine();
            engine.Start();

            Thread.Sleep(120000);

            engine.Stop();
        }
    }
}
