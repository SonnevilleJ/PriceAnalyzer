using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.AutomatedTrading.Fidelity
{
    public class FidelityCommissionSchedule : FlatCommissionSchedule
    {
        #region Private Members

        private readonly IDictionary<Tuple<OrderType>, decimal> _prices = new Dictionary<Tuple<OrderType>, decimal>();

        #endregion

        #region Constructors

        public FidelityCommissionSchedule()
            : base(7.95m)
        {
        }

        #endregion
    }
}