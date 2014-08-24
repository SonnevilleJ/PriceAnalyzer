using System;
using System.Linq;
using NUnit.Framework;
using Sonneville.PriceTools.SampleData;

namespace Sonneville.PriceTools.PortfolioStatistics.Test
{
    [TestFixture]
    public class PortfolioStatisticsTest
    {
        private IProfitCalculator _profitCalculator;
        private IHoldingFactory _holdingFactory;

        [SetUp]
        public void Setup()
        {
            _profitCalculator = new ProfitCalculator();
            _holdingFactory = new HoldingFactory();
        }

        [Test]
        public void KellyPercentTest()
        {
            var basket = SamplePortfolios.FidelityBrokerageLink.TransactionHistory;
            var settlementDate = DateTime.Now;
            var holdings = _holdingFactory.CalculateHoldings(basket, settlementDate);
            var winPercentage = (decimal) holdings.Count(h => _profitCalculator.NetProfit(h) > 0)/holdings.Count;
            var lossPercentage = (decimal) holdings.Count(h => _profitCalculator.NetProfit(h) <= 0)/holdings.Count;
            var averageWin = holdings.Where(h => _profitCalculator.NetProfit(h) > 0).Average(h => _profitCalculator.NetProfit(h));
            var averageLoss = holdings.Where(h => _profitCalculator.NetProfit(h) <= 0).Average(h => Math.Abs(_profitCalculator.NetProfit(h)));

            var expected = winPercentage - ((lossPercentage/(averageWin/averageLoss)));
            var actual = holdings.KellyPercentage();
            Assert.AreEqual(expected, actual);
        }
    }
}
