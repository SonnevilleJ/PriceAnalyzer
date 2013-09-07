using System;
using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading;

namespace Sonneville.PriceTools.Fidelity
{
    public class FidelityCommissionSchedule : FlatCommissionSchedule
    {
        private readonly IDictionary<Tuple<OrderType>, decimal> _prices = new Dictionary<Tuple<OrderType>, decimal>();

        public FidelityCommissionSchedule()
            : base(7.95m)
        {
        }
    }
}