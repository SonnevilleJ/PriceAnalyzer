using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for FidelityTransactionHistoryCsvFileTest and is intended
    ///to contain all FidelityTransactionHistoryCsvFileTest Unit Tests
    ///</summary>
    [TestClass]
    public class FidelityTransactionHistoryCsvFileTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Settings.SetDefaultSettings();
            Settings.CanConnectToInternet = false;
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioTest()
        {
            using (Stream csvStream = new MemoryStream(TestData.FidelityTransactions))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                IPortfolio portfolio = new Portfolio(target, "FTEXX");

                const decimal expected = 2848.43m;
                var actual = portfolio.GetValue(new DateTime(2010, 11, 16));
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void TransactionPriceRoundingTest()
        {
            using (Stream csvStream = new ResourceStream(TestData.TransactionPriceRounding))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var transactions = target.Transactions;

                const decimal expected = 500.00m;
                var actual = ((IShareTransaction) transactions.First()).TotalValue;
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
