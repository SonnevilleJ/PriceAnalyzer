using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools.AutomatedTrading
{
    [TestClass]
    public class TradingStrategyTest
    {
        [TestMethod]
        public void PriceSeriesBeforeStartTest()
        {
            var ticker = TickerManager.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker);

            var target = GetTradingStrategy();

            target.PriceSeries = priceSeries;

            Assert.AreEqual(priceSeries, target.PriceSeries);
        }

        [TestMethod]
        public void TradingAccountBeforeStartTest()
        {
            var tradingAccount = TradingAccountUtilities.CreateSimulatedTradingAccount();

            var target = GetTradingStrategy();

            target.TradingAccount = tradingAccount;

            Assert.AreEqual(tradingAccount, target.TradingAccount);
        }

        private static TradingStrategy GetTradingStrategy()
        {
            return new DailyTriggerTradingStrategy();
        }
    }
}
