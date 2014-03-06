using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestClass]
    public class PortfolioAnnualNetReturnTests
    {
        private IPortfolioFactory _portfolioFactory;
        private ITransactionFactory _transactionFactory;
        private ISecurityBasketCalculator _securityBasketCalculator;

        [TestInitialize]
        public void Initialize()
        {
            _portfolioFactory = new PortfolioFactory();
            _transactionFactory = new TransactionFactory();
            _securityBasketCalculator = new SecurityBasketCalculator();
        }

        // Annualized return calculations are based on Head and Tail
        // Comparing results from these methods on different SecurityBasket requires the Head and Tail to be the same

        [TestMethod]
        public void CalculateAnnualNetReturnOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = _portfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            Assert.IsNull(_securityBasketCalculator.CalculateAnnualNetReturn(target, dateTime));
        }

        [TestMethod]
        public void CalculateAnnualNetReturnAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var deposit = _transactionFactory.ConstructDeposit(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            var withdrawal = _transactionFactory.ConstructWithdrawal(withdrawalDate, amount);

            var target = _portfolioFactory.ConstructPortfolio(deposit, withdrawal);

            Assert.IsNull(_securityBasketCalculator.CalculateAnnualNetReturn(target, dateTime));
        }

        [TestMethod]
        public void CalculateAnnualNetReturnOpenPosition()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;

            var buyDate = dateTime.AddDays(1);
            var calculateDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const int shares = 5;
            const decimal commission = 7.95m;

            var target = _portfolioFactory.ConstructPortfolio(ticker,
                                                              _transactionFactory.ConstructDeposit(dateTime,
                                                                                                   openingDeposit),
                                                              _transactionFactory.ConstructBuy(ticker, buyDate, shares,
                                                                                               buyPrice, commission));

            Assert.IsNull(_securityBasketCalculator.CalculateAnnualNetReturn(target, calculateDate));
        }

        [TestMethod]
        public void CalculateAnnualNetReturnAfterGain()
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

            var target = _portfolioFactory.ConstructPortfolio(ticker,
                                                              _transactionFactory.ConstructDeposit(dateTime,
                                                                                                   openingDeposit),
                                                              _transactionFactory.ConstructBuy(ticker, buyDate, shares,
                                                                                               buyPrice, commission),
                                                              _transactionFactory.ConstructSell(ticker, sellDate, shares,
                                                                                                sellPrice, commission));

            var expected = _securityBasketCalculator.CalculateAnnualNetReturn(target.GetPosition(ticker), calculateDate);
            var actual = _securityBasketCalculator.CalculateAnnualNetReturn(target, calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnTwoGain()
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

            var deReturn = _securityBasketCalculator.CalculateAnnualNetReturn(target.GetPosition(de), sellDate);
            var msftReturn = _securityBasketCalculator.CalculateAnnualNetReturn(target.GetPosition(msft), sellDate);

            var expected = ((deReturn*sharesSold) + (msftReturn*sharesSold))/(sharesSold*2);
            var actual = _securityBasketCalculator.CalculateAnnualNetReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnTwoLoss()
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

            var deReturn = _securityBasketCalculator.CalculateAnnualNetReturn(target.GetPosition(de), sellDate);
            var msftReturn = _securityBasketCalculator.CalculateAnnualNetReturn(target.GetPosition(msft), sellDate);

            var expected = ((deReturn*sharesSold) + (msftReturn*sharesSold))/(sharesSold*2);
            var actual = _securityBasketCalculator.CalculateAnnualNetReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnOneGainOneLoss()
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

            var deReturn = _securityBasketCalculator.CalculateAnnualNetReturn(target.GetPosition(de), sellDate);
            var msftReturn = _securityBasketCalculator.CalculateAnnualNetReturn(target.GetPosition(msft), sellDate);

            var expected = ((deReturn*sharesSold) + (msftReturn*sharesSold))/(sharesSold*2);
            var actual = _securityBasketCalculator.CalculateAnnualNetReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnOneGainOneOpen()
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

            var deReturn = _securityBasketCalculator.CalculateAnnualNetReturn(target.GetPosition(de), sellDate);

            var expected = deReturn;
            var actual = _securityBasketCalculator.CalculateAnnualNetReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnOneLossOneOpen()
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

            var msftReturn = _securityBasketCalculator.CalculateAnnualNetReturn(target.GetPosition(msft), sellDate);

            var expected = msftReturn;
            var actual = _securityBasketCalculator.CalculateAnnualNetReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }
    }
}