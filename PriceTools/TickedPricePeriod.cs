using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A <see cref="PricePeriodImpl"/> made from <see cref="PriceTicks"/>.
    /// </summary>
    public abstract class TickedPricePeriod : PricePeriodImpl, IEquatable<TickedPricePeriod>
    {
        /// <summary>
        ///  The <see cref="PriceTick" />s contained within this TickedPricePeriod.
        ///  </summary>
        public abstract IList<PriceTick> PriceTicks { get; }

        /// <summary>
        ///    Adds one or more <see cref="PriceTick" />s to the PriceSeries.
        ///  </summary><param name="priceTicks">The <see cref="PriceTick" />s to add.</param>
        public abstract void AddPriceTicks(params PriceTick[] priceTicks);

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(TickedPricePeriod other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return base.Equals(other) &&
                   other.PriceTicks.All(priceTick => PriceTicks.Contains(priceTick));
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return Equals(obj as TickedPricePeriod);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            var result = base.GetHashCode();
            result = (result * 397) ^ PriceTicks.Aggregate(result, (current, priceTick) => (current * 397) ^ priceTick.GetHashCode());
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(TickedPricePeriod left, TickedPricePeriod right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TickedPricePeriod left, TickedPricePeriod right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}