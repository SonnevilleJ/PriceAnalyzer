using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PortfolioHoldingTests
    {
        [TestMethod]
        public void CalculateHoldingsTestWithOnePositionOneBuyOneSell()
        {
            var dateTime = new DateTime(2011, 7, 26);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 7, 26);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const double shares = 2;
            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice);
            target.AddTransaction(buy);

            var sellDate = new DateTime(2011, 9, 26);
            const decimal sellPrice = 75.00m;
            var sell = TransactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice);
            target.AddTransaction(sell);

            var holdings = target.CalculateHoldings(sellDate);

            Assert.AreEqual(1, holdings.Count);
            var expected = new Holding
                               {
                                   Ticker = ticker,
                                   Head = buyDate,
                                   Tail = sellDate,
                                   Shares = shares,
                                   OpenPrice = buyPrice,
                                   ClosePrice = sellPrice
                               };

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
            const double sharesBought = 5;      // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(testDate, deposit);

            target.AddTransaction(TransactionFactory.ConstructBuy(ticker, firstBuyDate, sharesBought, buyPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructBuy(ticker, secondBuyDate, sharesBought, buyPrice, commission));

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.AddTransaction(TransactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, sellPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const double sharesInHolding = sharesSold;
            var expected1 = new Holding
                                {
                                    Ticker = ticker,
                                    Head = secondBuyDate,
                                    Tail = secondSellDate,
                                    Shares = sharesInHolding,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };
            var expected2 = new Holding
                                {
                                    Ticker = ticker,
                                    Head = firstBuyDate,
                                    Tail = firstSellDate,
                                    Shares = sharesInHolding,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };

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
            const double sharesBought = 5;      // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(testDate, deposit);

            target.AddTransaction(TransactionFactory.ConstructBuy(firstTicker, firstBuyDate, sharesBought, buyPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructBuy(secondTicker, secondBuyDate, sharesBought, buyPrice, commission));

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.AddTransaction(TransactionFactory.ConstructSell(firstTicker, firstSellDate, sharesSold, sellPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(secondTicker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const double sharesInHolding = sharesSold;
            var expected1 = new Holding
                                {
                                    Ticker = secondTicker,
                                    Head = secondBuyDate,
                                    Tail = secondSellDate,
                                    Shares = sharesInHolding,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };
            var expected2 = new Holding
                                {
                                    Ticker = firstTicker,
                                    Head = firstBuyDate,
                                    Tail = firstSellDate,
                                    Shares = sharesInHolding,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };

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
            const double sharesBought = 5;      // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(testDate, deposit);

            target.AddTransaction(TransactionFactory.ConstructBuy(firstTicker, firstBuyDate, sharesBought, buyPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructBuy(secondTicker, secondBuyDate, sharesBought, buyPrice, commission));

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.AddTransaction(TransactionFactory.ConstructSell(firstTicker, firstSellDate, sharesSold, sellPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(secondTicker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const double sharesInHolding = sharesSold;
            var expected1 = new Holding
                                {
                                    Ticker = secondTicker,
                                    Head = secondBuyDate,
                                    Tail = secondSellDate,
                                    Shares = sharesInHolding,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };
            var expected2 = new Holding
                                {
                                    Ticker = firstTicker,
                                    Head = firstBuyDate,
                                    Tail = firstSellDate,
                                    Shares = sharesInHolding,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };
            var holding1 = holdings[0];
            var holding2 = holdings[1];
            Assert.AreEqual(expected1, holding1);
            Assert.AreEqual(expected2, holding2);
        }
    }
}