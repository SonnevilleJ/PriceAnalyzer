using System;
using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
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
            return ConstructPortfolio(DefaultCashTicker, dateTime, openingDeposit);
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        public static Portfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit)
        {
            var portfolio = ConstructPortfolio(ticker);
            portfolio.Deposit(dateTime, openingDeposit);
            return portfolio;
        }

        /// <summary>
        /// Constructs a Portfolio from a <see cref="SecurityBasket"/>.
        /// </summary>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public static Portfolio ConstructPortfolio(IEnumerable<Transaction> transactions)
        {
            var portfolio = ConstructPortfolio(DefaultCashTicker, transactions);
            return portfolio;
        }

        /// <summary>
        /// Constructs a Portfolio from a <see cref="SecurityBasket"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="CashAccount"/>.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public static Portfolio ConstructPortfolio(string ticker, IEnumerable<Transaction> transactions)
        {
            var portfolio = new PortfolioImpl(ticker);
            portfolio.AddTransactions(transactions);
            return portfolio;
        }
    }
}
