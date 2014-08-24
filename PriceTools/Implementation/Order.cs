using System;
using System.Globalization;

namespace Sonneville.PriceTools.Implementation
{
    [Serializable]
    public sealed class Order
    {
        public DateTime Issued { get; set; }

        public DateTime Expiration { get; internal set; }

        public string Ticker { get; set; }

        public decimal Price { get; set; }

        public OrderType OrderType { get; set; }

        public PricingType PricingType { get; internal set; }

        public decimal Shares { get; set; }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}: {1} {2} shares of {3} at {4}", Issued, OrderType, Shares, Ticker, Price);
        }
    }
}