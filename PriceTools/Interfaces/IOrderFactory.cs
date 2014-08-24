using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public interface IOrderFactory
    {
        Order ConstructOrder(DateTime issued, DateTime expiration, OrderType orderType, string ticker, decimal shares, decimal price);

        Order ConstructOrder(DateTime issued, DateTime expiration, OrderType orderType, string ticker, decimal shares, decimal price, PricingType pricingType);
    }
}