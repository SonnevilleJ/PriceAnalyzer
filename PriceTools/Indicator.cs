//using System;
//using System.Collections.Generic;

//namespace Sonneville.PriceTools
//{
//    /// <summary>
//    /// A generic indicator used to transform ITimeSeries data to identify a trend, correlation, reversal, or other meaningful information about the underlying ITimeSeries data.
//    /// </summary>
//    public abstract class Indicator : ITimeSeries
//    {
//        #region Private Members

//        private readonly IPriceSeries _priceSeries;
//        private readonly Int32 _range;
//        private readonly IDictionary<DateTime, decimal> _dictionary;
//        private readonly object _padlock = new object();

//        #endregion

//        #region Constructors

//        /// <summary>
//        /// Constructs an Indicator for a given <see cref="ITimeSeries"/>.
//        /// </summary>
//        /// <param name="priceSeries">The <see cref="ITimeSeries"/> to measure.</param>
//        /// <param name="range">The range used by this indicator.</param>
//        protected Indicator(IPriceSeries priceSeries, int range)
//        {
//            if (priceSeries == null)
//            {
//                throw new ArgumentNullException("priceSeries");
//            }
//            _priceSeries = priceSeries;
//            _dictionary = new Dictionary<DateTime, decimal>(priceSeries.Periods.Count - range);
//            _range = range;
//        }

//        #endregion

//        #region Accessors

//        /// <summary>
//        /// Gets the first DateTime in the ITimeSeries.
//        /// </summary>
//        public virtual DateTime Head
//        {
//            get { throw new NotImplementedException(); }
//        }

//        /// <summary>
//        /// Gets the last DateTime in the ITimeSeries.
//        /// </summary>
//        public virtual DateTime Tail
//        {
//            get { throw new NotImplementedException(); }
//        }

//        /// <summary>
//        /// Gets the most recent value of this Indicator.
//        /// </summary>
//        public decimal Last
//        {
//            get { return this[Tail]; }
//        }

//        #endregion

//        /// <summary>
//        /// Calculates a single value of this Indicator.
//        /// </summary>
//        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
//        /// <returns>The value of this Indicator for the given period.</returns>
//        protected abstract decimal Calculate(DateTime index);

//        /// <summary>
//        /// Pre-caches all values for this Indicator.
//        /// </summary>
//        public virtual void CalculateAll()
//        {
//            for (DateTime date = Head; date <= Tail; date = IncrementDate(date))
//            {
//                if (HasValue(date))
//                {
//                    this[date] = Calculate(date);
//                }
//            }
//        }

//        /// <summary>
//        /// Increments a date by 1 day.
//        /// </summary>
//        /// <param name="date">The date to increment.</param>
//        /// <returns>A <see cref="DateTime"/> 1 day after <paramref name="date"/>.</returns>
//        protected static DateTime IncrementDate(DateTime date)
//        {
//            return date.AddDays(1);
//        }

//        #region Implementation of ITimeSeries

//        /// <summary>
//        /// Gets the value stored at a given index of this Indicator.
//        /// </summary>
//        /// <param name="index">The DateTime of the desired value.</param>
//        /// <returns>THe value of the ITimeSeries as of the given DateTime.</returns>
//        public virtual decimal this[DateTime index]
//        {
//            get
//            {
//                decimal value;
//                return _dictionary.TryGetValue(index, out value) ? value : Calculate(index);
//            }
//            protected set { _dictionary[index] = value; }
//        }

//        /// <summary>
//        /// Gets the number of periods for which this Indicator has a value.
//        /// </summary>
//        public virtual int Span
//        {
//            get { return _dictionary.Count; }
//        }

//        /// <summary>
//        /// Gets the range of this Indicator.
//        /// </summary>
//        /// <example>A 50-period MovingAverage has a Range of 50.</example>
//        public int Range
//        {
//            get { return _range; }
//        }

//        /// <summary>
//        /// The underlying data which is to be analyzed by this Indicator.
//        /// </summary>
//        protected ITimeSeries PriceSeries
//        {
//            get { return _priceSeries as ITimeSeries; }
//        }

//        /// <summary>
//        /// An object to lock when performing thread unsafe tasks.
//        /// </summary>
//        protected object Padlock
//        {
//            get { return _padlock; }
//        }

//        /// <summary>
//        /// Determines if the Indicator has a valid value for a given date.
//        /// </summary>
//        /// <remarks>Assumes the Indicator has a valid value for every date of the underlying IPriceSeries.</remarks>
//        /// <param name="settlementDate">The date to check.</param>
//        /// <returns>A value indicating if the Indicator has a valid value for the given date.</returns>
//        public virtual bool HasValue(DateTime settlementDate)
//        {
//            return (settlementDate >= Head && settlementDate <= Tail);
//        }

//        #endregion
//    }
//}
