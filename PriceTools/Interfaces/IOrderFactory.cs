using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public interface IOrderFactory
    {
        /// <summary>
        /// Constructs a new <see cref="Order"/> object from parameters.
        /// </summary>
        /// <param name="issued"></param>
        /// <param name="expiration"></param>
        /// <param name="orderType"></param>
        /// <param name="ticker"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        Order ConstructOrder(DateTime issued, DateTime expiration, OrderType orderType, string ticker, decimal shares, decimal price);

        /// <summary>
        /// Constructs a new <see cref="Order"/> object from parameters.
        /// </summary>
        /// <param name="issued"></param>
        /// <param name="expiration"></param>
        /// <param name="orderType"></param>
        /// <param name="ticker"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <param name="pricingType"></param>
        /// <returns></returns>
        Order ConstructOrder(DateTime issued, DateTime expiration, OrderType orderType, string ticker, decimal shares, decimal price, PricingType pricingType);
    }
}