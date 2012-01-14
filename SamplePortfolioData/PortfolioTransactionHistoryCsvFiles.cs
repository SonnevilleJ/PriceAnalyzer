using Sonneville.PriceTools.Data.Fidelity;
using Sonneville.PriceTools.SamplePriceData;
using Sonneville.PriceTools.Services;

namespace SamplePortfolioData
{
    public static class PortfolioTransactionHistoryCsvFiles
    {
        public static TransactionHistoryCsvFile FidelityTransactions
        {
            get { return new FidelityTransactionHistoryCsvFile(new ResourceStream(PortfolioCsv.FidelityTransactions)); }
        }

        public static TransactionHistoryCsvFile BrokerageLink_trades
        {
            get { return new FidelityTransactionHistoryCsvFile(new ResourceStream(PortfolioCsv.BrokerageLink_trades)); }
        }

        public static TransactionHistoryCsvFile BrokerageLink_TransactionPriceRounding
        {
            get { return new FidelityTransactionHistoryCsvFile(new ResourceStream(PortfolioCsv.BrokerageLink_TransactionPriceRounding)); }
        }
    }
}
