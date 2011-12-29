﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;
using Sonneville.Utilities;

namespace Sonneville.TradingTest
{
    [TestClass]
    public class TradingStrategyTest
    {
        [TestMethod]
        public void PriceSeriesBeforeStartTest()
        {
            const string ticker = "DE";
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker);

            var target = GetTradingStrategy();

            target.PriceSeries = priceSeries;

            Assert.AreEqual(priceSeries, target.PriceSeries);
        }

        [TestMethod]
        public void TradingAccountBeforeStartTest()
        {
            var tradingAccount = TestUtilities.CreateSimulatedTradingAccount();

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
