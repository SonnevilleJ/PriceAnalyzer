using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to reinvest dividends.
    /// </summary>
    public sealed partial class DividendReinvestment : ShareTransaction
    {
        /// <summary>
        /// Constructs a DividendReinvestment ShareTransaction.
        /// </summary>
        public DividendReinvestment()
        {
            OrderType = OrderType.DividendReinvestment;
        }

        /// <summary>
        /// Constructs a DividendReinvestment ShareTransaction.
        /// </summary>
        /// <param name="date">The date and time this ShareTransaction took place.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the ShareTransaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        public DividendReinvestment(DateTime date, string ticker, decimal price, double shares)
            : base(date, OrderType.DividendReinvestment, ticker, price, shares, 0.00m)
        {
        }
    }
}