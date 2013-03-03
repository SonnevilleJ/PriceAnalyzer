using Sonneville.PriceTools;
using Sonneville.PriceTools.Fidelity;
using Sonneville.Utilities;

namespace Test.Sonneville.PriceTools.PortfolioData
{
    public static class TestTransactionHistory
    {
        /// <summary>
        /// Consists of 6 months of trading activity investing in 4 stocks.
        /// </summary>
        public static ISecurityBasket FidelityTransactions
        {
            get { return new FidelityTransactionHistoryCsvFile(new ResourceStream(TestTransactionHistoryCsvFiles.FidelityTransactions)); }
        }

        /// <summary>
        /// Consists of 15 months of trading activity in a retirement account.
        /// </summary>
        public static ISecurityBasket BrokerageLinkTransactions
        {
            get { return new FidelityBrokerageLinkTransactionHistoryCsvFile(new ResourceStream(TestTransactionHistoryCsvFiles.BrokerageLinkTransactions)); }
        }
    }
}
