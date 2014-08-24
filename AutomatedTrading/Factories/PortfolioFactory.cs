using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Extensions;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// Constructs Portfolio objects.
    /// </summary>
    public class PortfolioFactory : IPortfolioFactory
    {
        private readonly string _defaultCashTicker = String.Empty;
        private readonly ITransactionFactory _transactionFactory;
        private static readonly PriceSeriesFactory _priceSeriesFactory;

        public PortfolioFactory()
            : this(new TransactionFactory())
        {
        }

        public PortfolioFactory(ITransactionFactory transactionFactory)
        {
            _transactionFactory = transactionFactory;
        }

        static PortfolioFactory()
        {
            _priceSeriesFactory = new PriceSeriesFactory();
        }

        /// <summary>
        /// Constructs a Portfolio.
        /// </summary>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public IPortfolio ConstructPortfolio(params ITransaction[] transactions)
        {
            return ConstructPortfolio(transactions.AsEnumerable());
        }

        /// <summary>
        /// Constructs a Portfolio from a <see cref="ISecurityBasket"/>.
        /// </summary>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public IPortfolio ConstructPortfolio(IEnumerable<ITransaction> transactions)
        {
            return ConstructPortfolio(_defaultCashTicker, transactions);
        }

        /// <summary>
        /// Constructs a Portfolio and assigns a ticker symbol to use as the Portfolio's <see cref="ICashAccount"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="ICashAccount"/>.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public IPortfolio ConstructPortfolio(string ticker, params ITransaction[] transactions)
        {
            return ConstructPortfolio(ticker, transactions.AsEnumerable());
        }

        /// <summary>
        /// Constructs a Portfolio from a <see cref="ISecurityBasket"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="ICashAccount"/>.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public IPortfolio ConstructPortfolio(string ticker, IEnumerable<ITransaction> transactions)
        {
            var portfolio = new Portfolio(ticker);

            if (transactions == null) throw new ArgumentNullException("transactions", Strings.PortfolioFactory_ConstructPortfolio_Parameter_transactions_cannot_be_null_);
            foreach (var transaction in transactions)
            {
                transaction.ApplyToPortfolio(portfolio);
            }
            return portfolio;
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public IPortfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, params ITransaction[] transactions)
        {
            return ConstructPortfolio(dateTime, openingDeposit, transactions.AsEnumerable());
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public IPortfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, IEnumerable<ITransaction> transactions)
        {
            return ConstructPortfolio(_defaultCashTicker, dateTime, openingDeposit, transactions.AsEnumerable());
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public IPortfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit, params ITransaction[] transactions)
        {
            return ConstructPortfolio(ticker, dateTime, openingDeposit, transactions.AsEnumerable());
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="transactions">The list of <see cref="Transaction"/>s currently in the <see cref="Portfolio"/>.</param>
        public IPortfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit, IEnumerable<ITransaction> transactions)
        {
            var deposit = new Transaction[] {_transactionFactory.ConstructDeposit(dateTime, openingDeposit)};
            var concat = deposit.Concat(transactions);
            return ConstructPortfolio(ticker, concat);
        }

        public IPriceSeries ConstructPriceSeries(IPortfolio portfolio, IPriceDataProvider priceDataProvider)
        {
            var result = _priceSeriesFactory.ConstructPriceSeries(string.Empty);
            var positionFactory = new PositionFactory();

            var dictionary = new Dictionary<DateTime, decimal>();
            var cashPriceSeries = portfolio.CashPriceSeries;
            AddToDictionary(cashPriceSeries, dictionary);
            foreach (var position in portfolio.Positions)
            {
                var positionPriceSeries = positionFactory.ConstructPriceSeries(position, priceDataProvider);

                AddToDictionary(positionPriceSeries, dictionary);
            }
            foreach (var keyValuePair in dictionary)
            {
                var period = new PricePeriod(keyValuePair.Key, keyValuePair.Key.CurrentPeriodClose(Resolution.Days), keyValuePair.Value);
                result.AddPriceData(period);
            }
            return result;
        }

        private static void AddToDictionary(IPriceSeries positionPriceSeries, IDictionary<DateTime, decimal> dictionary)
        {
            foreach (var pricePeriod in positionPriceSeries.PricePeriods)
            {
                if (dictionary.ContainsKey(pricePeriod.Head))
                {
                    dictionary[pricePeriod.Head] = dictionary[pricePeriod.Head] + pricePeriod.Close;
                }
                else
                {
                    dictionary.Add(pricePeriod.Head, pricePeriod.Close);
                }
            }
        }
    }
}
