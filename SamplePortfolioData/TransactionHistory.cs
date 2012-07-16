using Sonneville.PriceTools.Fidelity;
using Sonneville.PriceTools.SamplePriceData;

namespace Sonneville.PriceTools.SamplePortfolioData
{
    public static class TransactionHistory
    {
        /// <summary>
        /// Consists of 6 months of trading activity investing in 4 stocks.
        /// </summary>
        public static Data.TransactionHistory FidelityTransactions
        {
            get { return new FidelityTransactionHistoryCsvFile(new ResourceStream(PortfolioCsv.FidelityTransactions)); }
        }

        /// <summary>
        /// Consists of 15 months of trading activity in a retirement account.
        /// </summary>
        public static Data.TransactionHistory BrokerageLink_trades
        {
            get { return new FidelityBrokerageLinkTransactionHistoryCsvFile(new ResourceStream(PortfolioCsv.BrokerageLink_trades)); }
        }
    }
}
