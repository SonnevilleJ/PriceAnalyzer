using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class PositionHoldingTests
    {
        private IHoldingFactory _holdingFactory;
        private IPositionFactory _positionFactory;
        private ITransactionFactory _transactionFactory;

        [SetUp]
        public void Setup()
        {
            _holdingFactory = new HoldingFactory();
            _positionFactory = new PositionFactory(new PriceSeriesFactory(), new SecurityBasketCalculator());
            _transactionFactory = new TransactionFactory();
        }

        [Test]
        public void CalculateHoldingsTestWithOneBuyOneSellCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares

            var sellDate = buyDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, sellDate);

            Assert.AreEqual(1, holdings.Count);
        }

        [Test]
        public void CalculateHoldingsTestWithOneBuyOneSellValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares

            var sellDate = buyDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, sellDate);

            var expected = _holdingFactory.ConstructHolding(ticker, buyDate, sellDate, sharesSold, buyPrice, commission, sellPrice, commission);

            Assert.IsTrue(holdings.Contains(expected));
        }

        [Test]
        public void CalculateHoldingsTestWithTwoBuysOneSellCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 3;      // 3 shares

            var sellDate = secondBuyDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 6;        // 6 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, firstBuyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructBuy(ticker, secondBuyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, sellDate);

            Assert.AreEqual(2, holdings.Count);
        }

        [Test]
        public void CalculateHoldingsTestWithTwoBuysOneSellValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 3;      // 3 shares

            var sellDate = secondBuyDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 6;        // 6 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, firstBuyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructBuy(ticker, secondBuyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, sellDate);

            var expected1 = _holdingFactory.ConstructHolding(ticker, firstBuyDate, sellDate, sharesBought, buyPrice, commission, sellPrice, commission);
            var expected2 = _holdingFactory.ConstructHolding(ticker, secondBuyDate, sellDate, sharesBought, buyPrice, commission, sellPrice, commission);

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [Test]
        public void CalculateHoldingsTestWithOneBuyTwoSellsCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares

            var firstSellDate = buyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, sellPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            Assert.AreEqual(2, holdings.Count);
        }

        [Test]
        public void CalculateHoldingsTestWithOneBuyTwoSellsValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares

            var firstSellDate = buyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, sellPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            const decimal sharesInHolding = sharesSold;

            var expected1 = _holdingFactory.ConstructHolding(ticker, buyDate, secondSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);
            var expected2 = _holdingFactory.ConstructHolding(ticker, buyDate, firstSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [Test]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 5;      // 5 shares

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, firstBuyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructBuy(ticker, secondBuyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            Assert.AreEqual(2, holdings.Count);
        }

        [Test]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var firstBuyDate = new DateTime(2001, 1, 1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 5;      // 5 shares
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, firstBuyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, sellPrice, commission),
                                                           _transactionFactory.ConstructBuy(ticker, secondBuyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            var expected1 = _holdingFactory.ConstructHolding(ticker, firstBuyDate, firstSellDate, sharesSold, buyPrice, commission, sellPrice, commission);
            var expected2 = _holdingFactory.ConstructHolding(ticker, secondBuyDate, secondSellDate, sharesSold, buyPrice, commission, sellPrice, commission);

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [Test]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 5;      // 5 shares

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, firstBuyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructBuy(ticker, secondBuyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            Assert.AreEqual(2, holdings.Count);
        }

        [Test]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 5;      // 5 shares

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, firstBuyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructBuy(ticker, secondBuyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, sellPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            const decimal sharesInHolding = sharesSold;
            var expected1 = _holdingFactory.ConstructHolding(ticker, secondBuyDate, secondSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);
            var expected2 = _holdingFactory.ConstructHolding(ticker, firstBuyDate, firstSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [Test]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingUnevenCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal firstSharesBought = 9;
            const decimal secondSharesBought = 1;

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, firstBuyDate, firstSharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructBuy(ticker, secondBuyDate, secondSharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            Assert.AreEqual(3, holdings.Count);
        }

        [Test]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingUnevenValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal firstSharesBought = 9;
            const decimal secondSharesBought = 1;

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, firstBuyDate, firstSharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructBuy(ticker, secondBuyDate, secondSharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, sellPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            var expected1 = _holdingFactory.ConstructHolding(ticker, secondBuyDate, secondSellDate, 1, buyPrice, commission, sellPrice, commission);
            var expected2 = _holdingFactory.ConstructHolding(ticker, firstBuyDate, secondSellDate, 4, buyPrice, commission, sellPrice, commission);
            var expected3 = _holdingFactory.ConstructHolding(ticker, firstBuyDate, firstSellDate, 5, buyPrice, commission, sellPrice, commission);


            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
            Assert.IsTrue(holdings.Contains(expected3));
        }

        [Test]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingUnevenSortOrderCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal firstSharesBought = 9;
            const decimal secondSharesBought = 1;

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, firstBuyDate, firstSharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructBuy(ticker, secondBuyDate, secondSharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, sellPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            Assert.AreEqual(3, holdings.Count);
        }

        [Test]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingUnevenSortOrderValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal firstSharesBought = 9;
            const decimal secondSharesBought = 1;

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, firstBuyDate, firstSharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructBuy(ticker, secondBuyDate, secondSharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, sellPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission));

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            var expected1 = _holdingFactory.ConstructHolding(ticker, firstBuyDate, firstSellDate, 5, buyPrice, commission, sellPrice, commission);
            var expected2 = _holdingFactory.ConstructHolding(ticker, firstBuyDate, secondSellDate, 4, buyPrice, commission, sellPrice, commission);
            var expected3 = _holdingFactory.ConstructHolding(ticker, secondBuyDate, secondSellDate, 1, buyPrice, commission, sellPrice, commission);
            var holding1 = holdings[0];
            var holding2 = holdings[1];
            var holding3 = holdings[2];
            Assert.AreEqual(expected1, holding1);
            Assert.AreEqual(expected2, holding2);
            Assert.AreEqual(expected3, holding3);
        }
    }
}