using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A <see cref="IPricePeriod"/> made from <see cref="PriceTicks"/>.
    /// </summary>
    public interface ITickedPricePeriod : IPricePeriod, IEquatable<ITickedPricePeriod>
    {
        /// <summary>
        ///  The <see cref="PriceTick" />s contained within this ITickedPricePeriod.
        ///  </summary>
        IList<PriceTick> PriceTicks { get; }

        /// <summary>
        ///    Adds one or more <see cref="PriceTick" />s to the PriceSeries.
        ///  </summary><param name="priceTicks">The <see cref="PriceTick" />s to add.</param>
        void AddPriceTicks(params PriceTick[] priceTicks);
    }
}