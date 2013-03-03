using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A <see cref="IPricePeriod"/> made from <see cref="PriceTicks"/>.
    /// </summary>
    public interface ITickedPricePeriod : IPricePeriod, IEquatable<ITickedPricePeriod>
    {
        /// <summary>
        ///  The <see cref="IPriceTick" />s contained within this ITickedPricePeriod.
        ///  </summary>
        IList<IPriceTick> PriceTicks { get; }

        /// <summary>
        ///    Adds one or more <see cref="IPriceTick" />s to the PriceSeries.
        ///  </summary><param name="priceTicks">The <see cref="IPriceTick" />s to add.</param>
        void AddPriceTicks(params IPriceTick[] priceTicks);
    }
}