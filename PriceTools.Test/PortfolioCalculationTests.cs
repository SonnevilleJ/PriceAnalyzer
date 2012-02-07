﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PortfolioCalculationTests
    {
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
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
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
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
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
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
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
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
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
        public void CalculateNetProfitOneLossOneProfit()
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
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
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
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
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
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
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
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
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
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
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
        public void CalculateGrossProfitOneLossOneProfit()
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
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
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

        [TestMethod]
        public void PortfolioCalculateGrossReturnTest()
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
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = TransactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = TransactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var grossProfit = target.CalculateGrossProfit(sellDate);

            const decimal deReturn = (dePriceSold / dePriceBought) - 1;
            var deProfit = target.GetPosition(de).CalculateGrossProfit(sellDate);
            var deWeight = deProfit / grossProfit;
            
            const decimal msftReturn = (msftPriceSold / msftPriceBought) - 1;
            var msftProfit = target.GetPosition(msft).CalculateGrossProfit(sellDate);
            var msftWeight = msftProfit / grossProfit;

            var expected = (deReturn*deWeight) + (msftReturn*msftWeight);
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        #region Holdings Tests

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

        #endregion
    }
}