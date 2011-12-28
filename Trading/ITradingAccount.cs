﻿using System;
using System.Collections.ObjectModel;

namespace Sonneville.PriceTools.Trading
{
    public interface ITradingAccount
    {
        /// <summary>
        /// A list of <see cref="IPosition"/>s currently held in this account.
        /// </summary>
        ReadOnlyCollection<IPosition> Positions { get; }

        /// <summary>
        /// Gets the list of features supported by this TradingAccount.
        /// </summary>
        TradingAccountFeatures Features { get; }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        void Submit(Order order);

        /// <summary>
        /// Attempts to cancel an <see cref="Order"/> before it is filled.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to attempt to cancel.</param>
        void TryCancelOrder(Order order);

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

        /// <summary>
        /// Blocks the calling thread until all submitted orders are filled, cancelled, or expired.
        /// </summary>
        void WaitAll();
    }
}