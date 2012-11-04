using Sonneville.PriceTools.Fidelity;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.SamplePortfolioData
{
    public static class SampleTransactionHistory
    {
        /// <summary>
        /// Consists of 6 months of trading activity investing in 4 stocks.
        /// </summary>
        public static SecurityBasket FidelityTransactions
        {
            get { return new FidelityTransactionHistoryCsvFile(new ResourceStream(SampleTransactionHistoryCsvFiles.FidelityTransactions)); }
        }

        /// <summary>
        /// Consists of 15 months of trading activity in a retirement account.
        /// </summary>
        public static SecurityBasket BrokerageLinkTransactions
        {
            get { return new FidelityBrokerageLinkTransactionHistoryCsvFile(new ResourceStream(SampleTransactionHistoryCsvFiles.BrokerageLinkTransactions)); }
        }
    }
}
