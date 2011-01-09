﻿using System.IO;
using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Serialization;
using Sonneville.Utilities;

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
            new Transaction(date, OrderType.Buy, ticker, price, shares, commission);
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
            new Transaction(date, OrderType.Buy, ticker, price, shares, commission);
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
            new Transaction(date, OrderType.Buy, ticker, price, shares, commission);
        }

        [TestMethod()]
        public void SerializeTransactionTest()
        {
            const string ticker = "DE";
            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime purchaseDate = testDate.AddDays(1);
            const decimal buyPrice = 100.0m;    // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            ITransaction expected = new Transaction(purchaseDate, OrderType.Buy, ticker, buyPrice, shares, commission);

            MemoryStream stream = new MemoryStream();
            TestUtilities.BinarySerialize(expected, stream);

            ITransaction actual = (ITransaction)TestUtilities.BinaryDeserialize(stream);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Transaction Constructor
        ///</summary>
        [TestMethod()]
        public void TransactionConstructorTest1()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Buy;
            const decimal price = 100.0m;      // bought at $100.00 per share

            Transaction target = new Transaction(date, type, ticker, price);
            Assert.AreEqual(ticker, target.Ticker);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(type, target.OrderType);
            Assert.AreEqual(price, target.Price);
        }

        /// <summary>
        ///A test for Transaction Constructor
        ///</summary>
        [TestMethod()]
        public void TransactionConstructorTest2()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Buy;
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = 5;           // bought 5 shares

            Transaction target = new Transaction(date, type, ticker, price, shares);
            Assert.AreEqual(ticker, target.Ticker);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(type, target.OrderType);
            Assert.AreEqual(price, target.Price);
            Assert.AreEqual(shares, target.Shares);
        }

        /// <summary>
        ///A test for Transaction Constructor
        ///</summary>
        [TestMethod()]
        public void TransactionConstructorTest3()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Buy;
            const decimal price = 100.0m; // bought at $100.00 per share
            const double shares = 5; // bought 5 shares
            const decimal commission = 7.95m; // bought with $7.95 commission

            Transaction target = new Transaction(date, type, ticker, price, shares, commission);
            Assert.AreEqual(ticker, target.Ticker);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(type, target.OrderType);
            Assert.AreEqual(price, target.Price);
            Assert.AreEqual(shares, target.Shares);
            Assert.AreEqual(commission, target.Commission);
        }
    }
}
