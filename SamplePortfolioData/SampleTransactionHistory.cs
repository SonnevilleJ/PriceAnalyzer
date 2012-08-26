using Sonneville.PriceTools.Fidelity;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.SamplePortfolioData
{
    public static class SampleTransactionHistory
    {
        /// <summary>
        /// Consists of 6 months of trading activity investing in 4 stocks.
        /// </summary>
        public static Data.TransactionHistory FidelityTransactions
        {
            get { return new FidelityTransactionHistoryCsvFile(new ResourceStream(SamplePortfolioCsv.FidelityTransactions)); }
        }

        /// <summary>
        /// Consists of 15 months of trading activity in a retirement account.
        /// </summary>
        public static Data.TransactionHistory BrokerageLink_trades
        {
            get { return new FidelityBrokerageLinkTransactionHistoryCsvFile(new ResourceStream(SamplePortfolioCsv.BrokerageLink_trades)); }
        }
    }
}
