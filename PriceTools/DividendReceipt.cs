using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for receipt of dividends.
    /// </summary>
    public sealed partial class DividendReceipt : ShareTransaction
    {
        /// <summary>
        /// Constructs a DividendReceipt ShareTransaction.
        /// </summary>
        public DividendReceipt()
        {
            OrderType = OrderType.DividendReceipt;
        }

        /// <summary>
        /// Constructs a DividendReceipt ShareTransaction.
        /// </summary>
        /// <param name="date">The date and time this ShareTransaction took place.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the ShareTransaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        public DividendReceipt(DateTime date, string ticker, decimal price, double shares)
            : base(date, OrderType.DividendReceipt, ticker, price, shares, 0.00m)
        {
        }
    }
}