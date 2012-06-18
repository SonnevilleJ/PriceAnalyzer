using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// Returns a value indicating whether a target <see cref="IEnumerable{PricePeriod}"/> is equivalent to the original <see cref="IEnumerable{PricePeriod}"/>. Order of <see cref="PricePeriod"/>s is ignored.
        /// </summary>
        /// <remarks>This method only considers the data within each <see cref="PricePeriod"/> and ignores the datatype implementing the <see cref="PricePeriod"/>s.</remarks>
        public static bool IsEquivalent(this IEnumerable<PricePeriod> original, IEnumerable<PricePeriod> target)
        {
            var oParallel = original.AsParallel();
            var tParallel = target.AsParallel();
            return oParallel.All(period => oParallel.Where(p => p.IsEqual(period)).Count() == tParallel.Where(p => p.IsEqual(period)).Count());
        }

        /// <summary>
        /// Returns a value indicating whether a target <see cref="IEnumerable{PricePeriod}"/> is equal to the original <see cref="IEnumerable{PricePeriod}"/>. Order of <see cref="PricePeriod"/>s must match.
        /// </summary>
        /// <remarks>This method only considers the data within each <see cref="PricePeriod"/> and ignores the datatype implementing the <see cref="PricePeriod"/>s.</remarks>
        public static bool IsEqual(this IEnumerable<PricePeriod> original, IEnumerable<PricePeriod> target)
        {
            if (original.Count() != target.Count()) return false;

            var result = true;
            Parallel.For(0, original.Count(), (i, loopState) => {
                                                       if (!target.ElementAt(i).IsEqual(original.ElementAt(i)))
                                                       {
                                                           result = false;
                                                           loopState.Stop();
                                                       }
            });
            return result;
        }
    }
}
