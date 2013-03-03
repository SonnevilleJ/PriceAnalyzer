using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.PortfolioStatistics;
using Test.Sonneville.PriceTools.PortfolioData;

namespace Test.Sonneville.PriceTools.PortfolioStatistics
{
    /// <summary>
    /// Summary description for PortfolioStatisticsTest
    /// </summary>
    [TestClass]
    public class PortfolioStatisticsTest
    {
        [TestMethod]
        public void KellyPercentTest()
        {
            var holdings = TestTransactionHistory.BrokerageLinkTransactions.CalculateHoldings(DateTime.Now);
            var winPercentage = (decimal) holdings.Count(h => h.NetProfit() > 0)/holdings.Count;
            var lossPercentage = (decimal) holdings.Count(h => h.NetProfit() <= 0)/holdings.Count;
            var averageWin = holdings.Where(h => h.NetProfit() > 0).Average(h => h.NetProfit());
            var averageLoss = holdings.Where(h => h.NetProfit() <= 0).Average(h => Math.Abs(h.NetProfit()));

            var expected = winPercentage - ((lossPercentage/(averageWin/averageLoss)));
            var actual = holdings.KellyPercentage();
            Assert.AreEqual(expected, actual);
        }
    }
}
