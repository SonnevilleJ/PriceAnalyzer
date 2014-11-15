using Sonneville.PriceTools.Fidelity;
using Sonneville.PriceTools.SampleData.Internal;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.SampleData
{
    public static class SamplePortfolios
    {
        private static readonly FidelityTransactionHistoryCsvFile _fidelityTransactionHistoryCsvFile;

        static SamplePortfolios()
        {
            _fidelityTransactionHistoryCsvFile = new FidelityTransactionHistoryCsvFile();
        }

        public static SamplePortfolio FidelityBrokerageLink
        {
            get
            {
                return new SamplePortfolio
                    {
                        CsvString = FidelityData.BrokerageLink_trades,
                        TransactionHistory = new FidelityBrokerageLinkTransactionHistoryCsvFile(new ResourceStream(FidelityData.BrokerageLink_trades), new TransactionFactory(), new HoldingFactory()),
                    };
            }
        }

        public static SamplePortfolio FidelityTaxable
        {
            get
            {
                _fidelityTransactionHistoryCsvFile.Parse(new ResourceStream(FidelityData.FidelityTransactions));
                return new SamplePortfolio
                    {
                        CsvString = FidelityData.FidelityTransactions,
                        TransactionHistory = _fidelityTransactionHistoryCsvFile
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
                        TransactionHistory = new FidelityBrokerageLinkTransactionHistoryCsvFile(new ResourceStream(FidelityData.BrokerageLink_TransactionPriceRounding), new TransactionFactory(), new HoldingFactory()),
                    };
            }
        }
    }
}