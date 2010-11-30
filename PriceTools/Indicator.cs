using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A generic indicator used to transform ITimeSeries data to identify a trend, correlation, reversal, or other meaningful information about the underlying ITimeSeries data.
    /// </summary>
    public abstract class Indicator : ITimeSeries
    {
        #region Protected Members

        /// <summary>
        /// The underlying data which is to be analyzed by this Indicator.
        /// </summary>
        protected ITimeSeries _TimeSeries;

        /// <summary>
        /// The number of periods for which this Indicator has a value.
        /// </summary>
        /// <example>A 50-period MovingAverage has a Range of 50.</example>
        protected Int32 _Range;

        /// <summary>
        /// The IDictionary used to store the values of this Indicator.
        /// </summary>
        protected IDictionary<int, decimal> _Dictionary;

        /// <summary>
        /// An object to lock when performing thread unsafe tasks.
        /// </summary>
        protected readonly object _Padlock = new object();

        #endregion

        #region Constructors

        protected Indicator(ITimeSeries timeSeries, int range)
        {
            _TimeSeries = timeSeries;
            _Dictionary = new Dictionary<int, decimal>(timeSeries.Span - range);
            _Range = range;
        }

        #endregion

        /// <summary>
        /// Gets the most recent value of this Indicator.
        /// </summary>
        public virtual decimal Last
        {
            get { return this[0]; }
        }

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        /// <returns>The value of this Indicator for the given period.</returns>
        protected abstract decimal Calculate(int index);

        /// <summary>
        /// Pre-caches all values for this Indicator.
        /// </summary>
        public virtual void CalculateAll()
        {
            for (int i = 0; i < _TimeSeries.Span - _Range; i++)
            {
                Calculate(-i);
            }
        }

        #region Implementation of ITimeSeries

        /// <summary>
        /// Gets the value stored at a given index of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to retrieve.</param>
        /// <remarks>Usually <para>index</para> must be negative or zero, as most Indicators are lagging and not forward projecting.</remarks>
        /// <returns>The value at the given index.</returns>
        public virtual decimal this[int index]
        {
            get
            {
                decimal value;
                if (_Dictionary.TryGetValue(index, out value))
                {
                    return value;
                }
                else
                {
                    return Calculate(index);
                }
            }
        }

        /// <summary>
        /// Gets the number of periods for which this Indicator has a value.
        /// </summary>
        public virtual int Span
        {
            get { return _Dictionary.Count; }
        }

        /// <summary>
        /// Gets the range of this Indicator.
        /// </summary>
        /// <example>A 50-period MovingAverage has a Range of 50.</example>
        public virtual int Range
        {
            get { return _Range; }
        }

        #endregion

        #region Implementation of ISerializable

        protected Indicator(SerializationInfo info, StreamingContext context)
        {
            _Dictionary = (IDictionary<int, decimal>) info.GetValue("Dictionary", typeof (IDictionary<int, decimal>));
            _TimeSeries = (ITimeSeries) info.GetValue("TimeSeries", typeof (ITimeSeries));
            _Range = (Int32) info.GetValue("Range", typeof (Int32));
            _Padlock = new object();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Dictionary", _Dictionary);
            info.AddValue("TimeSeries", _TimeSeries);
            info.AddValue("Range", _Range);
        }

        #endregion
    }
}
