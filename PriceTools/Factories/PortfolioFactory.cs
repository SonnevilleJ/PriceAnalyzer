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
        private static readonly string DefaultCashTicker = String.Empty;

        /// <summary>
        /// Constructs a Portfolio.
        /// </summary>
        /// <returns></returns>
        public static Portfolio ConstructPortfolio()
        {
            return ConstructPortfolio(DefaultCashTicker);
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
            return ConstructPortfolio(dateTime, openingDeposit, DefaultCashTicker);
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        public static Portfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, string ticker)
        {
            var portfolio = ConstructPortfolio(ticker);
            portfolio.Deposit(dateTime, openingDeposit);
            return portfolio;
        }

        /// <summary>
        /// Constructs a Portfolio from a <see cref="TransactionHistory"/>.
        /// </summary>
        /// <param name="csvFile">The <see cref="TransactionHistory"/> containing transaction data.</param>
        public static Portfolio ConstructPortfolio(TransactionHistory csvFile)
        {
            return ConstructPortfolio(csvFile, DefaultCashTicker);
        }

        /// <summary>
        /// Constructs a Portfolio from a <see cref="TransactionHistory"/>.
        /// </summary>
        /// <param name="csvFile">The <see cref="TransactionHistory"/> containing transaction data.</param>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="CashAccount"/>.</param>
        public static Portfolio ConstructPortfolio(TransactionHistory csvFile, string ticker)
        {
            var portfolio = ConstructPortfolio(ticker);
            portfolio.AddTransactionHistory(csvFile);
            return portfolio;
        }
    }
}
