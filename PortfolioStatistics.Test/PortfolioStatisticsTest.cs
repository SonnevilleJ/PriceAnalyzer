using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.SampleData;

namespace Sonneville.PriceTools.PortfolioStatistics.Test
{
    /// <summary>
    /// Summary description for PortfolioStatisticsTest
    /// </summary>
    [TestClass]
    public class PortfolioStatisticsTest
    {
        private IProfitCalculator _profitCalculator;
        private IHoldingFactory _holdingFactory;

        [TestInitialize]
        public void Initialize()
        {
            _profitCalculator = new ProfitCalculator();
            _holdingFactory = new HoldingFactory();
        }

        [TestMethod]
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
