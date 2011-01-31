using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to sell shares.
    /// </summary>
    [Serializable]
    public sealed class Sell : ShareTransaction
    {
        /// <summary>
        /// Constructs a Sell ShareTransaction.
        /// </summary>
        /// <param name="date">The date and time this ShareTransaction took place.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the ShareTransaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        /// <param name="commission">The optional commission paid for this ShareTransaction. Default = $0.00</param>
        public Sell(DateTime date, string ticker, decimal price, double shares, decimal commission)
            : base(date, OrderType.Sell, ticker, price, shares, commission)
        {
        }

        /// <summary>
        /// Deserializes a Sell.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private Sell(SerializationInfo info, StreamingContext context)
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