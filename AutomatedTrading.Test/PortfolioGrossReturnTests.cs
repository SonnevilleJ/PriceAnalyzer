using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class PortfolioGrossReturnTests
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
        public void CalculateGrossReturnOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = _portfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            Assert.IsNull(_securityBasketCalculator.CalculateGrossReturn(target, dateTime));
        }

        [Test]
        public void CalculateGrossReturnAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var deposit = _transactionFactory.ConstructDeposit(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            var withdrawal = _transactionFactory.ConstructWithdrawal(withdrawalDate, amount);

            var target = _portfolioFactory.ConstructPortfolio(deposit, withdrawal);

            Assert.IsNull(_securityBasketCalculator.CalculateGrossReturn(target, dateTime));
        }

        [Test]
        public void CalculateGrossReturnOpenPosition()
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

            // CalculateGrossReturn does not consider open positions - it can only account for closed holdings
            Assert.IsNull(_securityBasketCalculator.CalculateGrossReturn(target, calculateDate));
        }

        [Test]
        public void CalculateGrossReturnAfterGain()
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

            var expected = _securityBasketCalculator.CalculateGrossReturn(target.GetPosition(ticker), calculateDate);
            var actual = _securityBasketCalculator.CalculateGrossReturn(target, calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateGrossReturnTwoGain()
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

            var deReturn = _securityBasketCalculator.CalculateGrossReturn(target.GetPosition(de), sellDate);
            var msftReturn = _securityBasketCalculator.CalculateGrossReturn(target.GetPosition(msft), sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = _securityBasketCalculator.CalculateGrossReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateGrossReturnTwoLoss()
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

            var deReturn = _securityBasketCalculator.CalculateGrossReturn(target.GetPosition(de), sellDate);
            var msftReturn = _securityBasketCalculator.CalculateGrossReturn(target.GetPosition(msft), sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = _securityBasketCalculator.CalculateGrossReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateGrossReturnOneGainOneLoss()
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

            var deReturn = _securityBasketCalculator.CalculateGrossReturn(target.GetPosition(de), sellDate);
            var msftReturn = _securityBasketCalculator.CalculateGrossReturn(target.GetPosition(msft), sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = _securityBasketCalculator.CalculateGrossReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateGrossReturnOneGainOneOpen()
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

            var deReturn = _securityBasketCalculator.CalculateGrossReturn(target.GetPosition(de), sellDate);

            var expected = deReturn;
            var actual = _securityBasketCalculator.CalculateGrossReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateGrossReturnOneLossOneOpen()
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

            var msftReturn = _securityBasketCalculator.CalculateGrossReturn(target.GetPosition(msft), sellDate);

            var expected = msftReturn;
            var actual = _securityBasketCalculator.CalculateGrossReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }
    }
}