using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to buy shares to cover a previous short sell.
    /// </summary>
    [Serializable]
    public sealed class BuyToCover : ShareTransaction
    {
        /// <summary>
        /// Constructs a BuyToCover ShareTransaction.
        /// </summary>
        /// <param name="date">The date and time this ShareTransaction took place.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the ShareTransaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        /// <param name="commission">The optional commission paid for this ShareTransaction. Default = $0.00</param>
        public BuyToCover(DateTime date, string ticker, decimal price, double shares, decimal commission)
            : base(date, OrderType.BuyToCover, ticker, price, shares, commission)
        {
        }

        /// <summary>
        /// Deserializes a BuyToCover.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private BuyToCover(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the price at which the ShareTransaction took place.
        /// </summary>
        public override decimal Price
        {
            get
            {
                return -1 * base.Price;
            }
        }
    }
}