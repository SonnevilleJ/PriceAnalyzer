using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class HoldingExtensionsTests
    {
        private readonly HoldingFactory _holdingFactory;

        public HoldingExtensionsTests()
        {
            _holdingFactory = new HoldingFactory();
        }

        [TestMethod]
        public void GrossProfitTest()
        {
            var target = _holdingFactory.ConstructHolding(5, 10, 20, 2, 3);

            var expected = (target.Shares*(target.ClosePrice - target.OpenPrice));
            var actual = target.GrossProfit();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NetProfitTest()
        {
            var target = _holdingFactory.ConstructHolding(5, 10, 20, 2, 3);

            var expected = (target.Shares * (target.ClosePrice - target.OpenPrice)) - target.OpenCommission - target.CloseCommission;
            var actual = target.NetProfit();
            Assert.AreEqual(expected, actual);
        }
    }
}
