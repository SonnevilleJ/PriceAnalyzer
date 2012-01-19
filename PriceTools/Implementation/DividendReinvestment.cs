using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to reinvest dividends.
    /// </summary>
    [Serializable]
    internal sealed class DividendReinvestment : ShareTransaction, IDividendReinvestment
    {
        /// <summary>
        /// Constructs a DividendReinvestment Transaction.
        /// </summary>
        internal DividendReinvestment()
        {
            OrderType = OrderType.DividendReinvestment;
        }
    }
}