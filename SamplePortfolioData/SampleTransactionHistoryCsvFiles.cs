namespace Sonneville.PriceTools.SamplePortfolioData
{
    public static class SampleTransactionHistoryCsvFiles
    {
        public static string FidelityTransactions
        {
            get { return FidelityData.FidelityTransactions; }
        }

        public static string BrokerageLinkTransactions
        {
            get { return FidelityData.BrokerageLink_trades; }
        }

        public static string BrokerageLink_TransactionPriceRounding
        {
            get { return FidelityData.BrokerageLink_TransactionPriceRounding; }
        }
    }
}