using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sonneville.PriceToolsTest
{
    
    
    /// <summary>
    ///This is a test class for TransactionTest and is intended
    ///to contain all TransactionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TransactionTest
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
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = -5;           // bought 5 shares
            const decimal commission = 7.95m;  // bought with $7.95 commission
            Transaction target = new Transaction(date, OrderType.Buy, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativePriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = -100.0m;      // bought at $100.00 per share
            const double shares = 5;           // bought 5 shares
            const decimal commission = 7.95m;  // bought with $7.95 commission
            Transaction target = new Transaction(date, OrderType.Buy, ticker, price, shares, commission); // TODO: Initialize to an appropriate value
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeCommissionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = 5;           // bought 5 shares
            const decimal commission = -7.95m;  // bought with $7.95 commission
            Transaction target = new Transaction(date, OrderType.Buy, ticker, price, shares, commission); // TODO: Initialize to an appropriate value
        }
    }
}
