using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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


        /// <summary>
        ///A test for Portfolio Constructor
        ///</summary>
        [TestMethod()]
        public void PortfolioConstructorWithTransactionsTest()
        {
            const decimal availableCash = 10000m;
            ITransaction[] transactions = null; // TODO: Initialize to an appropriate value
            Portfolio target = new Portfolio(availableCash, transactions);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Portfolio Constructor
        ///</summary>
        [TestMethod()]
        public void PortfolioDefaultConstructorTest()
        {
            Portfolio target = new Portfolio();
            Assert.AreEqual(0, target.AvailableCash);
        }

        /// <summary>
        ///A test for Portfolio Constructor
        ///</summary>
        [TestMethod()]
        public void PortfolioConstructorWithOpeningDepositTest()
        {
            const decimal openingDeposit = 10000m;
            Portfolio target = new Portfolio(openingDeposit);
            Assert.AreEqual(openingDeposit, target.AvailableCash);
        }

        /// <summary>
        ///A test for AddTransaction
        ///</summary>
        [TestMethod()]
        public void AddTransactionTest()
        {
            Portfolio target = new Portfolio(); // TODO: Initialize to an appropriate value
            ITransaction transaction = null; // TODO: Initialize to an appropriate value
            target.AddTransaction(transaction);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for HasValue
        ///</summary>
        [TestMethod()]
        public void HasValueTest()
        {
            Portfolio target = new Portfolio(); // TODO: Initialize to an appropriate value
            DateTime date = new DateTime(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.HasValue(date);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Head
        ///</summary>
        [TestMethod()]
        public void HeadTest()
        {
            Portfolio target = new Portfolio(); // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = target.Head;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void TestValueWithSingleDepositOnly()
        {
            DateTime date = new DateTime(2011, 1, 6);
            const decimal amount = 500m;
            Deposit deposit = new Deposit(date, amount);
            Portfolio target = new Portfolio(0, deposit);

            decimal actual = target[date];

            Assert.AreEqual(amount, actual);
        }

        /// <summary>
        ///A test for OpenPositions
        ///</summary>
        [TestMethod()]
        public void OpenPositionsTest()
        {
            Portfolio target = new Portfolio(); // TODO: Initialize to an appropriate value
            IEnumerable<IPosition> actual;
            actual = target.OpenPositions;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Span
        ///</summary>
        [TestMethod()]
        public void SpanTest()
        {
            Portfolio target = new Portfolio(500m);
            Assert.AreEqual(1, target.Span);
        }

        /// <summary>
        ///A test for Tail
        ///</summary>
        [TestMethod()]
        public void TailTest()
        {
            Portfolio target = new Portfolio(); // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = target.Tail;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
