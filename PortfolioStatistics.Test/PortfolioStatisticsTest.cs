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
        private ProfitCalculator _profitCalculator;
        private ISecurityBasketExtensions _securityBasketExtensions;

        [TestInitialize]
        public void Initialize()
        {
            _profitCalculator = new ProfitCalculator();
            _securityBasketExtensions = new SecurityBasketExtensions();
        }

        [TestMethod]
        public void KellyPercentTest()
        {
            var holdings = _securityBasketExtensions.CalculateHoldings(SamplePortfolios.FidelityBrokerageLink.TransactionHistory, DateTime.Now);
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
