using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A trading strategy which issues orders to buy and sell a security.
    /// </summary>
    public abstract class TradingStrategy
    {
        private IList<IPricePeriod> _pricePeriods;

        protected DateTime StartDateTime { get; set; }

        public IPriceSeries PriceSeries { get; set; }

        public ITradingAccount TradingAccount { get; set; }

        public void Start()
        {
            if (TradingAccount == null)
                throw new InvalidOperationException("A TradingAccount is required before executing the TradingStrategy.");
            if(PriceSeries == null)
                throw new InvalidOperationException("A PriceSeries is required before executing the TradingStrategy.");

            ProcessAllPeriods();
        }

        public void Stop()
        {
            throw new NotSupportedException();
        }

        protected void ProcessPeriod(int index)
        {
            var period = _pricePeriods[index];

            const bool timeToBuy = true;

            if (timeToBuy)
            {
                var order = CreateBuyOrder(period.Tail);

                TradingAccount.Submit(order);
            }
        }

        private void ProcessAllPeriods()
        {
            _pricePeriods = PriceSeries.GetPricePeriods(PriceSeries.Resolution, StartDateTime, DateTime.Now);

            for (var i = 0; i < _pricePeriods.Count; i++)
            {
                ProcessPeriod(i);
            }
        }

        private Order CreateBuyOrder(DateTime issued)
        {
            var expiration = issued.GetFollowingClose();
            var orderType = OrderType.Buy;
            var ticker = PriceSeries.Ticker;
            var shares = 5;
            var price = 100.00m;

            return new Order(issued, expiration, orderType, ticker, shares, price);
        }
    }
}
