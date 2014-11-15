using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Extensions;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class PortfolioFactory : IPortfolioFactory
    {
        private readonly string _defaultCashTicker = String.Empty;
        private readonly ITransactionFactory _transactionFactory;
        private readonly IPriceSeriesFactory _priceSeriesFactory;
        private readonly ICashAccountFactory _cashAccountFactory;
        private readonly ISecurityBasketCalculator _securityBasketCalculator;
        private readonly PositionFactory _positionFactory;

        public PortfolioFactory(ITransactionFactory transactionFactory, ICashAccountFactory cashAccountFactory, ISecurityBasketCalculator securityBasketCalculator, PositionFactory positionFactory, IPriceSeriesFactory priceSeriesFactory)
        {
            _priceSeriesFactory = priceSeriesFactory;
            _transactionFactory = transactionFactory;
            _cashAccountFactory = cashAccountFactory;
            _securityBasketCalculator = securityBasketCalculator;
            _positionFactory = positionFactory;
        }

        public IPortfolio ConstructPortfolio(params ITransaction[] transactions)
        {
            return ConstructPortfolio(transactions.AsEnumerable());
        }

        public IPortfolio ConstructPortfolio(IEnumerable<ITransaction> transactions)
        {
            return ConstructPortfolio(_defaultCashTicker, transactions);
        }

        public IPortfolio ConstructPortfolio(string ticker, params ITransaction[] transactions)
        {
            return ConstructPortfolio(ticker, transactions.AsEnumerable());
        }

        public IPortfolio ConstructPortfolio(string ticker, IEnumerable<ITransaction> transactions)
        {
            var portfolio = new Portfolio(ticker, _cashAccountFactory, _positionFactory, _securityBasketCalculator, _priceSeriesFactory);

            if (transactions == null) throw new ArgumentNullException("transactions", Strings.PortfolioFactory_ConstructPortfolio_Parameter_transactions_cannot_be_null_);
            foreach (var transaction in transactions)
            {
                transaction.ApplyToPortfolio(portfolio);
            }
            return portfolio;
        }

        public IPortfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, params ITransaction[] transactions)
        {
            return ConstructPortfolio(dateTime, openingDeposit, transactions.AsEnumerable());
        }

        public IPortfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, IEnumerable<ITransaction> transactions)
        {
            return ConstructPortfolio(_defaultCashTicker, dateTime, openingDeposit, transactions.AsEnumerable());
        }

        public IPortfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit, params ITransaction[] transactions)
        {
            return ConstructPortfolio(ticker, dateTime, openingDeposit, transactions.AsEnumerable());
        }

        public IPortfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit, IEnumerable<ITransaction> transactions)
        {
            var deposit = new Transaction[] {_transactionFactory.ConstructDeposit(dateTime, openingDeposit)};
            var concat = deposit.Concat(transactions);
            return ConstructPortfolio(ticker, concat);
        }

        public IPriceSeries ConstructPriceSeries(IPortfolio portfolio, IPriceDataProvider priceDataProvider)
        {
            var result = _priceSeriesFactory.ConstructPriceSeries(string.Empty);
            var positionFactory = _positionFactory;

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
