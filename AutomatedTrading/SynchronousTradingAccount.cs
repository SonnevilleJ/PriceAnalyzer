using System;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public abstract class SynchronousTradingAccount : TradingAccount
    {
        /// <summary>
        /// Attempts to cancel an <see cref="Order"/> before it is filled.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to attempt to cancel.</param>
        public override void TryCancelOrder(Order order)
        {
            throw new NotSupportedException("Order cancellation is not supported by SynchronousTradingAccount.");
        }
    }
}