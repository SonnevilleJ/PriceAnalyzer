using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.AutomatedTrading.Test
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
