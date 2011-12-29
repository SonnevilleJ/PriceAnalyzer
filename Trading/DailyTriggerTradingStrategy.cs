﻿using System;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceTools.Trading
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

            return new Order(issued, expiration, orderType, ticker, shares, price);
        }
    }
}