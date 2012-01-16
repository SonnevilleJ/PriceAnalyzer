using Sonneville.PriceTools.Fidelity;
using Sonneville.PriceTools.SamplePriceData;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools.SamplePortfolioData
{
    public static class PortfolioTransactionHistoryCsvFiles
    {
        public static ITransactionHistory FidelityTransactions
        {
            get { return new FidelityTransactionHistoryCsvFile(new ResourceStream(PortfolioCsv.FidelityTransactions)); }
        }

        public static ITransactionHistory BrokerageLink_trades
        {
            get { return new FidelityTransactionHistoryCsvFile(new ResourceStream(PortfolioCsv.BrokerageLink_trades)); }
        }

        public static ITransactionHistory BrokerageLink_TransactionPriceRounding
        {
            get { return new FidelityTransactionHistoryCsvFile(new ResourceStream(PortfolioCsv.BrokerageLink_TransactionPriceRounding)); }
        }
    }
}
