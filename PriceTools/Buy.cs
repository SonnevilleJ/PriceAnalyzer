using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to buy shares.
    /// </summary>
    public sealed partial class Buy : ShareTransaction
    {
        /// <summary>
        /// Constructs a Buy ShareTransaction.
        /// </summary>
        public Buy()
        {
            OrderType = OrderType.Buy;
        }

        /// <summary>
        /// Constructs a Buy ShareTransaction.
        /// </summary>
        /// <param name="date">The date and time this ShareTransaction took place.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the ShareTransaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        /// <param name="commission">The optional commission paid for this ShareTransaction. Default = $0.00</param>
        public Buy(DateTime date, string ticker, decimal price, double shares, decimal commission)
            : base(date, OrderType.Buy, ticker, price, shares, commission)
        {
        }
    }
}