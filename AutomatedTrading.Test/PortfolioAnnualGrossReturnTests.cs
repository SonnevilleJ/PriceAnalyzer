using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class PortfolioAnnualGrossReturnTests
    {
        private IPortfolioFactory _portfolioFactory;
        private ITransactionFactory _transactionFactory;
        private ISecurityBasketCalculator _securityBasketCalculator;

        [SetUp]
        public void Setup()
        {
            _securityBasketCalculator = new SecurityBasketCalculator();
            _portfolioFactory = new PortfolioFactory(new TransactionFactory(), new CashAccountFactory(), _securityBasketCalculator, new PositionFactory(new PriceSeriesFactory(), _securityBasketCalculator), new PriceSeriesFactory());
            _transactionFactory = new TransactionFactory();
        }

        // Annualized return calculations are based on Head and Tail
        // Comparing results from these methods on different SecurityBasket requires the Head and Tail to be the same

        [Test]
        public void CalculateAnnualGrossReturnOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = _portfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            Assert.IsNull(_securityBasketCalculator.CalculateAnnualGrossReturn(target, dateTime));
        }

        [Test]
        public void CalculateAnnualGrossReturnAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var deposit = _transactionFactory.ConstructDeposit(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            var withdrawal = _transactionFactory.ConstructWithdrawal(withdrawalDate, amount);

            var target = _portfolioFactory.ConstructPortfolio(deposit, withdrawal);

            Assert.IsNull(_securityBasketCalculator.CalculateAnnualGrossReturn(target, dateTime));
        }

        [Test]
        public void CalculateAnnualGrossReturnOpenPosition()
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

            // CalculateAnnualGrossReturn does not consider open positions - it can only account for closed holdings
            Assert.IsNull(_securityBasketCalculator.CalculateAnnualGrossReturn(target, calculateDate));
        }

        [Test]
        public void CalculateAnnualGrossReturnAfterGain()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            
            var buyDate = dateTime;
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

            var expected = _securityBasketCalculator.CalculateAnnualGrossReturn(target.GetPosition(ticker), calculateDate);
            var actual = _securityBasketCalculator.CalculateAnnualGrossReturn(target, calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateAnnualGrossReturnTwoGain()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = dateTime;
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

            var deReturn = _securityBasketCalculator.CalculateAnnualGrossReturn(target.GetPosition(de), sellDate);
            var msftReturn = _securityBasketCalculator.CalculateAnnualGrossReturn(target.GetPosition(msft), sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = _securityBasketCalculator.CalculateAnnualGrossReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateAnnualGrossReturnTwoLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = dateTime;
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

            var deReturn = _securityBasketCalculator.CalculateAnnualGrossReturn(target.GetPosition(de), sellDate);
            var msftReturn = _securityBasketCalculator.CalculateAnnualGrossReturn(target.GetPosition(msft), sellDate);

            var expected = ((deReturn * 8) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = _securityBasketCalculator.CalculateAnnualGrossReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateAnnualGrossReturnOneGainOneLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = dateTime;
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

            var deReturn = _securityBasketCalculator.CalculateAnnualGrossReturn(target.GetPosition(de), sellDate);
            var msftReturn = _securityBasketCalculator.CalculateAnnualGrossReturn(target.GetPosition(msft), sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = _securityBasketCalculator.CalculateAnnualGrossReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateAnnualGrossReturnOneGainOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = dateTime;
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

            var deReturn = _securityBasketCalculator.CalculateAnnualGrossReturn(target.GetPosition(de), sellDate);

            var expected = deReturn;
            var actual = _securityBasketCalculator.CalculateAnnualGrossReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateAnnualGrossReturnOneLossOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = dateTime;
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

            var msftReturn = _securityBasketCalculator.CalculateAnnualGrossReturn(target.GetPosition(msft), sellDate);

            var expected = msftReturn;
            var actual = _securityBasketCalculator.CalculateAnnualGrossReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }
    }
}