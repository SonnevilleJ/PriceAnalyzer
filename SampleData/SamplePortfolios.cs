using SampleData.Internal;
using Sonneville.PriceTools.Fidelity;
using Sonneville.Utilities;

namespace SampleData
{
    public static class SamplePortfolios
    {
        public static ISamplePortfolio FidelityBrokerageLink
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

        public static ISamplePortfolio FidelityTaxable
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

        public static ISamplePortfolio FidelityBrokerageLinkTransactionPriceRounding
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