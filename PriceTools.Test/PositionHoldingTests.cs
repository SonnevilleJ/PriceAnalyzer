﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PositionHoldingTests
    {
        [TestMethod]
        public void CalculateHoldingsTestWithOneBuyOneSellCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            var sellDate = buyDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(sellDate);

            Assert.AreEqual(1, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithOneBuyOneSellValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            var sellDate = buyDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(sellDate);

            var expected = new Holding
                               {
                                   Ticker = ticker,
                                   Head = buyDate,
                                   Tail = sellDate,
                                   Shares = sharesSold,
                                   OpenPrice = buyPrice,
                                   OpenCommission = commission,
                                   ClosePrice = sellPrice,
                                   CloseCommission = commission
                               };

            Assert.IsTrue(holdings.Contains(expected));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysOneSellCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 3;      // 3 shares

            target.Buy(firstBuyDate, sharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, sharesBought, buyPrice, commission);

            var sellDate = secondBuyDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 6;        // 6 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(sellDate);

            Assert.AreEqual(2, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysOneSellValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 3;      // 3 shares

            target.Buy(firstBuyDate, sharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, sharesBought, buyPrice, commission);

            var sellDate = secondBuyDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 6;        // 6 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(sellDate);

            var expected1 = new Holding
                                {
                                    Ticker = ticker,
                                    Head = secondBuyDate,
                                    Tail = sellDate,
                                    Shares = sharesBought,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };
            var expected2 = new Holding
                                {
                                    Ticker = ticker,
                                    Head = firstBuyDate,
                                    Tail = sellDate,
                                    Shares = sharesBought,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithOneBuyTwoSellsCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            var firstSellDate = buyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithOneBuyTwoSellsValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            var firstSellDate = buyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            const double sharesInHolding = sharesSold;

            var expected1 = new Holding
                                {
                                    Ticker = ticker,
                                    Head = buyDate,
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
                                    Head = buyDate,
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
        public void CalculateHoldingsTestWithTwoBuysTwoSellsCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 5;      // 5 shares

            target.Buy(firstBuyDate, sharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, sharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 5;      // 5 shares

            target.Buy(firstBuyDate, sharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, sharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

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
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 5;      // 5 shares

            target.Buy(firstBuyDate, sharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, sharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 5;      // 5 shares

            target.Buy(firstBuyDate, sharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, sharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

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
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingUnevenCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double firstSharesBought = 9;
            const double secondSharesBought = 1;

            target.Buy(firstBuyDate, firstSharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, secondSharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(3, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingUnevenValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double firstSharesBought = 9;
            const double secondSharesBought = 1;

            target.Buy(firstBuyDate, firstSharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, secondSharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            var expected1 = new Holding
                                {
                                    Ticker = ticker,
                                    Head = secondBuyDate,
                                    Tail = secondSellDate,
                                    Shares = 1,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };
            var expected2 = new Holding
                                {
                                    Ticker = ticker,
                                    Head = firstBuyDate,
                                    Tail = secondSellDate,
                                    Shares = 4,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };
            var expected3 = new Holding
                                {
                                    Ticker = ticker,
                                    Head = firstBuyDate,
                                    Tail = firstSellDate,
                                    Shares = 5,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };


            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
            Assert.IsTrue(holdings.Contains(expected3));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingUnevenSortOrderCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double firstSharesBought = 9;
            const double secondSharesBought = 1;

            target.Buy(firstBuyDate, firstSharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, secondSharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(3, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingUnevenSortOrderValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double firstSharesBought = 9;
            const double secondSharesBought = 1;

            target.Buy(firstBuyDate, firstSharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, secondSharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            var expected1 = new Holding
                                {
                                    Ticker = ticker,
                                    Head = secondBuyDate,
                                    Tail = secondSellDate,
                                    Shares = 1,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };
            var expected2 = new Holding
                                {
                                    Ticker = ticker,
                                    Head = firstBuyDate,
                                    Tail = secondSellDate,
                                    Shares = 4,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };
            var expected3 = new Holding
                                {
                                    Ticker = ticker,
                                    Head = firstBuyDate,
                                    Tail = firstSellDate,
                                    Shares = 5,
                                    OpenPrice = buyPrice,
                                    OpenCommission = commission,
                                    ClosePrice = sellPrice,
                                    CloseCommission = commission
                                };
            var holding1 = holdings[0];
            var holding2 = holdings[1];
            var holding3 = holdings[2];
            Assert.AreEqual(expected1, holding1);
            Assert.AreEqual(expected2, holding2);
            Assert.AreEqual(expected3, holding3);
        }
    }
}