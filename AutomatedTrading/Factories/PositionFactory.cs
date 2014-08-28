using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class PositionFactory : IPositionFactory
    {
        private readonly PriceSeriesFactory _priceSeriesFactory;
        private readonly SecurityBasketCalculator _securityBasketCalculator;

        public PositionFactory()
        {
            _priceSeriesFactory = new PriceSeriesFactory();
            _securityBasketCalculator = new SecurityBasketCalculator();
        }

        public Position ConstructPosition(string ticker, params ShareTransaction[] transactions)
        {
            return ConstructPosition(ticker, transactions.AsEnumerable());
        }

        public Position ConstructPosition(string ticker, IEnumerable<ShareTransaction> transactions)
        {
            var position = new Position(ticker);
            foreach (var transaction in transactions)
            {
                position.AddTransaction(transaction);
            }
            return position;
        }

        public IPriceSeries ConstructPriceSeries(IPosition position, IPriceDataProvider priceDataProvider)
        {
            var underlyingPriceSeries = _priceSeriesFactory.ConstructPriceSeries(position.Ticker);
            priceDataProvider.UpdatePriceSeries(underlyingPriceSeries, position.Head, position.Tail, Resolution.Days);

            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(position.Ticker);
            for (var date = position.Head; date <= position.Tail; date = date.NextPeriodOpen(Resolution.Days))
            {
                var heldShares = _securityBasketCalculator.GetHeldShares(
                    position.Transactions.Where(transaction => transaction is ShareTransaction).Cast<ShareTransaction>(),
                    date);
                if(heldShares!= 0)
                {
                    priceSeries.AddPriceData(new PricePeriod(date, date.CurrentPeriodClose(Resolution.Days), underlyingPriceSeries[date]*heldShares));
                }
            }
            return priceSeries;
        }
    }
}
