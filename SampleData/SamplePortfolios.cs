using Sonneville.PriceTools.Fidelity;
using Sonneville.PriceTools.SampleData.Internal;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.SampleData
{
    public static class SamplePortfolios
    {
        private static readonly FidelityTransactionHistoryCsvFile _fidelityTransactionHistoryCsvFile;
        private static readonly TransactionFactory _transactionFactory;
        private static readonly HoldingFactory _holdingFactory;

        static SamplePortfolios()
        {
            _transactionFactory = new TransactionFactory();
            _holdingFactory = new HoldingFactory();
            _fidelityTransactionHistoryCsvFile = new FidelityTransactionHistoryCsvFile(_transactionFactory, _holdingFactory);
        }

        public static SamplePortfolio FidelityBrokerageLink
        {
            get
            {
                return new SamplePortfolio
                    {
                        CsvString = FidelityData.BrokerageLink_trades,
                        TransactionHistory = new FidelityBrokerageLinkTransactionHistoryCsvFile(new ResourceStream(FidelityData.BrokerageLink_trades), _transactionFactory, _holdingFactory),
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
                        TransactionHistory = new FidelityBrokerageLinkTransactionHistoryCsvFile(new ResourceStream(FidelityData.BrokerageLink_TransactionPriceRounding), _transactionFactory, _holdingFactory),
                    };
            }
        }
    }
}