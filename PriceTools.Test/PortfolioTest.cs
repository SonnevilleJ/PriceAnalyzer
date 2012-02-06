using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Yahoo;
using Sonneville.PriceTools.SamplePortfolioData;

namespace Sonneville.PriceTools.Test
{
    /// <summary>
    ///This is a test class for Portfolio and is intended
    ///to contain all Portfolio Unit Tests
    ///</summary>
    [TestClass]
    public class PortfolioTest
    {
        [TestMethod]
        public void ConstructorTest1()
        {
            var target = PortfolioFactory.ConstructPortfolio();

            Assert.AreEqual(string.Empty, target.CashTicker);
            Assert.AreEqual(0, target.GetAvailableCash(DateTime.Now));
            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void ConstructorTest2()
        {
            const string ticker = "FDRXX";  // Fidelity Cash Reserves
            var target = PortfolioFactory.ConstructPortfolio(ticker);

            Assert.AreEqual(ticker, target.CashTicker);
            Assert.AreEqual(0, target.GetAvailableCash(DateTime.Now));
            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void ConstructorTest3()
        {
            var openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            const string ticker = "FDRXX";  // Fidelity Cash Reserves
            var target = PortfolioFactory.ConstructPortfolio(openDate, amount, ticker);

            Assert.AreEqual(ticker, target.CashTicker);
            Assert.AreEqual(amount, target.GetAvailableCash(openDate));
            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void Constructor4Test()
        {
            var csvFile = PortfolioTransactionHistoryCsvFiles.FidelityTransactions;
            var ticker = String.Empty;

            var target = PortfolioFactory.ConstructPortfolio(csvFile);

            Assert.AreEqual(ticker, target.CashTicker);
        }

        [TestMethod]
        public void DepositTest1()
        {
            var openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio();

            target.Deposit(openDate, amount);

            Assert.AreEqual(amount, target.GetAvailableCash(openDate));
        }

        [TestMethod]
        public void DepositTest2()
        {
            var openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio();

            var deposit = TransactionFactory.ConstructDeposit(openDate, amount);
            target.Deposit(deposit);

            Assert.AreEqual(amount, target.GetAvailableCash(openDate));
        }

        [TestMethod]
        public void WithdrawalTest1()
        {
            var openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio();

            target.Deposit(openDate, amount);
            target.Withdraw(openDate, amount);

            Assert.AreEqual(0, target.GetAvailableCash(openDate));
        }

        [TestMethod]
        public void WithdrawalTest2()
        {
            var openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio();

            var deposit = TransactionFactory.ConstructDeposit(openDate, amount);
            var withdrawal = TransactionFactory.ConstructWithdrawal(openDate, amount);

            target.Deposit(deposit);
            target.Withdraw(withdrawal);

            Assert.AreEqual(0, target.GetAvailableCash(openDate));
        }

        [TestMethod]
        public void GetAvailableCashNoTransactions()
        {
            var target = PortfolioFactory.ConstructPortfolio();

            const decimal expectedCash = 0;
            var availableCash = target.GetAvailableCash(DateTime.Now);
            Assert.AreEqual(expectedCash, availableCash);
        }

        [TestMethod]
        public void CalculateGrossProfitNoTransactions()
        {
            var target = PortfolioFactory.ConstructPortfolio();

            const decimal expectedValue = 0;
            var actualValue = target.CalculateGrossProfit(DateTime.Now);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void GetAvailableCashOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            const decimal expectedCash = openingDeposit;
            var availableCash = target.GetAvailableCash(dateTime);
            Assert.AreEqual(expectedCash, availableCash);
        }

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
        public void GetAvailableCashAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(dateTime.AddDays(1), amount);

            const decimal expectedCash = 0;
            var availableCash = target.GetAvailableCash(withdrawalDate);
            Assert.AreEqual(expectedCash, availableCash);
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
        public void CalculateGrossProfitWithOpenPosition()
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
            const decimal buyValue = (shares * buyPrice);
            
            // Because CalculateGrossProfit cannot get price data, it must calculate based on the buy prices and any sell prices
            const decimal currentValue = buyPrice*shares;

            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, buyPrice, shares, commission);
            target.AddTransaction(buy);

            const decimal expected = 0 - buyValue;
            var actual = target.CalculateGrossProfit(calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitWithClosedPosition()
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
            const decimal buyValue = (shares*buyPrice);
            const decimal sellValue = (shares*sellPrice);

            target.AddTransaction(TransactionFactory.ConstructBuy(buyDate, ticker, buyPrice, shares, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(sellDate, ticker, sellPrice, shares, commission));

            const decimal expected = sellValue - buyValue;
            var actual = target.CalculateGrossProfit(calculateDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateRawReturnTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 7.95m;   // with $7.95 commission
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares, commission);

            target.AddTransaction(buy);

            Assert.IsNull(target.CalculateRawReturn(sellDate));
        }

        [TestMethod]
        public void CalculateRawReturnFromClosedPortfolioTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 7.95m;   // with $7.95 commission
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares, commission);
            var sell = TransactionFactory.ConstructSell(sellDate, ticker, price, shares, commission);

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = 0.0m;      // 0% raw return on investment
            var actual = target.CalculateRawReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateTotalReturnTest()
        {
            var dateTime = new DateTime(2001, 1, 1);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares, commission);

            target.AddTransaction(buy);

            Assert.IsNull(target.CalculateTotalReturn(sellDate));
        }

        [TestMethod]
        public void CalculateTotalReturnFromClosedPortfolioTest()
        {
            var dateTime = new DateTime(2001, 1, 1);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares, commission);
            var sell = TransactionFactory.ConstructSell(sellDate, ticker, price, shares, commission);

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = -0.02m;      // negative 2% return; 98% of original investment
            var actual = target.CalculateTotalReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetAverageAnnualReturnTest()
        {
            var dateTime = new DateTime(2001, 1, 1);
            const decimal deposit = 505.00m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const string ticker = "DE";
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, buyPrice, shares, commission);

            target.AddTransaction(buy);

            Assert.IsNull(target.CalculateAverageAnnualReturn(sellDate));
        }

        [TestMethod]
        public void GetAverageAnnualReturnFromClosedPortfolioTest()
        {
            var dateTime = new DateTime(2001, 1, 1);
            const decimal deposit = 505.00m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const string ticker = "DE";
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sellPrice = 112.00m;      // $112.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, buyPrice, shares, commission);
            var sell = TransactionFactory.ConstructSell(sellDate, ticker, sellPrice, shares, commission);

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expectedReturn = 0.1m;    // 10% return; profit = $50 after commissions; initial investment = $500
            var actualReturn = target.CalculateTotalReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = 0.5m;          // 50% annual rate return
            var actual = target.CalculateAverageAnnualReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetInvestedValueFromEmptyPortfolio()
        {
            var target = PortfolioFactory.ConstructPortfolio();
            Assert.AreEqual(0.0m, target.CalculateInvestedValue(new YahooPriceDataProvider(), DateTime.Now));
        }

        [TestMethod]
        public void GetInvestedValue()
        {
            var dateTime = new DateTime(2011, 4, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 4, 25);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            const decimal commission = 7.95m;
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares, commission);

            target.AddTransaction(buy);

            // If the price date falls within a period, CalculateInvestedValue will use price data from that period.
            // Because of this, I changed the price date to 11:59 pm rather than the next day (default of midnight).
            // Todo: adding a "market open/close times" feature would be a better fix; more convenient for the client.
            var priceDate = new DateTime(2011, 4, 25, 23, 59, 59);
            
            const decimal expected = 189.44m; // closing price 25 April 2011 = $94.72 * 2 shares = 189.44
            var actual = target.CalculateInvestedValue(new YahooPriceDataProvider(), priceDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetInvestedValueFromClosedPortfolio()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 4, 25);
            var sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            const decimal commission = 7.95m;
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares, commission);
            var sell = TransactionFactory.ConstructSell(sellDate, ticker, price, shares, commission);

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = 0.00m; // all shares sold = no value
            var actual = target.CalculateInvestedValue(new YahooPriceDataProvider(), sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCostFromEmptyPortfolio()
        {
            var target = PortfolioFactory.ConstructPortfolio();

            Assert.AreEqual(0.0m, target.CalculateCost(DateTime.Now));
        }

        [TestMethod]
        public void GetCost()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 4, 25);
            var sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            const decimal commission = 7.95m;
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares, commission);
            var sell = TransactionFactory.ConstructSell(sellDate, ticker, price, shares, commission);

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = 100.00m;
            var actual = target.CalculateCost(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetProceedsFromEmptyPortfolio()
        {
            var target = PortfolioFactory.ConstructPortfolio();
            Assert.AreEqual(0.0m, target.CalculateProceeds(DateTime.Now));
        }

        [TestMethod]
        public void GetProceeds()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 4, 25);
            var sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            const decimal commission = 7.95m;
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares, commission);
            var sell = TransactionFactory.ConstructSell(sellDate, ticker, price, shares, commission);

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = 100.00m;
            var actual = target.CalculateProceeds(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCommissionsFromEmptyPortfolio()
        {
            var target = PortfolioFactory.ConstructPortfolio();
            Assert.AreEqual(0.0m, target.CalculateCommissions(DateTime.Now));
        }

        [TestMethod]
        public void GetCommission()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 4, 25);
            var sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            const decimal commission = 7.95m;
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares, commission);
            var sell = TransactionFactory.ConstructSell(sellDate, ticker, price, shares, commission);

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = 15.90m; // two $7.95 commissions paid
            var actual = target.CalculateCommissions(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPositionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);
            
            const string ticker = "DE";
            var buy = TransactionFactory.ConstructBuy(dateTime, ticker, 0.00m, 5);
            
            target.AddTransaction(buy);

            var position = target.GetPosition(ticker);
            Assert.IsTrue(position.Transactions.Contains(buy));
        }

        [TestMethod]
        public void GetPositionTestMissing()
        {
            var target = PortfolioFactory.ConstructPortfolio("FTEXX");
            Assert.IsNull(target.GetPosition("ASDF"));
        }

        [TestMethod]
        public void IndexerTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, amount);

            var expectedValue = target.CalculateGrossProfit(dateTime);
            decimal? actualValue = target[dateTime];
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void AddTransactionDepositTest()
        {
            var date = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio();

            var deposit = TransactionFactory.ConstructDeposit(date, amount);
            target.AddTransaction(deposit);

            Assert.IsTrue(target.Transactions.Contains(deposit));
        }

        [TestMethod]
        public void AddTransactionWithdrawalTest()
        {
            var date = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio();

            var deposit = TransactionFactory.ConstructDeposit(date, amount);
            target.AddTransaction(deposit);

            Assert.IsTrue(target.Transactions.Contains(deposit));
        }

        [TestMethod]
        public void AddBuyTransactionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var date = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            var buy = TransactionFactory.ConstructBuy(date, ticker, price, shares);
            target.AddTransaction(buy);

            Assert.IsTrue(target.Transactions.Contains(buy));
        }

        [TestMethod]
        public void AddSellTransactionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares);
            var sell = TransactionFactory.ConstructSell(buyDate.AddDays(1), ticker, price, shares);

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            Assert.IsTrue(target.Transactions.Contains(buy)); 
            Assert.IsTrue(target.Transactions.Contains(sell));
        }

        [TestMethod]
        public void AddBuyToCoverTransactionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            var sellShort = TransactionFactory.ConstructSellShort(buyDate, ticker, price, shares);
            var buyToCover = TransactionFactory.ConstructBuyToCover(buyDate.AddDays(1), ticker, price, shares);

            target.AddTransaction(sellShort);
            target.AddTransaction(buyToCover);

            Assert.IsTrue(target.Transactions.Contains(sellShort));
            Assert.IsTrue(target.Transactions.Contains(buyToCover));
        }

        [TestMethod]
        public void AddSellShortTransactionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var date = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            var sellShort = TransactionFactory.ConstructSellShort(date, ticker, price, shares);
            target.AddTransaction(sellShort);

            Assert.IsTrue(target.Transactions.Contains(sellShort));
        }

        [TestMethod]
        public void AddDividendReceiptTransactionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares);
            var dividendReceipt = TransactionFactory.ConstructDividendReceipt(buyDate.AddDays(1), (decimal) shares);

            target.AddTransaction(buy);
            target.AddTransaction(dividendReceipt);

            Assert.IsTrue(target.Transactions.Contains(buy));
            Assert.IsTrue(target.Transactions.Contains(dividendReceipt));
        }

        [TestMethod]
        public void AddDividendReinvestmentTransactionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares);
            var dividendReinvestment = TransactionFactory.ConstructDividendReinvestment(buyDate.AddDays(1), ticker, 1, shares);

            target.AddTransaction(buy);
            target.AddTransaction(dividendReinvestment);

            Assert.IsTrue(target.Transactions.Contains(buy));
            Assert.IsTrue(target.Transactions.Contains(dividendReinvestment));
        }

        [TestMethod]
        public void TransactionsTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);   // first transaction: deposit

            var buyDate = new DateTime(2011, 1, 9);
            const string de = "DE";
            const string msft = "MSFT";
            const decimal price = 50.00m;
            const double shares = 2;

            // second transaction: implicit withdrawal from buy1
            // third transaction: buy1
            // fourth transaction: implicit withdrawal from buy2
            // fifth transaction: buy2
            var buy1 = TransactionFactory.ConstructBuy(buyDate, de, price, shares);
            var buy2 = TransactionFactory.ConstructBuy(buyDate, msft, price, shares);
            target.AddTransaction(buy1);
            target.AddTransaction(buy2);
            
            const int expectedTransactions = 5;
            var actualTransactions = target.Transactions.Count;
            Assert.AreEqual(expectedTransactions, actualTransactions);

            Assert.IsTrue(target.Transactions.Contains(buy1));
            Assert.IsTrue(target.Transactions.Contains(buy2));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WithdrawWithoutAvailableCash()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, amount);

            var buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, price, shares);
            target.AddTransaction(buy);

            var withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(withdrawalDate, amount);
        }

        [TestMethod]
        public void HeadTestWhenEmpty()
        {
            var target = PortfolioFactory.ConstructPortfolio();

            // measure DayOfYear because ticks will be slightly different between calls
            var expected = DateTime.Now.DayOfYear;
            var actual = target.Head.DayOfYear;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadTestWithPosition()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio();

            target.Deposit(dateTime, amount);

            var buyDate = dateTime.AddDays(1);
            var buy = TransactionFactory.ConstructBuy(buyDate, "DE", 100.00m, 10, 7.95m);
            target.AddTransaction(buy);

            var expected = dateTime;
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadTestWithOneTransaction()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio();

            target.Deposit(dateTime, amount);
            
            var expected = dateTime;
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadTestWithTwoTransactions()
        {
            var originalDate = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio();

            target.Deposit(originalDate, amount);
            target.Deposit(originalDate.AddDays(10), amount);

            var expected = originalDate;
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TailTestWhenEmpty()
        {
            var target = PortfolioFactory.ConstructPortfolio();

            // measure DayOfYear because ticks will be slightly different between calls
            var expected = DateTime.Now.DayOfYear;
            var actual = target.Tail.DayOfYear;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TailTestWithPosition()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio();

            target.Deposit(dateTime, amount);

            var buyDate = dateTime.AddDays(1);
            var buy = TransactionFactory.ConstructBuy(buyDate, "DE", 100.00m, 10, 7.95m);
            target.AddTransaction(buy);

            var expected = buyDate;
            var actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TailTestWithOneTransaction()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio();

            target.Deposit(dateTime, amount);

            var expected = dateTime;
            var actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TailTestWithTwoTransactions()
        {
            var originalDate = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = PortfolioFactory.ConstructPortfolio();

            target.Deposit(originalDate, amount);
            var nextDate = originalDate.AddDays(10);
            target.Deposit(nextDate, amount);

            var expected = nextDate;
            var actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CashTickerSetCorrectly()
        {
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            var target = PortfolioFactory.ConstructPortfolio(ticker);

            const string expected = ticker;
            var actual = target.CashTicker;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HasValueTest()
        {
            var testDate = new DateTime(2011, 1, 8);
            var purchaseDate = testDate.AddDays(1);
            const decimal amount = 10000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            var target = PortfolioFactory.ConstructPortfolio(purchaseDate, amount, ticker);

            Assert.AreEqual(false, target.HasValueInRange(testDate));
            Assert.AreEqual(true, target.HasValueInRange(purchaseDate));
            Assert.AreEqual(true, target.HasValueInRange(purchaseDate.AddDays(1)));
        }

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
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, buyPrice, shares);
            target.AddTransaction(buy);

            var sellDate = new DateTime(2011, 9, 26);
            const decimal sellPrice = 75.00m;
            var sell = TransactionFactory.ConstructSell(sellDate, ticker, sellPrice, shares);
            target.AddTransaction(sell);

            var holdings = target.CalculateHoldings(sellDate);

            Assert.AreEqual(1, holdings.Count);
            var expected = new Holding
            {
                Ticker = ticker,
                Head = buyDate,
                Tail = sellDate,
                Shares = shares,
                OpenPrice = buyPrice * (decimal)shares,
                ClosePrice = sellPrice * (decimal)shares
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

            target.AddTransaction(TransactionFactory.ConstructBuy(firstBuyDate, ticker, buyPrice, sharesBought, commission));
            target.AddTransaction(TransactionFactory.ConstructBuy(secondBuyDate, ticker, buyPrice, sharesBought, commission));

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.AddTransaction(TransactionFactory.ConstructSell(firstSellDate, ticker, sellPrice, sharesSold, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(secondSellDate, ticker, sellPrice, sharesSold, commission));

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const double sharesInHolding = sharesSold;
            var expected1 = new Holding
            {
                Ticker = ticker,
                Head = secondBuyDate,
                Tail = secondSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
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

            target.AddTransaction(TransactionFactory.ConstructBuy(firstBuyDate, firstTicker, buyPrice, sharesBought, commission));
            target.AddTransaction(TransactionFactory.ConstructBuy(secondBuyDate, secondTicker, buyPrice, sharesBought, commission));

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.AddTransaction(TransactionFactory.ConstructSell(firstSellDate, firstTicker, sellPrice, sharesSold, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(secondSellDate, secondTicker, sellPrice, sharesSold, commission));

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const double sharesInHolding = sharesSold;
            var expected1 = new Holding
            {
                Ticker = secondTicker,
                Head = secondBuyDate,
                Tail = secondSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };
            var expected2 = new Holding
            {
                Ticker = firstTicker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
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

            target.AddTransaction(TransactionFactory.ConstructBuy(firstBuyDate, firstTicker, buyPrice, sharesBought, commission));
            target.AddTransaction(TransactionFactory.ConstructBuy(secondBuyDate, secondTicker, buyPrice, sharesBought, commission));

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.AddTransaction(TransactionFactory.ConstructSell(firstSellDate, firstTicker, sellPrice, sharesSold, commission));
            target.AddTransaction(TransactionFactory.ConstructSell(secondSellDate, secondTicker, sellPrice, sharesSold, commission));

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const double sharesInHolding = sharesSold;
            var expected1 = new Holding
            {
                Ticker = secondTicker,
                Head = secondBuyDate,
                Tail = secondSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };
            var expected2 = new Holding
            {
                Ticker = firstTicker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };
            var holding1 = holdings[0];
            var holding2 = holdings[1];
            Assert.AreEqual(expected1, holding1);
            Assert.AreEqual(expected2, holding2);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ValuesTest()
        {
            const string ticker = "DE";
            var target = PortfolioFactory.ConstructPortfolio(ticker);

            var values = target.Values;
        }
    }
}
