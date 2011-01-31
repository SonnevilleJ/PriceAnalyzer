using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to short-sell shares.
    /// </summary>
    [Serializable]
    public sealed class SellShort : ShareTransaction
    {
        /// <summary>
        /// Constructs a SellShort ShareTransaction.
        /// </summary>
        /// <param name="date">The date and time this ShareTransaction took place.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the ShareTransaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        /// <param name="commission">The optional commission paid for this ShareTransaction. Default = $0.00</param>
        public SellShort(DateTime date, string ticker, decimal price, double shares, decimal commission)
            : base(date, OrderType.SellShort, ticker, price, shares, commission)
        {
        }

        /// <summary>
        /// Deserializes a SellShort.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private SellShort(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}