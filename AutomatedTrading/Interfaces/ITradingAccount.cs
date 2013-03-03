using System;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// A brokerage account used for trading financial securities.
    /// </summary>
    public interface ITradingAccount
    {
        /// <summary>
        /// The portfolio of transactions recorded by this TradingAccount.
        /// </summary>
        IPortfolio Portfolio { get; }

        /// <summary>
        /// Gets the list of features supported by this TradingAccount.
        /// </summary>
        TradingAccountFeatures Features { get; }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="IOrder"/> to execute.</param>
        void Submit(IOrder order);

        /// <summary>
        /// Attempts to cancel an <see cref="IOrder"/> before it is filled.
        /// </summary>
        /// <param name="order">The <see cref="IOrder"/> to attempt to cancel.</param>
        void TryCancelOrder(IOrder order);

        /// <summary>
        /// Triggered when an order has been filled.
        /// </summary>
        event EventHandler<OrderExecutedEventArgs> OrderFilled;

        /// <summary>
        /// Triggered when an order has expired.
        /// </summary>
        event EventHandler<OrderExpiredEventArgs> OrderExpired;

        /// <summary>
        /// Triggered when an order has been cancelled.
        /// </summary>
        event EventHandler<OrderCancelledEventArgs> OrderCancelled;
    }
}