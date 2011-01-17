using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    
    
    /// <summary>
    ///This is a test class for ICashAccountTest and is intended
    ///to contain all ICashAccountTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CashAccountTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion



        /// <summary>
        ///A test for Deposit
        ///</summary>
        [TestMethod()]
        public void DepositPositiveTest()
        {
            ICashAccount target = new CashAccount();
            DateTime dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);

            const decimal expectedValue = amount;
            decimal actualValue = target.GetCashBalance(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        ///A test for Deposit
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DepositNegativeTest()
        {
            ICashAccount target = new CashAccount();
            DateTime dateTime = new DateTime(2010, 1, 16);
            const decimal amount = -500.00m;
            target.Deposit(dateTime, amount);
        }

        /// <summary>
        ///A test for Withdraw
        ///</summary>
        [TestMethod()]
        public void WithdrawPositiveTest()
        {
            ICashAccount target = new CashAccount();
            DateTime dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);
            target.Withdraw(dateTime, amount);

            const decimal expectedValue = 0.00m;
            decimal actualValue = target.GetCashBalance(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        ///A test for Withdraw
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WithdrawNegativeTest()
        {
            ICashAccount target = new CashAccount();
            DateTime dateTime = new DateTime(2010, 1, 16);
            const decimal deposit = 500.00m;
            const decimal withdraw = -500.00m;
            target.Deposit(dateTime, deposit);
            target.Withdraw(dateTime, withdraw);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod()]
        public void TickerTest()
        {
            const string ticker = "FTEXX";
            ICashAccount target = new CashAccount(ticker);

            const string expectedTicker = ticker;
            string actualTicker = target.Ticker;
            Assert.AreEqual(expectedTicker, actualTicker);
        }

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod()]
        public void TransactionsTest()
        {
            ICashAccount target = new CashAccount();
            DateTime dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);
            target.Withdraw(dateTime, amount);

            const int expectedTransactions = 2;
            int actualTransactions = target.Transactions.Count;
            Assert.AreEqual(expectedTransactions, actualTransactions);
        }

        [TestMethod]
        public void SerializeCashAccountTest()
        {
            DateTime date = new DateTime(2011, 1, 16);
            const decimal amount = 10000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            ICashAccount expected = new CashAccount(ticker);
            expected.Deposit(date, amount);
            expected.Withdraw(date, amount);

            TestUtilities.VerifySerialization(expected);
        }
    }
}
