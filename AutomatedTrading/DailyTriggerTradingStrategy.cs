using System;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class DailyTriggerTradingStrategy : TradingStrategy
    {
        protected override Order CreateOrder(DateTime issued)
        {
            var expiration = issued.GetFollowingClose();
            var orderType = OrderType.Buy;
            var ticker = PriceSeries.Ticker;
            var shares = 5;
            var price = 100.00m;

            return Order.Factory.ConstructOrder(issued, expiration, orderType, ticker, shares, price);
        }
    }
}