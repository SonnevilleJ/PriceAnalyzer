using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class HoldingExtensionsTests
    {
        [TestMethod]
        public void GrossProfitTest()
        {
            var target = HoldingFactory.ConstructHolding(5, 10, 20, 2, 3);

            var expected = (target.Shares*(target.ClosePrice - target.OpenPrice));
            var actual = target.GrossProfit();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NetProfitTest()
        {
            var target = HoldingFactory.ConstructHolding(5, 10, 20, 2, 3);

            var expected = (target.Shares * (target.ClosePrice - target.OpenPrice)) - target.OpenCommission - target.CloseCommission;
            var actual = target.NetProfit();
            Assert.AreEqual(expected, actual);
        }
    }
}
