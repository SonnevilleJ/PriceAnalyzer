using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A <see cref="PricePeriod"/> made from <see cref="PriceTicks"/>.
    /// </summary>
    public abstract class TickedPricePeriod : PricePeriod
    {
        /// <summary>
        ///  The <see cref="PriceTick" />s contained within this TickedPricePeriod.
        ///  </summary>
        public abstract IList<PriceTick> PriceTicks { get; }

        /// <summary>
        ///    Adds one or more <see cref="PriceTick" />s to the PriceSeries.
        ///  </summary><param name="priceTicks">The <see cref="PriceTick" />s to add.</param>
        public abstract void AddPriceTicks(params PriceTick[] priceTicks);
    }
}