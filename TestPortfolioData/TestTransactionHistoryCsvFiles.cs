namespace Test.Sonneville.PriceTools.PortfolioData
{
    public static class TestTransactionHistoryCsvFiles
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