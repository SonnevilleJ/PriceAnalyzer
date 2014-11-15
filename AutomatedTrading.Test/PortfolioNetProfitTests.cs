using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class PortfolioNetProfitTests
    {
        private IPortfolioFactory _portfolioFactory;
        private ITransactionFactory _transactionFactory;
        private ISecurityBasketCalculator _securityBasketCalculator;

        [SetUp]
        public void Setup()
        {
            _securityBasketCalculator = new SecurityBasketCalculator();
            _portfolioFactory = new PortfolioFactory(new TransactionFactory(), new CashAccountFactory(), _securityBasketCalculator, new PositionFactory(new PriceSeriesFactory(), _securityBasketCalculator));
            _transactionFactory = new TransactionFactory();
        }

        [Test]
        public void CalculateNetProfitOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = _portfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            const decimal expectedValue = 0.00m;
            var actualValue = _securityBasketCalculator.CalculateNetProfit(target, dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void CalculateNetProfitAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var deposit = _transactionFactory.ConstructDeposit(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            var withdrawal = _transactionFactory.ConstructWithdrawal(withdrawalDate, amount);

            var target = _portfolioFactory.ConstructPortfolio(deposit, withdrawal);

            const decimal expectedValue = 0;
            var actualValue = _securityBasketCalculator.CalculateNetProfit(target, withdrawalDate);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void CalculateNetProfitOpenPosition()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            
            var buyDate = dateTime.AddDays(1);
            var calculateDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const int shares = 5;
            const decimal commission = 7.95m;

            var buy = _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, openingDeposit, buy);

            // CalculateNetProfit does not consider open positions - it can only account for closed holdings
            const decimal expected = 0;
            var actual = _securityBasketCalculator.CalculateNetProfit(target, calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateNetProfitAfterGain()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
             
            var buyDate = dateTime.AddDays(1);
            var sellDate = buyDate.AddDays(1);
            var calculateDate = sellDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const decimal sellPrice = 75.00m;
            const int shares = 5;
            const decimal commission = 7.95m;

            var buy = _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission);
            var sell = _transactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, openingDeposit, buy, sell);

            var expected = _securityBasketCalculator.CalculateNetProfit(target.GetPosition(ticker), calculateDate);
            var actual = _securityBasketCalculator.CalculateNetProfit(target, calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateNetProfitTwoGain()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const string msft = "MSFT";
            const decimal dePriceBought = 100.00m;
            const decimal dePriceSold = 110.00m;
            const decimal msftPriceBought = 50.00m;
            const decimal msftPriceSold = 60.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;

            var deBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = _transactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = _transactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, deBuy, deSell, msftBuy, msftSell);

            var deProfit = _securityBasketCalculator.CalculateNetProfit(target.GetPosition(de), sellDate);
            var msftProfit = _securityBasketCalculator.CalculateNetProfit(target.GetPosition(msft), sellDate);

            var expected = deProfit + msftProfit;
            var actual = _securityBasketCalculator.CalculateNetProfit(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateNetProfitTwoLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const string msft = "MSFT";
            const decimal dePriceBought = 100.00m;
            const decimal dePriceSold = 00.00m;
            const decimal msftPriceBought = 50.00m;
            const decimal msftPriceSold = 40.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;

            var deBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = _transactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = _transactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, deBuy, deSell, msftBuy, msftSell);

            var deProfit = _securityBasketCalculator.CalculateNetProfit(target.GetPosition(de), sellDate);
            var msftProfit = _securityBasketCalculator.CalculateNetProfit(target.GetPosition(msft), sellDate);

            var expected = deProfit + msftProfit;
            var actual = _securityBasketCalculator.CalculateNetProfit(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateNetProfitOneGainOneLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const string msft = "MSFT";
            const decimal dePriceBought = 100.00m;
            const decimal dePriceSold = 110.00m;
            const decimal msftPriceBought = 50.00m;
            const decimal msftPriceSold = 40.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;

            var deBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = _transactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = _transactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, deBuy, deSell, msftBuy, msftSell);

            var deProfit = _securityBasketCalculator.CalculateNetProfit(target.GetPosition(de), sellDate);
            var msftProfit = _securityBasketCalculator.CalculateNetProfit(target.GetPosition(msft), sellDate);

            var expected = deProfit + msftProfit;
            var actual = _securityBasketCalculator.CalculateNetProfit(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateNetProfitOneGainOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const string msft = "MSFT";
            const decimal dePriceBought = 100.00m;
            const decimal dePriceSold = 110.00m;
            const decimal msftPriceBought = 50.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;

            var deBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = _transactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, deBuy, deSell, msftBuy);

            var deProfit = _securityBasketCalculator.CalculateNetProfit(target.GetPosition(de), sellDate);
            var msftProfit = _securityBasketCalculator.CalculateNetProfit(target.GetPosition(msft), sellDate);

            var expected = deProfit + msftProfit;
            var actual = _securityBasketCalculator.CalculateNetProfit(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateNetProfitOneLossOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const string msft = "MSFT";
            const decimal dePriceBought = 100.00m;
            const decimal msftPriceBought = 50.00m;
            const decimal msftPriceSold = 40.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var msftBuy = _transactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = _transactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, deBuy, msftBuy, msftSell);

            var deProfit = _securityBasketCalculator.CalculateNetProfit(target.GetPosition(de), sellDate);
            var msftProfit = _securityBasketCalculator.CalculateNetProfit(target.GetPosition(msft), sellDate);

            var expected = deProfit + msftProfit;
            var actual = _securityBasketCalculator.CalculateNetProfit(target, sellDate);
            Assert.AreEqual(expected, actual);
        }
    }
}