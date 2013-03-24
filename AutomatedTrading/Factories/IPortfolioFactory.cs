using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IPortfolioFactory
    {
        /// <summary>
        /// Constructs a Portfolio.
        /// </summary>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="IPortfolio"/>.</param>
        IPortfolio ConstructPortfolio(params ITransaction[] transactions);

        /// <summary>
        /// Constructs a Portfolio from a <see cref="ISecurityBasket"/>.
        /// </summary>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="IPortfolio"/>.</param>
        IPortfolio ConstructPortfolio(IEnumerable<ITransaction> transactions);

        /// <summary>
        /// Constructs a Portfolio and assigns a ticker symbol to use as the Portfolio's <see cref="ICashAccount"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="ICashAccount"/>.</param>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="IPortfolio"/>.</param>
        IPortfolio ConstructPortfolio(string ticker, params ITransaction[] transactions);

        /// <summary>
        /// Constructs a Portfolio from a <see cref="ISecurityBasket"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="ICashAccount"/>.</param>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="IPortfolio"/>.</param>
        IPortfolio ConstructPortfolio(string ticker, IEnumerable<ITransaction> transactions);

        /// <summary>
        /// Constructs a Portfolio with an opening deposit.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="IPortfolio"/>.</param>
        IPortfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, params ITransaction[] transactions);

        /// <summary>
        /// Constructs a Portfolio with an opening deposit.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="IPortfolio"/>.</param>
        IPortfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, IEnumerable<ITransaction> transactions);

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="IPortfolio"/>.</param>
        IPortfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit, params ITransaction[] transactions);

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="ITransaction"/>s currently in the <see cref="IPortfolio"/>.</param>
        IPortfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit, IEnumerable<ITransaction> transactions);
    }
}