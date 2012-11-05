using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.SamplePortfolioData;
using Sonneville.PriceTools.Yahoo;

namespace Test.Sonneville.PriceTools.AutomatedTrading
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
            Assert.AreEqual(0, target.Positions.Count());
        }

        [TestMethod]
        public void ConstructorTest2()
        {
            const string ticker = "FDRXX";  // Fidelity Cash Reserves
            var target = PortfolioFactory.ConstructPortfolio(ticker);

            Assert.AreEqual(ticker, target.CashTicker);
            Assert.AreEqual(0, target.GetAvailableCash(DateTime.Now));
            Assert.AreEqual(0, target.Positions.Count());
        }

        [TestMethod]
        public void ConstructorTest3()
        {
            var openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            const string ticker = "FDRXX";  // Fidelity Cash Reserves
            var target = PortfolioFactory.ConstructPortfolio(ticker, openDate, amount);

            Assert.AreEqual(ticker, target.CashTicker);
            Assert.AreEqual(amount, target.GetAvailableCash(openDate));
            Assert.AreEqual(0, target.Positions.Count());
        }

        [TestMethod]
        public void Constructor4Test()
        {
            var csvFile = SampleTransactionHistory.FidelityTransactions;
            var ticker = String.Empty;

            var target = PortfolioFactory.ConstructPortfolio(csvFile.Transactions);

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
        public void GetInvestedValueFromEmptyPortfolio()
        {
            var target = PortfolioFactory.ConstructPortfolio();
            Assert.AreEqual(0.0m, target.CalculateMarketValue(new YahooPriceDataProvider(), DateTime.Now));
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
            const decimal shares = 2;
            const decimal commission = 7.95m;
            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission);

            target.AddTransaction(buy);

            // If the price date falls within a period, CalculateMarketValue will use price data from that period.
            // Because of this, I changed the price date to 11:59 pm rather than the next day (default of midnight).
            var priceDate = new DateTime(2011, 4, 25).CurrentPeriodClose(Resolution.Days);
            
            const decimal expected = 189.44m; // closing price 25 April 2011 = $94.72 * 2 shares = 189.44
            var actual = target.CalculateMarketValue(new YahooPriceDataProvider(), priceDate);
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
            const decimal shares = 2;
            const decimal commission = 7.95m;
            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission);
            var sell = TransactionFactory.ConstructSell(ticker, sellDate, shares, price, commission);

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = 0.00m; // all shares sold = no value
            var actual = target.CalculateMarketValue(new YahooPriceDataProvider(), sellDate);
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
            const decimal shares = 2;
            const decimal commission = 7.95m;
            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission);
            var sell = TransactionFactory.ConstructSell(ticker, sellDate, shares, price, commission);

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
            const decimal shares = 2;
            const decimal commission = 7.95m;
            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission);
            var sell = TransactionFactory.ConstructSell(ticker, sellDate, shares, price, commission);

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
            const decimal shares = 2;
            const decimal commission = 7.95m;
            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission);
            var sell = TransactionFactory.ConstructSell(ticker, sellDate, shares, price, commission);

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
            var buy = TransactionFactory.ConstructBuy(ticker, dateTime, 5, 0.00m);
            
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
        public void IndexerReturnsCalculateGrossProfit()
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
            const decimal shares = 2;
            var buy = TransactionFactory.ConstructBuy(ticker, date, shares, price);
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
            const decimal shares = 2;
            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, price);
            var sell = TransactionFactory.ConstructSell(ticker, buyDate.AddDays(1), shares, price);

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
            const decimal shares = 2;
            var sellShort = TransactionFactory.ConstructSellShort(ticker, buyDate, shares, price);
            var buyToCover = TransactionFactory.ConstructBuyToCover(ticker, buyDate.AddDays(1), shares, price);

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
            const decimal shares = 2;
            var sellShort = TransactionFactory.ConstructSellShort(ticker, date, shares, price);
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
            const decimal shares = 2;
            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, price);
            var dividendReceipt = TransactionFactory.ConstructDividendReceipt(buyDate.AddDays(1), shares);

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
            const decimal shares = 2;
            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, price);
            var dividendReinvestment = TransactionFactory.ConstructDividendReinvestment(ticker, buyDate.AddDays(1), shares, 1);

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
            const decimal shares = 2;

            // second transaction: implicit withdrawal from buy1
            // third transaction: buy1
            // fourth transaction: implicit withdrawal from buy2
            // fifth transaction: buy2
            var buy1 = TransactionFactory.ConstructBuy(de, buyDate, shares, price);
            var buy2 = TransactionFactory.ConstructBuy(msft, buyDate, shares, price);
            target.AddTransaction(buy1);
            target.AddTransaction(buy2);
            
            const int expectedTransactions = 5;
            var actualTransactions = target.Transactions.Count();
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
            const decimal shares = 2;
            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, price);
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
            var buy = TransactionFactory.ConstructBuy("DE", buyDate, 10, 100.00m, 7.95m);
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
            var buy = TransactionFactory.ConstructBuy("DE", buyDate, 10, 100.00m, 7.95m);
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
    }
}
