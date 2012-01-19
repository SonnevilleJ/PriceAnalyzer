﻿using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to reinvest dividends.
    /// </summary>
    [Serializable]
    internal sealed class DividendReinvestmentImpl : ShareTransactionImpl, DividendReinvestment
    {
        /// <summary>
        /// Constructs a DividendReinvestment Transaction.
        /// </summary>
        internal DividendReinvestmentImpl()
        {
            OrderType = OrderType.DividendReinvestment;
        }
    }
}