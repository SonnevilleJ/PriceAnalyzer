using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools.PortfolioData
{
    public static class TestPortfolios
    {
        /// <summary>
        /// Consists of 6 months of trading activity investing in 4 stocks.
        /// </summary>
        public static Portfolio FidelityTransactions
        {
            get { return PortfolioFactory.ConstructPortfolio(TestTransactionHistory.FidelityTransactions, "FTEXX"); }
        }

        /// <summary>
        /// Consists of 15 months of trading activity in a retirement account.
        /// </summary>
        public static Portfolio BrokerageLink_trades
        {
            get { return PortfolioFactory.ConstructPortfolio(TestTransactionHistory.BrokerageLink_trades, "FDRXX"); }
        }
    }
}