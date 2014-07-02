using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// Constructs <see cref="Position"/> objects.
    /// </summary>
    public class PositionFactory : IPositionFactory
    {
        private readonly PriceSeriesFactory _priceSeriesFactory;
        private readonly PricePeriodFactory _pricePeriodFactory;
        private readonly SecurityBasketCalculator _securityBasketCalculator;

        public PositionFactory()
        {
            _priceSeriesFactory = new PriceSeriesFactory();
            _pricePeriodFactory = new PricePeriodFactory();
            _securityBasketCalculator = new SecurityBasketCalculator();
        }

        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker of the security held in this Position.</param>
        /// <param name="transactions">An optional list of <see cref="ShareTransaction"/>s previously in the Position.</param>
        public Position ConstructPosition(string ticker, params ShareTransaction[] transactions)
        {
            return ConstructPosition(ticker, transactions.AsEnumerable());
        }

        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker of the security held in this Position.</param>
        /// <param name="transactions">A list of <see cref="ShareTransaction"/>s previously in the Position.</param>
        public Position ConstructPosition(string ticker, IEnumerable<ShareTransaction> transactions)
        {
            var position = new Position(ticker);
            foreach (var transaction in transactions)
            {
                position.AddTransaction(transaction);
            }
            return position;
        }

        public IPriceSeries ConstructPriceSeries(Position position, IPriceDataProvider priceDataProvider)
        {
            var underlyingPriceSeries = _priceSeriesFactory.ConstructPriceSeries(position.Ticker);
            priceDataProvider.UpdatePriceSeries(underlyingPriceSeries, position.Head, position.Tail, Resolution.Days);

            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(position.Ticker);
            foreach (var pricePeriod in underlyingPriceSeries.PricePeriods)
            {
                var heldShares = _securityBasketCalculator.GetHeldShares(
                    position.Transactions.Where(transaction => transaction is ShareTransaction).Cast<ShareTransaction>(),
                    pricePeriod.Head);
                if(heldShares!= 0)
                {
                    priceSeries.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(
                    pricePeriod.Head,
                    pricePeriod.Head.CurrentPeriodClose(Resolution.Days),
                    underlyingPriceSeries[pricePeriod.Head]*heldShares));
                }
            }
            return priceSeries;
        }
    }
}
