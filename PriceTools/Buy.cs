using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to buy shares.
    /// </summary>
    [Serializable]
    public sealed class Buy : ShareTransaction
    {
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

        /// <summary>
        /// Deserializes a Buy.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private Buy(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Serializes a Buy.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}