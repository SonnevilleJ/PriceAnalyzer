using System;
using System.Collections.Generic;
using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class PortfolioCalculationTests
    {
        #region Annual Net Return
        
        // Annualized return calculations are based on Head and Tail
        // Comparing results from these methods on different SecurityBasket requires the Head and Tail to be the same
        
        [TestMethod]
        public void CalculateAnnualNetReturnOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            Assert.IsNull(target.CalculateAnnualNetReturn(dateTime));
        }

        [TestMethod]
        public void CalculateAnnualNetReturnAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(withdrawalDate, amount);

            Assert.IsNull(target.CalculateAnnualNetReturn(dateTime));
        }

        [TestMethod]
        public void CalculateAnnualNetReturnOpenPosition()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime.AddDays(1);
            var calculateDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const int shares = 5;
            const decimal commission = 7.95m;

            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission);
            target.AddTransaction(buy);

            Assert.IsNull(target.CalculateAnnualNetReturn(calculateDate));
        }

        [TestMethod]
        public void CalculateAnnualNetReturnAfterGain()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime;
            var sellDate = buyDate.AddDays(1);
            var calculateDate = sellDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const decimal sellPrice = 75.00m;
            const int shares = 5;
            const decimal commission = 7.95m;

            target.AddTransaction(TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            var expected = target.GetPosition(ticker).CalculateAnnualNetReturn(calculateDate);
            var actual = target.CalculateAnnualNetReturn(calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnTwoGain()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deReturn = target.GetPosition(de).CalculateAnnualNetReturn(sellDate);
            var msftReturn = target.GetPosition(msft).CalculateAnnualNetReturn(sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = target.CalculateAnnualNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnTwoLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deReturn = target.GetPosition(de).CalculateAnnualNetReturn(sellDate);
            var msftReturn = target.GetPosition(msft).CalculateAnnualNetReturn(sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = target.CalculateAnnualNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnOneGainOneLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deReturn = target.GetPosition(de).CalculateAnnualNetReturn(sellDate);
            var msftReturn = target.GetPosition(msft).CalculateAnnualNetReturn(sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = target.CalculateAnnualNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnOneGainOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);

            var deReturn = target.GetPosition(de).CalculateAnnualNetReturn(sellDate);

            var expected = deReturn;
            var actual = target.CalculateAnnualNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnOneLossOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var msftReturn = target.GetPosition(msft).CalculateAnnualNetReturn(sellDate);

            var expected = msftReturn;
            var actual = target.CalculateAnnualNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Annual Gross Return

        // Annualized return calculations are based on Head and Tail
        // Comparing results from these methods on different SecurityBasket requires the Head and Tail to be the same

        [TestMethod]
        public void CalculateAnnualGrossReturnOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            Assert.IsNull(target.CalculateAnnualGrossReturn(dateTime));
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(withdrawalDate, amount);

            Assert.IsNull(target.CalculateAnnualGrossReturn(dateTime));
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnOpenPosition()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime.AddDays(1);
            var calculateDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const int shares = 5;
            const decimal commission = 7.95m;

            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission);
            target.AddTransaction(buy);

            // CalculateAnnualGrossReturn does not consider open positions - it can only account for closed holdings
            Assert.IsNull(target.CalculateAnnualGrossReturn(calculateDate));
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnAfterGain()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime;
            var sellDate = buyDate.AddDays(1);
            var calculateDate = sellDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const decimal sellPrice = 75.00m;
            const int shares = 5;
            const decimal commission = 7.95m;

            target.AddTransaction(TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            var expected = target.GetPosition(ticker).CalculateAnnualGrossReturn(calculateDate);
            var actual = target.CalculateAnnualGrossReturn(calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnTwoGain()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deReturn = target.GetPosition(de).CalculateAnnualGrossReturn(sellDate);
            var msftReturn = target.GetPosition(msft).CalculateAnnualGrossReturn(sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = target.CalculateAnnualGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnTwoLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deReturn = target.GetPosition(de).CalculateAnnualGrossReturn(sellDate);
            var msftReturn = target.GetPosition(msft).CalculateAnnualGrossReturn(sellDate);

            var expected = ((deReturn * 8) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = target.CalculateAnnualGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnOneGainOneLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deReturn = target.GetPosition(de).CalculateAnnualGrossReturn(sellDate);
            var msftReturn = target.GetPosition(msft).CalculateAnnualGrossReturn(sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = target.CalculateAnnualGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnOneGainOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);

            var deReturn = target.GetPosition(de).CalculateAnnualGrossReturn(sellDate);

            var expected = deReturn;
            var actual = target.CalculateAnnualGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnOneLossOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var msftReturn = target.GetPosition(msft).CalculateAnnualGrossReturn(sellDate);

            var expected = msftReturn;
            var actual = target.CalculateAnnualGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Net Return

        [TestMethod]
        public void CalculateNetReturnOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            Assert.IsNull(target.CalculateNetReturn(dateTime));
        }

        [TestMethod]
        public void CalculateNetReturnAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(withdrawalDate, amount);

            Assert.IsNull(target.CalculateNetReturn(dateTime));
        }

        [TestMethod]
        public void CalculateNetReturnOpenPosition()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime.AddDays(1);
            var calculateDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const int shares = 5;
            const decimal commission = 7.95m;

            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission);
            target.AddTransaction(buy);

            Assert.IsNull(target.CalculateNetReturn(calculateDate));
        }

        [TestMethod]
        public void CalculateNetReturnAfterGain()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime.AddDays(1);
            var sellDate = buyDate.AddDays(1);
            var calculateDate = sellDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const decimal sellPrice = 75.00m;
            const int shares = 5;
            const decimal commission = 7.95m;
            
            target.AddTransaction(TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            var expected = target.GetPosition(ticker).CalculateNetReturn(calculateDate);
            var actual = target.CalculateNetReturn(calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetReturnTwoGain()
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
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deReturn = target.GetPosition(de).CalculateNetReturn(sellDate);
            var msftReturn = target.GetPosition(msft).CalculateNetReturn(sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetReturnTwoLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deReturn = target.GetPosition(de).CalculateNetReturn(sellDate);
            var msftReturn = target.GetPosition(msft).CalculateNetReturn(sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetReturnOneGainOneLoss()
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
            const decimal msftPriceSold = 40.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deReturn = target.GetPosition(de).CalculateNetReturn(sellDate);
            var msftReturn = target.GetPosition(msft).CalculateNetReturn(sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetReturnOneGainOneOpen()
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
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);

            var deReturn = target.GetPosition(de).CalculateNetReturn(sellDate);

            var expected = deReturn;
            var actual = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetReturnOneLossOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var msftReturn = target.GetPosition(msft).CalculateNetReturn(sellDate);

            var expected = msftReturn;
            var actual = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }
        
        #endregion

        #region Gross Return

        [TestMethod]
        public void CalculateGrossReturnOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            Assert.IsNull(target.CalculateGrossReturn(dateTime));
        }

        [TestMethod]
        public void CalculateGrossReturnAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(withdrawalDate, amount);

            Assert.IsNull(target.CalculateGrossReturn(dateTime));
        }

        [TestMethod]
        public void CalculateGrossReturnOpenPosition()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime.AddDays(1);
            var calculateDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const int shares = 5;
            const decimal commission = 7.95m;

            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission);
            target.AddTransaction(buy);

            // CalculateGrossReturn does not consider open positions - it can only account for closed holdings
            Assert.IsNull(target.CalculateGrossReturn(calculateDate));
        }

        [TestMethod]
        public void CalculateGrossReturnAfterGain()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime.AddDays(1);
            var sellDate = buyDate.AddDays(1);
            var calculateDate = sellDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const decimal sellPrice = 75.00m;
            const int shares = 5;
            const decimal commission = 7.95m;
            
            target.AddTransaction(TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            var expected = target.GetPosition(ticker).CalculateGrossReturn(calculateDate);
            var actual = target.CalculateGrossReturn(calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossReturnTwoGain()
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
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deReturn = target.GetPosition(de).CalculateGrossReturn(sellDate);
            var msftReturn = target.GetPosition(msft).CalculateGrossReturn(sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossReturnTwoLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deReturn = target.GetPosition(de).CalculateGrossReturn(sellDate);
            var msftReturn = target.GetPosition(msft).CalculateGrossReturn(sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossReturnOneGainOneLoss()
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
            const decimal msftPriceSold = 40.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deReturn = target.GetPosition(de).CalculateGrossReturn(sellDate);
            var msftReturn = target.GetPosition(msft).CalculateGrossReturn(sellDate);

            var expected = ((deReturn * sharesSold) + (msftReturn * sharesSold)) / (sharesSold * 2);
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossReturnOneGainOneOpen()
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
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);

            var deReturn = target.GetPosition(de).CalculateGrossReturn(sellDate);

            var expected = deReturn;
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossReturnOneLossOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var msftReturn = target.GetPosition(msft).CalculateGrossReturn(sellDate);

            var expected = msftReturn;
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }
        
        #endregion

        #region Net Profit

        [TestMethod]
        public void CalculateNetProfitOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            const decimal expectedValue = 0.00m;
            var actualValue = target.CalculateNetProfit(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void CalculateNetProfitAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(withdrawalDate, amount);

            const decimal expectedValue = 0;
            var actualValue = target.CalculateNetProfit(withdrawalDate);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void CalculateNetProfitOpenPosition()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime.AddDays(1);
            var calculateDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const int shares = 5;
            const decimal commission = 7.95m;

            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission);
            target.AddTransaction(buy);

            // CalculateNetProfit does not consider open positions - it can only account for closed holdings
            const decimal expected = 0;
            var actual = target.CalculateNetProfit(calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitAfterGain()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime.AddDays(1);
            var sellDate = buyDate.AddDays(1);
            var calculateDate = sellDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const decimal sellPrice = 75.00m;
            const int shares = 5;
            const decimal commission = 7.95m;
            
            target.AddTransaction(TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            var expected = target.GetPosition(ticker).CalculateNetProfit(calculateDate);
            var actual = target.CalculateNetProfit(calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitTwoGain()
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
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deProfit = target.GetPosition(de).CalculateNetProfit(sellDate);
            var msftProfit = target.GetPosition(msft).CalculateNetProfit(sellDate);

            var expected = deProfit + msftProfit;
            var actual = target.CalculateNetProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitTwoLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deProfit = target.GetPosition(de).CalculateNetProfit(sellDate);
            var msftProfit = target.GetPosition(msft).CalculateNetProfit(sellDate);

            var expected = deProfit + msftProfit;
            var actual = target.CalculateNetProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitOneGainOneLoss()
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
            const decimal msftPriceSold = 40.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deProfit = target.GetPosition(de).CalculateNetProfit(sellDate);
            var msftProfit = target.GetPosition(msft).CalculateNetProfit(sellDate);

            var expected = deProfit + msftProfit;
            var actual = target.CalculateNetProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitOneGainOneOpen()
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
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);

            var deProfit = target.GetPosition(de).CalculateNetProfit(sellDate);
            var msftProfit = target.GetPosition(msft).CalculateNetProfit(sellDate);

            var expected = deProfit + msftProfit;
            var actual = target.CalculateNetProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitOneLossOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deProfit = target.GetPosition(de).CalculateNetProfit(sellDate);
            var msftProfit = target.GetPosition(msft).CalculateNetProfit(sellDate);

            var expected = deProfit + msftProfit;
            var actual = target.CalculateNetProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }
        
        #endregion

        #region Gross Profit

        [TestMethod]
        public void CalculateGrossProfitOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            const decimal expectedValue = 0.00m;
            var actualValue = target.CalculateGrossProfit(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void CalculateGrossProfitAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(withdrawalDate, amount);

            const decimal expectedValue = 0;
            var actualValue = target.CalculateGrossProfit(withdrawalDate);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void CalculateGrossProfitOpenPosition()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime.AddDays(1);
            var calculateDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const int shares = 5;
            const decimal commission = 7.95m;

            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission);
            target.AddTransaction(buy);

            // CalculateGrossProfit does not consider open positions - it can only account for closed holdings
            const decimal expected = 0;
            var actual = target.CalculateGrossProfit(calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitAfterGain()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime.AddDays(1);
            var sellDate = buyDate.AddDays(1);
            var calculateDate = sellDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const decimal sellPrice = 75.00m;
            const int shares = 5;
            const decimal commission = 7.95m;
            
            target.AddTransaction(TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            var expected = target.GetPosition(ticker).CalculateGrossProfit(calculateDate);
            var actual = target.CalculateGrossProfit(calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitTwoGain()
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
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deProfit = target.GetPosition(de).CalculateGrossProfit(sellDate);
            var msftProfit = target.GetPosition(msft).CalculateGrossProfit(sellDate);

            var expected = deProfit + msftProfit;
            var actual = target.CalculateGrossProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitTwoLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deProfit = target.GetPosition(de).CalculateGrossProfit(sellDate);
            var msftProfit = target.GetPosition(msft).CalculateGrossProfit(sellDate);

            var expected = deProfit + msftProfit;
            var actual = target.CalculateGrossProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitOneGainOneLoss()
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
            const decimal msftPriceSold = 40.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deProfit = target.GetPosition(de).CalculateGrossProfit(sellDate);
            var msftProfit = target.GetPosition(msft).CalculateGrossProfit(sellDate);

            var expected = deProfit + msftProfit;
            var actual = target.CalculateGrossProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitOneGainOneOpen()
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
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);

            var deProfit = target.GetPosition(de).CalculateGrossProfit(sellDate);
            var msftProfit = target.GetPosition(msft).CalculateGrossProfit(sellDate);

            var expected = deProfit + msftProfit;
            var actual = target.CalculateGrossProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitOneLossOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deProfit = target.GetPosition(de).CalculateGrossProfit(sellDate);
            var msftProfit = target.GetPosition(msft).CalculateGrossProfit(sellDate);

            var expected = deProfit + msftProfit;
            var actual = target.CalculateGrossProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }
        
        #endregion

        #region Average Profit

        [TestMethod]
        public void CalculateAverageProfitOneGainOneLoss()
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
            const decimal msftPriceSold = 40.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var deProfit = target.GetPosition(de).CalculateAverageProfit(sellDate);
            var msftProfit = target.GetPosition(msft).CalculateAverageProfit(sellDate);

            var expected = deProfit + msftProfit;
            var actual = target.CalculateAverageProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Median Profit

        [TestMethod]
        public void CalculateMedianProfitOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(dateTime));
            var actual = target.CalculateMedianProfit(dateTime);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(withdrawalDate, amount);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(withdrawalDate));
            var actual = target.CalculateMedianProfit(withdrawalDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitOpenPosition()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime.AddDays(1);
            var calculateDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const int shares = 5;
            const decimal commission = 7.95m;

            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission);
            target.AddTransaction(buy);

            // CalculateMedianProfit does not consider open positions - it can only account for closed holdings
            var expected = GetExpectedMedianProfit(target.CalculateHoldings(calculateDate));
            var actual = target.CalculateMedianProfit(calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitAfterGain()
        {
            var dateTime = new DateTime(2011, 11, 21);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            var buyDate = dateTime.AddDays(1);
            var sellDate = buyDate.AddDays(1);
            var calculateDate = sellDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const decimal sellPrice = 75.00m;
            const int shares = 5;
            const decimal commission = 7.95m;
            
            target.AddTransaction(TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(sellDate));
            var actual = target.CalculateMedianProfit(calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitTwoGain()
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
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(sellDate));
            var actual = target.CalculateMedianProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitTwoLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(sellDate));
            var actual = target.CalculateMedianProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitOneGainOneLoss()
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
            const decimal msftPriceSold = 40.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(sellDate));
            var actual = target.CalculateMedianProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitOneGainOneOpen()
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
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(sellDate));
            var actual = target.CalculateMedianProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitOneLossOneOpen()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

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
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(sellDate));
            var actual = target.CalculateMedianProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Calculates the expected result of a call to CalculateMedianProfit on a single Position.
        /// </summary>
        /// <param name="holdings"></param>
        /// <returns></returns>
        private static decimal GetExpectedMedianProfit(IList<IHolding> holdings)
        {
            if (holdings.Count == 0) return 0.00m;

            var midpoint = (holdings.Count / 2);
            if (holdings.Count % 2 == 0)
            {
                return (holdings[midpoint - 1].GrossProfit() + holdings[midpoint].GrossProfit()) / 2;
            }
            return holdings[midpoint].GrossProfit();
        }

        #endregion
    }
}