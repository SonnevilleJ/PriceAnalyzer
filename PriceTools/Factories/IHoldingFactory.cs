using System;

namespace Sonneville.PriceTools
{
    public interface IHoldingFactory
    {
        /// <summary>
        /// Constructs a new <see cref="IHolding"/> object.
        /// </summary>
        /// <param name="shares"></param>
        /// <param name="openPrice"></param>
        /// <param name="openCommission"></param>
        /// <param name="closePrice"></param>
        /// <param name="closeCommission"></param>
        /// <returns></returns>
        IHolding ConstructHolding(decimal shares, decimal openPrice, decimal openCommission, decimal closePrice, decimal closeCommission);

        /// <summary>
        /// Constructs a new <see cref="IHolding"/> object.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="head"></param>
        /// <param name="tail"></param>
        /// <param name="shares"></param>
        /// <param name="openPrice"></param>
        /// <param name="closePrice"></param>
        /// <returns></returns>
        IHolding ConstructHolding(string ticker, DateTime head, DateTime tail, decimal shares, decimal openPrice, decimal closePrice);

        /// <summary>
        /// Constructs a new <see cref="IHolding"/> object.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="head"></param>
        /// <param name="tail"></param>
        /// <param name="shares"></param>
        /// <param name="openPrice"></param>
        /// <param name="openCommission"></param>
        /// <param name="closePrice"></param>
        /// <param name="closeCommission"></param>
        /// <returns></returns>
        IHolding ConstructHolding(string ticker, DateTime head, DateTime tail, decimal shares, decimal openPrice, decimal openCommission, decimal closePrice, decimal closeCommission);
    }
}