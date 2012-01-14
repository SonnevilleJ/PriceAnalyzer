namespace SamplePortfolioData
{
    public static class PortfolioCsv
    {
        public static string FidelityTransactions
        {
            get { return FidelityData.FidelityTransactions; }
        }

        public static string BrokerageLink_trades
        {
            get { return FidelityData.BrokerageLink_trades; }
        }

        public static string BrokerageLink_TransactionPriceRounding
        {
            get { return FidelityData.BrokerageLink_TransactionPriceRounding; }
        }
    }
}