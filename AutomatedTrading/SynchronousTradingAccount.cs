using System;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public abstract class SynchronousTradingAccount : TradingAccount
    {
        /// <summary>
        /// Attempts to cancel an <see cref="IOrder"/> before it is filled.
        /// </summary>
        /// <param name="order">The <see cref="IOrder"/> to attempt to cancel.</param>
        public override void TryCancelOrder(IOrder order)
        {
            throw new NotSupportedException("Order cancellation is not supported by SynchronousTradingAccount.");
        }
    }
}