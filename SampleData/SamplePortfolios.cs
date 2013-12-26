using Sonneville.PriceTools.Fidelity;
using Sonneville.PriceTools.SampleData.Internal;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.SampleData
{
    public static class SamplePortfolios
    {
        public static SamplePortfolio FidelityBrokerageLink
        {
            get
            {
                return new SamplePortfolio
                    {
                        CsvString = FidelityData.BrokerageLink_trades,
                        TransactionHistory = new FidelityBrokerageLinkTransactionHistoryCsvFile(new ResourceStream(FidelityData.BrokerageLink_trades)),
                    };
            }
        }

        public static SamplePortfolio FidelityTaxable
        {
            get
            {
                return new SamplePortfolio
                    {
                        CsvString = FidelityData.FidelityTransactions,
                        TransactionHistory = new FidelityTransactionHistoryCsvFile(new ResourceStream(FidelityData.FidelityTransactions))
                    };
            }
        }

        public static SamplePortfolio FidelityBrokerageLinkTransactionPriceRounding
        {
            get
            {
                return new SamplePortfolio
                    {
                        CsvString = FidelityData.BrokerageLink_TransactionPriceRounding,
                        TransactionHistory = new FidelityBrokerageLinkTransactionHistoryCsvFile(new ResourceStream(FidelityData.BrokerageLink_TransactionPriceRounding)),
                    };
            }
        }
    }
}