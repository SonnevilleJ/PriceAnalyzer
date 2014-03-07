using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class HoldingExtensionsTests
    {
        private IHoldingFactory _holdingFactory;
        private ProfitCalculator _profitCalculator;

        [TestInitialize]
        public void Initialize()
        {
            _holdingFactory = new HoldingFactory();
            _profitCalculator = new ProfitCalculator();
        }

        [TestMethod]
        public void GrossProfitTest()
        {
            var target = _holdingFactory.ConstructHolding(5, 10, 20, 2, 3);

            var expected = (target.Shares*(target.ClosePrice - target.OpenPrice));
            var actual = _profitCalculator.GrossProfit(target);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NetProfitTest()
        {
            var target = _holdingFactory.ConstructHolding(5, 10, 20, 2, 3);

            var expected = (target.Shares * (target.ClosePrice - target.OpenPrice)) - target.OpenCommission - target.CloseCommission;
            var actual = _profitCalculator.NetProfit(target);
            Assert.AreEqual(expected, actual);
        }
    }
}
