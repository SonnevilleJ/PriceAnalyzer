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


        [TestMethod()]
        public void GetAvailableCashNoTransactions()
        {
            Portfolio target = new Portfolio();

            const decimal expectedCash = 0;
            decimal availableCash = target.GetAvailableCash(DateTime.Now);
            Assert.AreEqual(expectedCash, availableCash);
        }

        [TestMethod()]
        public void GetValueNoTransactions()
        {
            Portfolio target = new Portfolio();

            const decimal expectedValue = 0;
            decimal actualValue = target.GetValue(DateTime.Now);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod()]
        public void GetAvailableCashOfDeposit()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            Portfolio target = new Portfolio(dateTime, openingDeposit);

            const decimal expectedCash = openingDeposit;
            decimal availableCash = target.GetAvailableCash(dateTime);
            Assert.AreEqual(expectedCash, availableCash);
        }

        [TestMethod()]
        public void GetValueOfDeposit()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            Portfolio target = new Portfolio(dateTime, openingDeposit);

            const decimal expectedValue = openingDeposit;
            decimal actualValue = target.GetValue(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod()]
        public void GetAvailableCashAfterFullWithdrawal()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            Portfolio target = new Portfolio(dateTime, amount);

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
            Portfolio target = new Portfolio(dateTime, amount);

            DateTime withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(dateTime.AddDays(1), amount);

            const decimal expectedValue = 0;
            decimal actualValue = target.GetValue(withdrawalDate);
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
