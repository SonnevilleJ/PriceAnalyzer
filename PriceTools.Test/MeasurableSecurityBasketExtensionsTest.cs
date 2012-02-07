﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class MeasurableSecurityBasketExtensionsTest
    {
        [TestMethod]
        public void PositionCalculateAverageAnnualReturnTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sellPrice = 112.00m;      // $112.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            target.Buy(buyDate, shares, buyPrice, commission);
            target.Sell(sellDate, shares, sellPrice, commission);

            const decimal expectedReturn = 0.1m;    // 10% return; profit = $50 after commissions; initial investment = $500
            var actualReturn = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = 0.5m;          // 50% annual rate return
            var actual = target.CalculateAverageAnnualReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateAverageAnnualReturnWithoutProceedsTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal price = 100.00m;          // $100.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            target.Buy(buyDate, shares, price, commission);

            Assert.IsNull(target.CalculateAverageAnnualReturn(sellDate));
        }

        [TestMethod]
        public void PositionCalculateGrossReturnTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const decimal priceBought = 100.00m;    // $100.00 per share
            const double sharesBought = 10;         // 10 shares
            const decimal commission = 7.95m;       // with $7.95 commission
            const decimal increase = 0.10m;         // 10% price increase when sold
            const decimal priceSold = priceBought * (1 + increase);
            const double sharesSold = sharesBought - 2;
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, priceBought, sharesBought, commission);
            var sell = TransactionFactory.ConstructSell(sellDate, ticker, priceSold, sharesSold, commission);

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = increase;
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PortfolioCalculateGrossReturnTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const string msft = "MSFT";
            const decimal dePriceBought = 100.00m;
            const decimal dePriceSold = 110.00m;
            const decimal msftPriceBought = 50.00m;
            const decimal msftPriceSold = 60.00m;
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(buyDate, de, dePriceBought, sharesBought, commission);
            var deSell = TransactionFactory.ConstructSell(sellDate, de, dePriceSold, sharesSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(buyDate, msft, msftPriceBought, sharesBought, commission);
            var msftSell = TransactionFactory.ConstructSell(sellDate, msft, msftPriceSold, sharesSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var grossProfit = target.CalculateGrossProfit(sellDate);

            const decimal deReturn = (dePriceSold / dePriceBought) - 1;
            var deProfit = target.GetPosition(de).CalculateGrossProfit(sellDate);
            var deWeight = deProfit / grossProfit;
            
            const decimal msftReturn = (msftPriceSold / msftPriceBought) - 1;
            var msftProfit = target.GetPosition(msft).CalculateGrossProfit(sellDate);
            var msftWeight = msftProfit / grossProfit;

            var expected = (deReturn*deWeight) + (msftReturn*msftWeight);
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateGrossReturnWithoutProceedsTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 7.95m;   // with $7.95 commission

            target.Buy(buyDate, shares, price, commission);

            Assert.IsNull(target.CalculateGrossReturn(sellDate));
        }

        [TestMethod]
        public void PositionCalculateNetReturnTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission

            target.Buy(buyDate, shares, price, commission);
            target.Sell(sellDate, shares, price + 2.00m, commission);

            const decimal expected = 0.00m;      // 0% return; 100% of original investment
            var actual = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateNetReturnTest2()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 0.00m;    // with $0 commission

            target.Buy(buyDate, shares, price, commission);
            target.Sell(sellDate, shares, price * 2m, commission);

            const decimal expected = 1.00m;      // 100% return; 200% of original investment
            var actual = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateNetReturnWithoutProceedsTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission

            target.Buy(buyDate, shares, price, commission);

            Assert.IsNull(target.CalculateNetReturn(sellDate));
        }
    }
}
