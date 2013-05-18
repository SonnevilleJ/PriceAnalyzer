using Sonneville.PriceTools.Fidelity;
using Sonneville.Utilities;
using Test.Sonneville.PriceTools.PortfolioData.Internal;

namespace Test.Sonneville.PriceTools.PortfolioData
{
    public static class TestPortfolios
    {
        public static ITestPortfolio FidelityBrokerageLink
        {
            get
            {
                var testPortfolio = new TestPortfolio
                    {
                        CsvString = FidelityData.BrokerageLink_trades,
                        TransactionHistory = new FidelityBrokerageLinkTransactionHistoryCsvFile(new ResourceStream(FidelityData.BrokerageLink_trades)),
                    };
                return testPortfolio;
            }
        }

        public static ITestPortfolio FidelityTaxable
        {
            get
            {
                var testPortfolio = new TestPortfolio();
                testPortfolio.CsvString = FidelityData.FidelityTransactions;
                testPortfolio.TransactionHistory = new FidelityTransactionHistoryCsvFile(new ResourceStream(FidelityData.FidelityTransactions));
                return testPortfolio;
            }
        }

        public static ITestPortfolio FidelityBrokerageLinkTransactionPriceRounding
        {
            get
            {
                var testPortfolio = new TestPortfolio
                    {
                        CsvString = FidelityData.BrokerageLink_TransactionPriceRounding,
                        TransactionHistory = new FidelityBrokerageLinkTransactionHistoryCsvFile(new ResourceStream(FidelityData.BrokerageLink_TransactionPriceRounding)),
                    };
                return testPortfolio;
            }
        }
    }
}