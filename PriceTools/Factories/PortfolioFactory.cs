using System;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs Portfolio objects.
    /// </summary>
    public static class PortfolioFactory
    {
        /// <summary>
        /// Constructs a Portfolio.
        /// </summary>
        /// <returns></returns>
        public static Portfolio ConstructPortfolio()
        {
            return new PortfolioImpl();
        }

        /// <summary>
        /// Constructs a Portfolio and assigns a ticker symbol to use as the Portfolio's <see cref="CashAccount"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="CashAccount"/>.</param>
        public static Portfolio ConstructPortfolio(string ticker)
        {
            return new PortfolioImpl(ticker);
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        public static Portfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit)
        {
            return new PortfolioImpl(dateTime, openingDeposit);
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        public static Portfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, string ticker)
        {
            return new PortfolioImpl(dateTime, openingDeposit, ticker);
        }

        /// <summary>
        /// Constructs a Portfolio from a <see cref="ITransactionHistory"/>.
        /// </summary>
        /// <param name="csvFile">The <see cref="ITransactionHistory"/> containing transaction data.</param>
        public static Portfolio ConstructPortfolio(ITransactionHistory csvFile)
        {
            return new PortfolioImpl(csvFile);
        }

        /// <summary>
        /// Constructs a Portfolio from a <see cref="ITransactionHistory"/>.
        /// </summary>
        /// <param name="csvFile">The <see cref="ITransactionHistory"/> containing transaction data.</param>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="CashAccount"/>.</param>
        public static Portfolio ConstructPortfolio(ITransactionHistory csvFile, string ticker)
        {
            return new PortfolioImpl(csvFile, ticker);
        }
    }
}
