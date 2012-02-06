using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A <see cref="PricePeriodImpl"/> made from <see cref="PriceTicks"/>.
    /// </summary>
    internal class TickedPricePeriodImpl : PricePeriodImpl, TickedPricePeriod
    {
        private readonly List<PriceTick> _priceTicks = new List<PriceTick>();
        
        /// <summary>
        /// The <see cref="PriceTickImpl"/>s contained within this TickedPricePeriod.
        /// </summary>
        public IList<PriceTick> PriceTicks { get { return _priceTicks.AsReadOnly(); } }

        #region Overrides of PricePeriod

        /// <summary>
        ///   Gets the total volume of trades during the PricePeriod.
        /// </summary>
        public override long? Volume
        {
            get { return PriceTicks.Sum(q => q.Volume); }
        }

        /// <summary>
        ///   Gets the first DateTime in the TimeSeries.
        /// </summary>
        public override DateTime Head
        {
            get { return PriceTicks.Min(q => q.SettlementDate); }
        }

        /// <summary>
        ///   Gets the last DateTime in the TimeSeries.
        /// </summary>
        public override DateTime Tail
        {
            get { return PriceTicks.Max(q => q.SettlementDate); }
        }

        /// <summary>
        /// Gets the values stored within the TimeSeries.
        /// </summary>
        public override IDictionary<DateTime, decimal> Values
        {
            get
            {
                return PriceTicks.ToDictionary(tick => tick.SettlementDate, tick => tick.Price);
            }
        }

        /// <summary>
        ///   Adds one or more <see cref = "PriceTick" />s to the PriceSeries.
        /// </summary>
        /// <param name = "priceTicks">The <see cref = "PriceTick" />s to add.</param>
        public void AddPriceTicks(params PriceTick[] priceTicks)
        {
            foreach (var quote in priceTicks)
            {
                _priceTicks.Add(quote);
            }
            var args = new NewPriceDataAvailableEventArgs
                           {
                               Head = priceTicks.Min(quote => quote.SettlementDate),
                               Tail = priceTicks.Max(quote => quote.SettlementDate)
                           };
            InvokeNewPriceDataAvailable(args);
        }

        /// <summary>
        ///   Gets the closing price for the PricePeriod.
        /// </summary>
        public override decimal Close
        {
            get { return PriceTicks.OrderBy(q => q.SettlementDate).Last().Price; }
        }

        /// <summary>
        ///   Gets the highest price that occurred during the PricePeriod.
        /// </summary>
        public override decimal High
        {
            get { return PriceTicks.Max(q => q.Price); }
        }

        /// <summary>
        ///   Gets the lowest price that occurred during the PricePeriod.
        /// </summary>
        public override decimal Low
        {
            get { return PriceTicks.Min(q => q.Price); }
        }

        /// <summary>
        ///   Gets the opening price for the PricePeriod.
        /// </summary>
        public override decimal Open
        {
            get { return PriceTicks.OrderBy(q => q.SettlementDate).First().Price; }
        }

        /// <summary>
        ///   Gets a value stored at a given DateTime index of the PricePeriod.
        /// </summary>
        /// <param name = "index">The DateTime of the desired value.</param>
        /// <returns>The value of the TimeSeries as of the given DateTime.</returns>
        public override decimal this[DateTime index]
        {
            get { return PriceTicks.Where(q => q.SettlementDate <= index).Last().Price; }
        }

        #endregion
    }
}