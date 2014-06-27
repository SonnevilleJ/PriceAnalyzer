using System;
using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IPortfolioFactory
    {
        /// <summary>
        /// Constructs a Portfolio.
        /// </summary>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        Portfolio ConstructPortfolio(params Transaction[] transactions);

        /// <summary>
        /// Constructs a Portfolio from a <see cref="ISecurityBasket"/>.
        /// </summary>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        Portfolio ConstructPortfolio(IEnumerable<Transaction> transactions);

        /// <summary>
        /// Constructs a Portfolio and assigns a ticker symbol to use as the Portfolio's <see cref="ICashAccount"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="ICashAccount"/>.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        Portfolio ConstructPortfolio(string ticker, params Transaction[] transactions);

        /// <summary>
        /// Constructs a Portfolio from a <see cref="ISecurityBasket"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="ICashAccount"/>.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        Portfolio ConstructPortfolio(string ticker, IEnumerable<Transaction> transactions);

        /// <summary>
        /// Constructs a Portfolio with an opening deposit.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        Portfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, params Transaction[] transactions);

        /// <summary>
        /// Constructs a Portfolio with an opening deposit.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        Portfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, IEnumerable<Transaction> transactions);

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        Portfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit, params Transaction[] transactions);

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        Portfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit, IEnumerable<Transaction> transactions);

        IPriceSeries ConstructPriceSeries(Portfolio portfolio, IPriceDataProvider priceDataProvider);
    }
}