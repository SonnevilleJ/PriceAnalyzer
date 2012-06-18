using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class storing extention methods 
    /// </summary>
    public static class PricePeriodExtentions
    {
        /// <summary>
        /// Returns a value indicating whether a target <see cref="PricePeriod"/> is equal to the original <see cref="PricePeriod"/>.
        /// </summary>
        /// <remarks>This method only considers the data within the <see cref="PricePeriod"/> and ignores the datatype implementing the <see cref="PricePeriod"/>.</remarks>
        public static bool IsEqual(this PricePeriod original, PricePeriod target)
        {
            if (original == null || target == null) return false;

            return original.Resolution == target.Resolution &&
                   original.Head == target.Head &&
                   original.Tail == target.Tail &&
                   original.Open == target.Open &&
                   original.High == target.High &&
                   original.Low == target.Low &&
                   original.Close == target.Close &&
                   original.Volume == target.Volume;
        }

        /// <summary>
        /// Returns a value indicating whether a target <see cref="IEnumerable{PricePeriod}"/> is equal to the original <see cref="IEnumerable{PricePeriod}"/>.
        /// </summary>
        /// <remarks>This method only considers the data within each <see cref="PricePeriod"/> and ignores the datatype implementing the <see cref="PricePeriod"/>s.</remarks>
        public static bool IsEqual(this IEnumerable<PricePeriod> original, IEnumerable<PricePeriod> target)
        {
            var oParallel = original.AsParallel();
            var tParallel = target.AsParallel();
            return oParallel.All(period => oParallel.Where(p => p.IsEqual(period)).Count() == tParallel.Where(p => p.IsEqual(period)).Count());
        }
    }
}
