using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to reinvest dividends.
    /// </summary>
    [Serializable]
    public sealed class DividendReinvestment : ShareTransaction
    {
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
            if (shares <= 0)
            {
                throw new ArgumentOutOfRangeException("shares", shares, "Dividend shares must be greater than zero.");
            }
            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException("price", price, String.Format(CultureInfo.CurrentCulture, "Price for dividends must be greater than {0}.", 0D));
            }
        }

        /// <summary>
        /// Deserializes a DividendReinvestment.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private DividendReinvestment(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}