using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sonneville.PriceToolsTest
{
    
    
    /// <summary>
    ///This is a test class for DepositTest and is intended
    ///to contain all DepositTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DepositTest
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
        ///A test for Date
        ///</summary>
        [TestMethod()]
        public void DateTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount, ticker);

            DateTime expectedDate = dateTime;
            DateTime actualDate = target.SettlementDate;
            Assert.AreEqual(expectedDate, actualDate);
        }

        /// <summary>
        ///A test for OrderType
        ///</summary>
        [TestMethod()]
        public void OrderTypeTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount, ticker);

            const OrderType expectedType = OrderType.Deposit;
            OrderType actualType = target.OrderType;
            Assert.AreEqual(expectedType, actualType);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod()]
        public void TickerTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount, ticker);

            const string expectedTicker = ticker;
            string actualTicker = target.Ticker;
            Assert.AreEqual(expectedTicker, actualTicker);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void PriceTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount, ticker);

            const decimal expectedPrice = -1.00m; // Deposits return negative price
            decimal actualPrice = target.Price;
            Assert.AreEqual(expectedPrice, actualPrice);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        public void SharesTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount, ticker);

            const double expectedShares = (double)amount;
            double actualShares = target.Shares;
            Assert.AreEqual(expectedShares, actualShares);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        public void CommissionTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount, ticker);

            const decimal expectedCommission = 0.00m;
            decimal actualCommission = target.Commission;
            Assert.AreEqual(expectedCommission, actualCommission);
        }

        [TestMethod()]
        public void DefaultTickerTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount);

            string expectedTicker = String.Empty;
            string actualTicker = target.Ticker;
            Assert.AreEqual(expectedTicker, actualTicker);
        }
    }
}
