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
        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod()]
        public void ParsePortfolioTest()
        {
            using (Stream csvStream = new MemoryStream(TestData.FidelityTransactions))
            {
                FidelityTransactionHistoryCsvFile target = new FidelityTransactionHistoryCsvFile(csvStream);
                IPortfolio portfolio = new Portfolio(target, "FTEXX");

                const decimal expectedValue = 2848.43m;
                decimal actualValue = portfolio.GetValue(new DateTime(2010, 11, 16));
                Assert.AreEqual(expectedValue, actualValue);
            }

        }
    }
}
