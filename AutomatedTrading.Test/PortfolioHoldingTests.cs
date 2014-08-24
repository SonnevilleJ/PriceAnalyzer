using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class PortfolioHoldingTests
    {
        private IHoldingFactory _holdingFactory;
        private IPortfolioFactory _portfolioFactory;
        private ITransactionFactory _transactionFactory;

        [SetUp]
        public void Setup()
        {
            _holdingFactory = new HoldingFactory();
            _portfolioFactory = new PortfolioFactory();
            _transactionFactory = new TransactionFactory();
        }

        [Test]
        public void CalculateHoldingsTestWithOnePositionOneBuyOneSell()
        {
            var dateTime = new DateTime(2011, 7, 26);
            const decimal deposit = 10000m;
            
            var buyDate = new DateTime(2011, 7, 26);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const decimal shares = 2;
            var buy = _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice);

            var sellDate = new DateTime(2011, 9, 26);
            const decimal sellPrice = 75.00m;
            var sell = _transactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy, sell);

            var holdings = _holdingFactory.CalculateHoldings(target, sellDate);

            Assert.AreEqual(1, holdings.Count);
            var expected = _holdingFactory.ConstructHolding(ticker, buyDate, sellDate, shares, buyPrice, sellPrice);

            Assert.IsTrue(holdings.Contains(expected));
        }

        [Test]
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

            var firstBuy = _transactionFactory.ConstructBuy(ticker, firstBuyDate, sharesBought, buyPrice, commission);
            var secondBuy = _transactionFactory.ConstructBuy(ticker, secondBuyDate, sharesBought, buyPrice, commission);
            var firstSell = _transactionFactory.ConstructSell(ticker, firstSellDate, sharesSold, sellPrice, commission);
            var secondSell = _transactionFactory.ConstructSell(ticker, secondSellDate, sharesSold, sellPrice, commission);

            var target = _portfolioFactory.ConstructPortfolio(testDate, deposit, firstBuy, secondBuy, firstSell, secondSell);

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const decimal sharesInHolding = sharesSold;
            var expected1 = _holdingFactory.ConstructHolding(ticker, secondBuyDate, secondSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);
            var expected2 = _holdingFactory.ConstructHolding(ticker, firstBuyDate, firstSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [Test]
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

            var firstBuy = _transactionFactory.ConstructBuy(firstTicker, firstBuyDate, sharesBought, buyPrice, commission);
            var secondBuy = _transactionFactory.ConstructBuy(secondTicker, secondBuyDate, sharesBought, buyPrice, commission);
            var firstSell = _transactionFactory.ConstructSell(firstTicker, firstSellDate, sharesSold, sellPrice, commission);
            var secondSell = _transactionFactory.ConstructSell(secondTicker, secondSellDate, sharesSold, sellPrice, commission);

            var target = _portfolioFactory.ConstructPortfolio(testDate, deposit, firstBuy, secondBuy, firstSell, secondSell);

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const decimal sharesInHolding = sharesSold;
            var expected1 = _holdingFactory.ConstructHolding(secondTicker, secondBuyDate, secondSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);
            var expected2 = _holdingFactory.ConstructHolding(firstTicker, firstBuyDate, firstSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [Test]
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

            var firstBuy = _transactionFactory.ConstructBuy(firstTicker, firstBuyDate, sharesBought, buyPrice, commission);
            var secondBuy = _transactionFactory.ConstructBuy(secondTicker, secondBuyDate, sharesBought, buyPrice, commission);
            var firstSell = _transactionFactory.ConstructSell(firstTicker, firstSellDate, sharesSold, sellPrice, commission);
            var secondSell = _transactionFactory.ConstructSell(secondTicker, secondSellDate, sharesSold, sellPrice, commission);

            var target = _portfolioFactory.ConstructPortfolio(testDate, deposit, firstBuy, secondBuy, firstSell, secondSell);

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const decimal sharesInHolding = sharesSold;
            var expected1 = _holdingFactory.ConstructHolding(firstTicker, firstBuyDate, firstSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);
            var expected2 = _holdingFactory.ConstructHolding(secondTicker, secondBuyDate, secondSellDate, sharesInHolding, buyPrice, commission, sellPrice, commission);
            var holding1 = holdings[0];
            var holding2 = holdings[1];
            Assert.AreEqual(expected1, holding1);
            Assert.AreEqual(expected2, holding2);
        }
    }
}