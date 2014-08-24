using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class HoldingExtensionsTests
    {
        private IHoldingFactory _holdingFactory;
        private IProfitCalculator _profitCalculator;

        [SetUp]
        public void Setup()
        {
            _holdingFactory = new HoldingFactory();
            _profitCalculator = new ProfitCalculator();
        }

        [Test]
        public void GrossProfitTest()
        {
            var target = _holdingFactory.ConstructHolding(5, 10, 20, 2, 3);

            var expected = (target.Shares*(target.ClosePrice - target.OpenPrice));
            var actual = _profitCalculator.GrossProfit(target);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NetProfitTest()
        {
            var target = _holdingFactory.ConstructHolding(5, 10, 20, 2, 3);

            var expected = (target.Shares * (target.ClosePrice - target.OpenPrice)) - target.OpenCommission - target.CloseCommission;
            var actual = _profitCalculator.NetProfit(target);
            Assert.AreEqual(expected, actual);
        }
    }
}
