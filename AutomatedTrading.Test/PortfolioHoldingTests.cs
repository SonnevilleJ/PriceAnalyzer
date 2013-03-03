using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;

namespace Test.Sonneville.PriceTools.AutomatedTrading
{
    [TestClass]
    public class PortfolioHoldingTests
    {
        [TestMethod]
        public void CalculateHoldingsTestWithOnePositionOneBuyOneSell()
        {
            var dateTime = new DateTime(2011, 7, 26);
            const decimal deposit = 10000m;
            
            var buyDate = new DateTime(2011, 7, 26);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const decimal shares = 2;
            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice);

            var sellDate = new DateTime(2011, 9, 26);
            const decimal sellPrice = 75.00m;
            var sell = TransactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice);

            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit, buy, sell);

            var holdings = target.CalculateHoldings(sellDate);

            Assert.AreEqual(1, holdings.Count);
            var expected = HoldingFactory.ConstructHolding(ticker, buyDate, sellDate, shares, buyPrice, sellPrice);

            Assert.IsTrue(holdings.Contains(expected));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithOnePositionTwoBuysTwoSells()
        {
            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 5;      // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            const decimal deposit = 10000m;

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var firstBuy = TransactionFactory.ConstructBuy(ticker, firstBuyDate, sharesBought, buyPrice, commission);
            var secondBuy = TransactionFactory.ConstructBuy(ticker, secondBuyDate, sharesBought, buyPrice, commission);
            var firstSell = TransactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, sellPrice, commission);
            var secondSell = TransactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission);

            var target = PortfolioFactory.ConstructPortfolio(testDate, deposit, firstBuy, secondBuy, firstSell, secondSell);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const decimal sharesInHolding = sharesSold;
            var expected1 = HoldingFactory.ConstructHolding(ticker, secondBuyDate, secondSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);
            var expected2 = HoldingFactory.ConstructHolding(ticker, firstBuyDate, firstSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoPositionsOneBuyOneSellEach()
        {
            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const string firstTicker = "DE";
            const string secondTicker = "IBM";
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 5;      // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            const decimal deposit = 10000m;

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var firstBuy = TransactionFactory.ConstructBuy(firstTicker, firstBuyDate, sharesBought, buyPrice, commission);
            var secondBuy = TransactionFactory.ConstructBuy(secondTicker, secondBuyDate, sharesBought, buyPrice, commission);
            var firstSell = TransactionFactory.ConstructSell(firstTicker, firstSellDate, sharesSold, sellPrice, commission);
            var secondSell = TransactionFactory.ConstructSell(secondTicker, secondSellDate, sharesSold, sellPrice, commission);

            var target = PortfolioFactory.ConstructPortfolio(testDate, deposit, firstBuy, secondBuy, firstSell, secondSell);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const decimal sharesInHolding = sharesSold;
            var expected1 = HoldingFactory.ConstructHolding(secondTicker, secondBuyDate, secondSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);
            var expected2 = HoldingFactory.ConstructHolding(firstTicker, firstBuyDate, firstSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [TestMethod]
        public void CalculateHoldingsTestSortOrder()
        {
            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const string firstTicker = "DE";
            const string secondTicker = "IBM";
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 5;      // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            const decimal deposit = 10000m;

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var firstBuy = TransactionFactory.ConstructBuy(firstTicker, firstBuyDate, sharesBought, buyPrice, commission);
            var secondBuy = TransactionFactory.ConstructBuy(secondTicker, secondBuyDate, sharesBought, buyPrice, commission);
            var firstSell = TransactionFactory.ConstructSell(firstTicker, firstSellDate, sharesSold, sellPrice, commission);
            var secondSell = TransactionFactory.ConstructSell(secondTicker, secondSellDate, sharesSold, sellPrice, commission);

            var target = PortfolioFactory.ConstructPortfolio(testDate, deposit, firstBuy, secondBuy, firstSell, secondSell);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const decimal sharesInHolding = sharesSold;
            var expected1 = HoldingFactory.ConstructHolding(firstTicker, firstBuyDate, firstSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);
            var expected2 = HoldingFactory.ConstructHolding(secondTicker, secondBuyDate, secondSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);
            var holding1 = holdings[0];
            var holding2 = holdings[1];
            Assert.AreEqual(expected1, holding1);
            Assert.AreEqual(expected2, holding2);
        }
    }
}