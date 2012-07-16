namespace Sonneville.PriceTools.SamplePortfolioData
{
    public static class SamplePortfolios
    {
        /// <summary>
        /// Consists of 6 months of trading activity investing in 4 stocks.
        /// </summary>
        public static Portfolio FidelityTransactions
        {
            get { return PortfolioFactory.ConstructPortfolio(TransactionHistory.FidelityTransactions, "FTEXX"); }
        }

        /// <summary>
        /// Consists of 15 months of trading activity in a retirement account.
        /// </summary>
        public static Portfolio BrokerageLink_trades
        {
            get { return PortfolioFactory.ConstructPortfolio(TransactionHistory.BrokerageLink_trades, "FDRXX"); }
        }
        
        /// <summary>
        /// Consists of a single buy transaction. Used to test for rounding errors when calculating invested capital.
        /// </summary>
        public static Portfolio BrokerageLink_TransactionPriceRounding
        {
            get { return PortfolioFactory.ConstructPortfolio(TransactionHistory.BrokerageLink_TransactionPriceRounding); }
        }
    }
}