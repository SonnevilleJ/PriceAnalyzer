using System;
using System.Globalization;

namespace Sonneville.PriceTools.Implementation
{
    [Serializable]
    public sealed class Order : IEquatable<Order>
    {
        public DateTime Issued { get; set; }

        public DateTime Expiration { get; set; }

        public string Ticker { get; set; }

        public decimal Price { get; set; }

        public OrderType OrderType { get; set; }

        public PricingType PricingType { get; set; }

        public decimal Shares { get; set; }

        public bool Equals(Order other)
        {
            return other.Ticker == Ticker
                   && other.Price == Price
                   && other.Shares == Shares
                   && other.OrderType == OrderType
                   && other.Expiration == Expiration
                   && other.Issued == Issued
                   && other.PricingType == PricingType;
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}: {1} {2} shares of {3} at {4}", Issued, OrderType, Shares, Ticker, Price);
        }
    }
}