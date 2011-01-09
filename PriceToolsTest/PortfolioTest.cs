using System.IO;
using Sonneville.PriceTools;
using Sonneville.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sonneville.PriceToolsTest
{
    
    
    /// <summary>
    ///This is a test class for PortfolioTest and is intended
    ///to contain all PortfolioTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PortfolioTest
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
        public void PositionTest_OnePosition_TwoTransactions()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio(dateTime, amount);

            DateTime withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(dateTime.AddDays(1), amount);

            const decimal expected = 1;
            decimal actual = target.Positions.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadTestWithOneTransaction()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio(dateTime, amount);

            DateTime expected = dateTime;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadTestWithTwoTransactions()
        {
            DateTime originalDate = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio(originalDate, amount);

            DateTime newDate = originalDate.AddDays(10);
            target.Deposit(newDate, amount);

            DateTime expected = originalDate;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TailTestWithOneTransaction()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio(dateTime, amount);

            DateTime expected = dateTime;
            DateTime actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TailTestWithTwoTransactions()
        {
            DateTime originalDate = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio(originalDate, amount);

            DateTime newDate = originalDate.AddDays(10);
            target.Deposit(newDate, amount);

            DateTime expected = newDate;
            DateTime actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CashTickerSetCorrectly()
        {
            DateTime originalDate = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(originalDate, amount, ticker);

            const string expected = ticker;
            string actual = target.CashTicker;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExceptionThrownWhenDepositConflictsWithCashTicker()
        {
            DateTime originalDate = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(originalDate, amount, ticker);

            const string newTicker = "FTEXX"; // Fidelity Municipal Money Market
            target.AddTransaction(originalDate.AddDays(100), OrderType.Deposit, newTicker, 5000);
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
            IPortfolio expected = new Portfolio(purchaseDate, amount, ticker);

            MemoryStream memoryStream = new MemoryStream();
            TestUtilities.BinarySerialize(expected, memoryStream);
            IPortfolio actual = (IPortfolio) TestUtilities.BinaryDeserialize(memoryStream);

            Assert.AreEqual(expected, actual);
        }
    }
}
