using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A <see cref="PricePeriodImpl"/> made from <see cref="PriceTicks"/>.
    /// </summary>
    public interface TickedPricePeriod : PricePeriod
    {
        /// <summary>
        /// The <see cref="PriceTickImpl"/>s contained within this TickedPricePeriod.
        /// </summary>
        IList<PriceTick> PriceTicks { get; }

        /// <summary>
        ///   Adds one or more <see cref = "PriceTick" />s to the PriceSeries.
        /// </summary>
        /// <param name = "priceTicks">The <see cref = "PriceTick" />s to add.</param>
        void AddPriceTicks(params PriceTick[] priceTicks);
    }
}