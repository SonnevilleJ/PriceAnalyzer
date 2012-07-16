using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.SamplePortfolioData;

namespace PortfolioStatistics.Test
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
            var holdings = TransactionHistory.BrokerageLink_trades.Transactions.CalculateHoldings(DateTime.Now);
            var winPercentage = (decimal) holdings.Where(h => h.NetProfit() > 0).Count()/holdings.Count;
            var lossPercentage = (decimal) holdings.Where(h => h.NetProfit() <= 0).Count()/holdings.Count;
            var averageWin = holdings.Where(h => h.NetProfit() > 0).Average(h => h.NetProfit());
            var averageLoss = holdings.Where(h => h.NetProfit() <= 0).Average(h => h.NetProfit());

            var expected = winPercentage - ((lossPercentage/(averageWin/averageLoss)));
            var actual = holdings.KellyPercentage();
            Assert.AreEqual(expected, actual);
        }
    }
}
