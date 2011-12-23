using System;
using System.Linq;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// An order to take a position on a financial security.
    /// </summary>
    public sealed class Order
    {
        public Order(DateTime issued, DateTime expiration, OrderType orderType, string ticker, double shares, decimal price, PricingType pricingType = PricingType.Market)
        {
            if (orderType == OrderType.Deposit || orderType == OrderType.Withdrawal || orderType == OrderType.DividendReceipt || orderType == OrderType.DividendReinvestment)
                throw new ArgumentOutOfRangeException("orderType", orderType, Strings.Order_Order_OrderType_must_be_Buy__Sell__SellShort__or_BuyToClose_);
            if (!Enum.GetValues(typeof(OrderType)).Cast<OrderType>().Contains(orderType))
                throw new ArgumentOutOfRangeException("orderType", orderType, Strings.Order_Order_Invalid_OrderType_);
            if (price < 0)
                throw new ArgumentOutOfRangeException("price", price, Strings.Order_Validate_Price_must_be_a_positive_number_);
            if (shares < 0)
                throw new ArgumentOutOfRangeException("shares", shares, Strings.Order_Validate_Shares_must_be_a_positive_number_);
            if (!Enum.GetValues(typeof(PricingType)).Cast<PricingType>().Contains(pricingType))
                throw new ArgumentOutOfRangeException("pricingType", pricingType, Strings.Order_Order_Invalid_PricingType_);

            Issued = issued;
            Expiration = expiration;
            OrderType = orderType;
            Ticker = ticker;
            Price = price;
            PricingType = pricingType == PricingType.Stop ? PricingType.StopMarket : pricingType;
            Shares = shares;
        }

        /// <summary>
        /// The DateTime this order was issued.
        /// </summary>
        public DateTime Issued { get; private set; }

        /// <summary>
        /// The DateTime this order expires.
        /// </summary>
        public DateTime Expiration { get; private set; }

        /// <summary>
        /// The ticker symbol for this order.
        /// </summary>
        public string Ticker { get; private set; }

        /// <summary>
        /// The price at which the order should be executed.
        /// </summary>
        public decimal Price { get; private set; }

        /// <summary>
        /// Specifies the type of order.
        /// </summary>
        public OrderType OrderType { get; private set; }

        /// <summary>
        /// The <see cref="PricingType"/> (market or limit) which should be used when submitting this order.
        /// </summary>
        public PricingType PricingType { get; private set; }

        /// <summary>
        /// The number of shares for this order.
        /// </summary>
        public double Shares { get; private set; }
    }
}