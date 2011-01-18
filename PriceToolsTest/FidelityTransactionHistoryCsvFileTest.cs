using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceToolsTest
{
    
    
    /// <summary>
    ///This is a test class for FidelityTransactionHistoryCsvFileTest and is intended
    ///to contain all FidelityTransactionHistoryCsvFileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FidelityTransactionHistoryCsvFileTest
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
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod()]
        public void ParsePortfolioTest()
        {
            using (Stream csvStream = new MemoryStream(TestData.FidelityTransactions))
            {
                IPortfolio portfolio;
                using (FidelityTransactionHistoryCsvFile target = new FidelityTransactionHistoryCsvFile(csvStream))
                {
                    portfolio = new Portfolio(target, "FTEXX");
                }

                const decimal expectedValue = 2848.43m;
                decimal actualValue = portfolio.GetValue(new DateTime(2010, 11, 16));
                Assert.AreEqual(expectedValue, actualValue);
            }

        }
    }
}
