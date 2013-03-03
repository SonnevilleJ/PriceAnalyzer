using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public static Portfolio ConstructPortfolio(params ITransaction[] transactions)
        {
            return ConstructPortfolio(transactions.AsEnumerable());
        }

        /// <summary>
        /// Constructs a Portfolio from a <see cref="SecurityBasket"/>.
        /// </summary>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public static Portfolio ConstructPortfolio(IEnumerable<ITransaction> transactions)
        {
            return ConstructPortfolio(DefaultCashTicker, transactions);
        }

        /// <summary>
        /// Constructs a Portfolio and assigns a ticker symbol to use as the Portfolio's <see cref="CashAccount"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="CashAccount"/>.</param>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public static Portfolio ConstructPortfolio(string ticker, params ITransaction[] transactions)
        {
            return ConstructPortfolio(ticker, transactions.AsEnumerable());
        }

        /// <summary>
        /// Constructs a Portfolio from a <see cref="SecurityBasket"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="CashAccount"/>.</param>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public static Portfolio ConstructPortfolio(string ticker, IEnumerable<ITransaction> transactions)
        {
            var portfolio = new PortfolioImpl(ticker);

            if (transactions == null) throw new ArgumentNullException("transactions", Strings.PortfolioFactory_ConstructPortfolio_Parameter_transactions_cannot_be_null_);
            foreach (var transaction in transactions)
            {
                portfolio.AddTransaction(transaction);
            }
            return portfolio;
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public static Portfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, params ITransaction[] transactions)
        {
            return ConstructPortfolio(dateTime, openingDeposit, transactions.AsEnumerable());
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public static Portfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, IEnumerable<ITransaction> transactions)
        {
            return ConstructPortfolio(DefaultCashTicker, dateTime, openingDeposit, transactions.AsEnumerable());
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public static Portfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit, params ITransaction[] transactions)
        {
            return ConstructPortfolio(ticker, dateTime, openingDeposit, transactions.AsEnumerable());
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public static Portfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit, IEnumerable<ITransaction> transactions)
        {
            var deposit = new Transaction[] {TransactionFactory.ConstructDeposit(dateTime, openingDeposit)};
            var concat = deposit.Concat(transactions);
            return ConstructPortfolio(ticker, concat);
        }
    }
}
