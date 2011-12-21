using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.Trading.Fidelity
{
    public class FidelityCommissionSchedule : ICommissionSchedule
    {
        #region Private Members

        private readonly IDictionary<Tuple<OrderType>, decimal> _prices = new Dictionary<Tuple<OrderType>, decimal>();

        #endregion

        public decimal PriceCheck(Order order)
        {
            return 7.95m;
        }
    }
}
