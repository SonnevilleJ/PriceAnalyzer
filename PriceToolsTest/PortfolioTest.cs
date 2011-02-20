using Sonneville.PriceTools;
using Sonneville.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for Portfolio and is intended
    ///to contain all Portfolio Unit Tests
    ///</summary>
    [TestClass()]
    public class PortfolioTest
    {
        [TestMethod]
        public void ConstructorTest1()
        {
            IPortfolio target = new Portfolio();

            Assert.AreEqual(string.Empty, target.CashTicker);
            Assert.AreEqual(0, target.GetAvailableCash(DateTime.Now));
            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void ConstructorTest2()
        {
            const string ticker = "FDRXX";  // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(ticker);

            Assert.AreEqual(ticker, target.CashTicker);
            Assert.AreEqual(0, target.GetAvailableCash(DateTime.Now));
            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void ConstructorTest3()
        {
            DateTime openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            const string ticker = "FDRXX";  // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(openDate, amount, ticker);

            Assert.AreEqual(ticker, target.CashTicker);
            Assert.AreEqual(amount, target.GetAvailableCash(openDate));
            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void DepositTest1()
        {
            DateTime openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(openDate, amount);

            Assert.AreEqual(amount, target.GetAvailableCash(openDate));
        }

        [TestMethod]
        public void DepositTest2()
        {
            DateTime openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            Deposit deposit = new Deposit
                                  {
                                      SettlementDate = openDate,
                                      Amount = amount
                                  };
            target.Deposit(deposit);

            Assert.AreEqual(amount, target.GetAvailableCash(openDate));
        }

        [TestMethod]
        public void WithdrawalTest1()
        {
            DateTime openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(openDate, amount);
            target.Withdraw(openDate, amount);

            Assert.AreEqual(0, target.GetAvailableCash(openDate));
        }

        [TestMethod]
        public void WithdrawalTest2()
        {
            DateTime openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            Deposit deposit = new Deposit
                                  {
                                      SettlementDate = openDate,
                                      Amount = amount
                                  };
            Withdrawal withdrawal = new Withdrawal
                                        {
                                            SettlementDate = openDate,
                                            Amount = amount
                                        };
            target.Deposit(deposit);
            target.Withdraw(withdrawal);

            Assert.AreEqual(0, target.GetAvailableCash(openDate));
        }

        [TestMethod()]
        public void GetAvailableCashNoTransactions()
        {
            IPortfolio target = new Portfolio();

            const decimal expectedCash = 0;
            decimal availableCash = target.GetAvailableCash(DateTime.Now);
            Assert.AreEqual(expectedCash, availableCash);
        }

        [TestMethod()]
        public void GetValueNoTransactions()
        {
            IPortfolio target = new Portfolio();

            const decimal expectedValue = 0;
            decimal actualValue = target.GetValue(DateTime.Now);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod()]
        public void GetAvailableCashOfDeposit()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, openingDeposit);

            const decimal expectedCash = openingDeposit;
            decimal availableCash = target.GetAvailableCash(dateTime);
            Assert.AreEqual(expectedCash, availableCash);
        }

        [TestMethod()]
        public void GetValueOfDeposit()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, openingDeposit);

            const decimal expectedValue = openingDeposit;
            decimal actualValue = target.GetValue(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod()]
        public void GetAvailableCashAfterFullWithdrawal()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio(dateTime, amount);

            DateTime withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(dateTime.AddDays(1), amount);

            const decimal expectedCash = 0;
            decimal availableCash = target.GetAvailableCash(withdrawalDate);
            Assert.AreEqual(expectedCash, availableCash);
        }

        [TestMethod()]
        public void GetValueAfterFullWithdrawal()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio(dateTime, amount);

            DateTime withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(dateTime.AddDays(1), amount);

            const decimal expectedValue = 0;
            decimal actualValue = target.GetValue(withdrawalDate);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod()]
        public void AddTransactionTest()
        {
            //DateTime dateTime = new DateTime(2011, 1, 8);
            //const decimal deposit = 10000m;
            //IPortfolio target = new Portfolio(dateTime, deposit);

            //DateTime buyDate = new DateTime(2011, 1, 9);
            //const string ticker = "DE";
            //const decimal price = 50.00m;
            //const double shares = 2;
            //Buy buy = new Buy {SettlementDate = buyDate, Ticker = ticker, Price = price, Shares = shares};
            //target.AddTransaction(buy);

        }

        [TestMethod()]
        public void PositionTest_OnePosition_TwoCashTransactions()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            Buy buy = new Buy {SettlementDate = buyDate, Ticker = ticker, Price = price, Shares = shares};
            target.AddTransaction(buy);

            const decimal withdrawal = 5000m;
            DateTime withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(withdrawalDate, withdrawal);

            const int expectedTransactions = 1;
            int actualTransactions = target.Positions.Count;
            Assert.AreEqual(expectedTransactions, actualTransactions);

            const decimal expectedValue = 5000m;
            decimal actualValue = target.GetValue(buyDate);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WithdrawWithoutAvailableCash()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio(dateTime, amount);

            DateTime buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            Buy buy = new Buy { SettlementDate = buyDate, Ticker = ticker, Price = price, Shares = shares };
            target.AddTransaction(buy);

            DateTime withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(withdrawalDate, amount);
        }

        [TestMethod]
        public void HeadTestWithPosition()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(dateTime, amount);

            DateTime buyDate = dateTime.AddDays(1);
            Buy buy = new Buy
                          {
                              SettlementDate = buyDate,
                              Ticker = "DE",
                              Price = 100.00m,
                              Shares = 10,
                              Commission = 7.95m,
                          };
            target.AddTransaction(buy);

            DateTime expected = dateTime;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadTestWithOneTransaction()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(dateTime, amount);
            
            DateTime expected = dateTime;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadTestWithTwoTransactions()
        {
            DateTime originalDate = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(originalDate, amount);
            target.Deposit(originalDate.AddDays(10), amount);

            DateTime expected = originalDate;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TailTestWithPosition()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(dateTime, amount);

            DateTime buyDate = dateTime.AddDays(1);
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = "DE",
                Price = 100.00m,
                Shares = 10,
                Commission = 7.95m,
            };
            target.AddTransaction(buy);

            DateTime expected = buyDate;
            DateTime actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TailTestWithOneTransaction()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(dateTime, amount);

            DateTime expected = dateTime;
            DateTime actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TailTestWithTwoTransactions()
        {
            DateTime originalDate = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(originalDate, amount);
            target.Deposit(originalDate.AddDays(10), amount);

            DateTime expected = originalDate.AddDays(10);
            DateTime actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CashTickerSetCorrectly()
        {
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(ticker);

            const string expected = ticker;
            string actual = target.CashTicker;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HasValueTest()
        {
            DateTime testDate = new DateTime(2011, 1, 8);
            DateTime purchaseDate = testDate.AddDays(1);
            const decimal amount = 10000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(purchaseDate, amount, ticker);

            Assert.AreEqual(false, target.HasValue(testDate));
            Assert.AreEqual(true, target.HasValue(purchaseDate));
            Assert.AreEqual(true, target.HasValue(purchaseDate.AddDays(1)));
        }

        [TestMethod]
        public void SerializePortfolioTest()
        {
            DateTime testDate = new DateTime(2011, 1, 8);
            DateTime purchaseDate = testDate.AddDays(1);
            const decimal amount = 10000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(purchaseDate, amount, ticker);

            decimal expected = target.GetValue(purchaseDate);
            decimal actual = ((IPortfolio)TestUtilities.Serialize(target)).GetValue(purchaseDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EntityPortfolioTest()
        {
            DateTime testDate = new DateTime(2011, 1, 8);
            DateTime purchaseDate = testDate.AddDays(1);
            const decimal amount = 10000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(purchaseDate, amount, ticker);

            TestUtilities.VerifyPortfolioEntity(target);
        }
    }
}
