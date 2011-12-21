﻿using System;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A trading account which communicates with Fidelity to perform execution of orders.
    /// </summary>
    public class FidelityTradingAccount : TradingAccount
    {
        public FidelityTradingAccount(TradingAccountFeatures features)
            : base(features)
        {
        }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        protected override void ProcessOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}