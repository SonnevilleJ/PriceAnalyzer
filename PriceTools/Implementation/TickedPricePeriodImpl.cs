using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    ///   An <see cref="IPricePeriod"/> made from <see cref="PriceTicks"/>.
    /// </summary>
    internal class TickedPricePeriodImpl : PricePeriodImpl, ITickedPricePeriod
    {
        private readonly List<PriceTick> _priceTicks = new List<PriceTick>();
        
        /// <summary>
        /// The <see cref="PriceTick"/>s contained within this ITickedPricePeriod.
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
        ///   Gets the first DateTime in the ITimePeriod.
        /// </summary>
        public override DateTime Head
        {
            get { return PriceTicks.Min(q => q.SettlementDate); }
        }

        /// <summary>
        ///   Gets the last DateTime in the ITimePeriod.
        /// </summary>
        public override DateTime Tail
        {
            get { return PriceTicks.Max(q => q.SettlementDate); }
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
            var args = new NewDataAvailableEventArgs
                           {
                               Head = priceTicks.Min(quote => quote.SettlementDate),
                               Tail = priceTicks.Max(quote => quote.SettlementDate)
                           };
            InvokeNewDataAvailable(args);
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
        /// <param name = "dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimePeriod as of the given DateTime.</returns>
        public override decimal this[DateTime dateTime]
        {
            get { return PriceTicks.Last(q => q.SettlementDate <= dateTime).Price; }
        }

        #endregion

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ITickedPricePeriod other)
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
            return Equals(obj as ITickedPricePeriod);
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
        public static bool operator ==(TickedPricePeriodImpl left, TickedPricePeriodImpl right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TickedPricePeriodImpl left, TickedPricePeriodImpl right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}