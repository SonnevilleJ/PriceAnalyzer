using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.Trading
{
    public class FidelityCommissionSchedule : ICommissionSchedule
    {
        #region Private Members

        private readonly IDictionary<Tuple<OrderType>, decimal> _prices = new Dictionary<Tuple<OrderType>, decimal>();

        #endregion

        public decimal PriceCheck(Order order)
        {
            var price = 0.00m;
            if (_prices.TryGetValue(new Tuple<OrderType>(order.OrderType), out price))
            {
                return price;
            }
            return price;
        }
    }
}
