using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A generic indicator used to transform ITimeSeries data to identify a trend, correlation, reversal, or other meaningful information about the underlying ITimeSeries data.
    /// </summary>
    [Serializable]
    public abstract class Indicator : ITimeSeries
    {
        #region Private Members

        private readonly IPriceSeries _priceSeries;
        private readonly Int32 _range;
        private readonly IDictionary<DateTime, decimal> _dictionary;
        private readonly object _padlock = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an Indicator for a given <see cref="ITimeSeries"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="ITimeSeries"/> to measure.</param>
        /// <param name="range">The range used by this indicator.</param>
        protected Indicator(IPriceSeries priceSeries, int range)
        {
            if (priceSeries == null)
            {
                throw new ArgumentNullException("priceSeries");
            }
            _priceSeries = priceSeries;
            _dictionary = new Dictionary<DateTime, decimal>(priceSeries.Periods.Count - range);
            _range = range;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public virtual DateTime Head
        {
            get { return _priceSeries.Head; }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public virtual DateTime Tail
        {
            get { return _priceSeries.Tail; }
        }

        /// <summary>
        /// Gets the most recent value of this Indicator.
        /// </summary>
        public decimal Last
        {
            get { return this[Tail]; }
        }

        #endregion

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        /// <returns>The value of this Indicator for the given period.</returns>
        protected abstract decimal Calculate(DateTime index);

        /// <summary>
        /// Pre-caches all values for this Indicator.
        /// </summary>
        public virtual void CalculateAll()
        {
            for (DateTime date = Head; date <= Tail; date = IncrementDate(date))
            {
                if (HasValue(date))
                {
                    this[date] = Calculate(date);
                }
            }
        }

        /// <summary>
        /// Increments a date by 1 day.
        /// </summary>
        /// <param name="date">The date to increment.</param>
        /// <returns>A <see cref="DateTime"/> 1 day after <paramref name="date"/>.</returns>
        protected static DateTime IncrementDate(DateTime date)
        {
            return date.AddDays(1);
        }

        #region Implementation of ITimeSeries

        /// <summary>
        /// Gets the value stored at a given index of this Indicator.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>THe value of the ITimeSeries as of the given DateTime.</returns>
        public virtual decimal this[DateTime index]
        {
            get
            {
                decimal value;
                return _dictionary.TryGetValue(index, out value) ? value : Calculate(index);
            }
            protected set { _dictionary[index] = value; }
        }

        /// <summary>
        /// Gets the number of periods for which this Indicator has a value.
        /// </summary>
        public virtual int Span
        {
            get { return _dictionary.Count; }
        }

        /// <summary>
        /// Gets the range of this Indicator.
        /// </summary>
        /// <example>A 50-period MovingAverage has a Range of 50.</example>
        public int Range
        {
            get { return _range; }
        }

        /// <summary>
        /// The underlying data which is to be analyzed by this Indicator.
        /// </summary>
        protected ITimeSeries PriceSeries
        {
            get { return _priceSeries; }
        }

        /// <summary>
        /// An object to lock when performing thread unsafe tasks.
        /// </summary>
        protected object Padlock
        {
            get { return _padlock; }
        }

        /// <summary>
        /// Determines if the Indicator has a valid value for a given date.
        /// </summary>
        /// <remarks>Assumes the Indicator has a valid value for every date of the underlying IPriceSeries.</remarks>
        /// <param name="date">The date to check.</param>
        /// <returns>A value indicating if the Indicator has a valid value for the given date.</returns>
        public virtual bool HasValue(DateTime date)
        {
            return (date >= Head && date <= Tail);
        }

        #endregion

        #region Implementation of ISerializable

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected Indicator(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            _dictionary = (IDictionary<DateTime, decimal>) info.GetValue("Dictionary", typeof (IDictionary<DateTime, decimal>));
            _priceSeries = (IPriceSeries) info.GetValue("priceSeries", typeof (IPriceSeries));
            _range = (Int32) info.GetValue("Range", typeof (Int32));
        }

        /// <summary>
        /// Serializies an Indicator.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("Dictionary", _dictionary);
            info.AddValue("priceSeries", PriceSeries);
            info.AddValue("Range", _range);
        }

        #endregion

        #region Equality Checks

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Indicator left, Indicator right)
        {
            return (left._dictionary == right._dictionary &&
                    left._priceSeries == right._priceSeries &&
                    left._range == right._range);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Indicator left, Indicator right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            return !(left == right);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Indicator other)
        {
            return Equals((object)other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ITimeSeries other)
        {
            return Equals((object)other);
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
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Indicator)) return false;
            return this == (Indicator)obj;
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
            unchecked
            {
                int result = _dictionary.GetHashCode();
                result = (result * 397) ^ _priceSeries.GetHashCode();
                result = (result * 397) ^ _range.GetHashCode();
                return result;
            }
        }

        #endregion
    }
}
