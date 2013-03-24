using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs <see cref="IHolding"/> objects.
    /// </summary>
    public class HoldingFactory : IHoldingFactory
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
        public IHolding ConstructHolding(decimal shares, decimal openPrice, decimal openCommission, decimal closePrice, decimal closeCommission)
        {
            return new Holding
            {
                Shares = shares,
                OpenPrice = openPrice,
                OpenCommission = openCommission,
                ClosePrice = closePrice,
                CloseCommission = closeCommission
            };

        }

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
        public IHolding ConstructHolding(string ticker, DateTime head, DateTime tail, decimal shares, decimal openPrice, decimal closePrice)
        {
            return new Holding
                {
                    Ticker = ticker,
                    Head = head,
                    Tail = tail,
                    Shares = shares,
                    OpenPrice = openPrice,
                    ClosePrice = closePrice,
                };
        }

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
        public IHolding ConstructHolding(string ticker, DateTime head, DateTime tail, decimal shares, decimal openPrice, decimal openCommission, decimal closePrice, decimal closeCommission)
        {
            return new Holding
                {
                    Ticker = ticker,
                    Head = head,
                    Tail = tail,
                    Shares = shares,
                    OpenPrice = openPrice,
                    OpenCommission = openCommission,
                    ClosePrice = closePrice,
                    CloseCommission = closeCommission
                };
        }
    }
}
