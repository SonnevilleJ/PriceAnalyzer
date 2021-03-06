﻿using System;
using System.Linq;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public class OrderFactory : IOrderFactory
    {
        public Order ConstructOrder(DateTime issued, DateTime expiration, OrderType orderType, string ticker, decimal shares, decimal price)
        {
            return ConstructOrder(issued, expiration, orderType, ticker, shares, price, PricingType.Market);
        }

        public Order ConstructOrder(DateTime issued, DateTime expiration, OrderType orderType, string ticker, decimal shares, decimal price, PricingType pricingType)
        {
            if (issued >= expiration)
                throw new ArgumentOutOfRangeException("expiration", expiration, Strings.Order_Order_Cannot_create_an_Order_with_an_expiration_date_before_the_issue_date_);
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

            return new Order
                       {
                           Issued = issued,
                           Expiration = expiration,
                           OrderType = orderType,
                           Ticker = ticker,
                           Shares = shares,
                           Price = price,
                           PricingType = pricingType == PricingType.Stop ? PricingType.StopMarket : pricingType
                       };
        }
    }
}
