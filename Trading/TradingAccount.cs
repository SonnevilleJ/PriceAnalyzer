using System;
using System.Collections.ObjectModel;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A trading account which communicates with a brokerage to perform execution of orders.
    /// </summary>
    public abstract class TradingAccount
    {
        /// <summary>
        /// A list of <see cref="IPosition"/>s currently held in this account.
        /// </summary>
        public ReadOnlyCollection<IPosition> Positions { get; private set; }

        /// <summary>
        /// Initiates a market order to buy a security.
        /// </summary>
        /// <param name="ticker">The security to buy.</param>
        /// <param name="shares">The number of shares to buy.</param>
        public abstract void Buy(string ticker, double shares);

        /// <summary>
        /// Initiates a limit order to buy a security.
        /// </summary>
        /// <param name="ticker">The security to buy.</param>
        /// <param name="shares">The number of shares to buy.</param>
        /// <param name="limitPrice">The limit price at which to buy.</param>
        public abstract void Buy(string ticker, double shares, decimal limitPrice);

        /// <summary>
        /// Initiates a market order to sell a security.
        /// </summary>
        /// <param name="ticker">The security to sell.</param>
        /// <param name="shares">The number of shares to sell.</param>
        public abstract void Sell(string ticker, double shares);

        /// <summary>
        /// Initiates a limit order to sell a security.
        /// </summary>
        /// <param name="ticker">The security to sell.</param>
        /// <param name="shares">The number of shares to sell.</param>
        /// <param name="limitPrice">The limit price at which to sell.</param>
        public abstract void Sell(string ticker, double shares, decimal limitPrice);

        /// <summary>
        /// Triggered when an order has been filled.
        /// </summary>
        public event EventHandler<OrderInfo> OrderFilled;
    }
}
