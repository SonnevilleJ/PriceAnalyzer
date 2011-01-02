using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceToolsTest
{
    
    
    /// <summary>
    ///This is a test class for FidelityPortfolioCsvParserTest and is intended
    ///to contain all FidelityPortfolioCsvParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FidelityPortfolioCsvParserTest
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
            Stream csvStream = new MemoryStream(TestData.FidelityTransactions);
            FidelityTransactionHistoryCsvParser target = new FidelityTransactionHistoryCsvParser(csvStream);
            const decimal expected = 2848.4m;
            decimal actual = target.ParsePortfolio().GetValue(new DateTime(2010, 09, 13));
            Assert.AreEqual(expected, actual);
        }
    }
}
