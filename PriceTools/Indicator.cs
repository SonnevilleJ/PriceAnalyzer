using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Sonneville.PriceTools.Internal;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A generic indicator used to transform ITimeSeries data to identify a trend, correlation, reversal, or other meaningful information about the underlying ITimeSeries data.
    /// </summary>
    [Serializable]
    public abstract class Indicator : ITimeSeries
    {
        #region Private Members

        private readonly DualIndexedCollection<decimal> _values;
        private readonly ITimeSeries _timeSeries;
        private readonly Int32 _range;
        private readonly object _padlock = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an Indicator for a given <see cref="ITimeSeries"/>.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to measure.</param>
        /// <param name="range">The range used by this indicator.</param>
        protected Indicator(ITimeSeries timeSeries, int range)
        {
            _timeSeries = timeSeries;
            _values = new DualIndexedCollection<decimal>(timeSeries.Span - range);
            _range = range;
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
            for (int i = 0; i < TimeSeries.Span - _range; i++)
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
            get { return _values[index]; }
            protected set { _values[index] = value; }
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>THe value of the ITimeSeries as of the given DateTime.</returns>
        public virtual decimal this[DateTime index]
        {
            get { return _values[index]; }
            protected set { _values[index] = value; }
        }

        /// <summary>
        /// Gets the number of periods for which this Indicator has a value.
        /// </summary>
        public virtual int Span
        {
            get { return _values.Count; }
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
        protected ITimeSeries TimeSeries
        {
            get { return _timeSeries; }
        }

        /// <summary>
        /// An object to lock when performing thread unsafe tasks.
        /// </summary>
        protected object Padlock
        {
            get { return _padlock; }
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
            _values = (DualIndexedCollection<decimal>) info.GetValue("Dictionary", typeof (DualIndexedCollection<decimal>));
            _timeSeries = (ITimeSeries) info.GetValue("TimeSeries", typeof (ITimeSeries));
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
            info.AddValue("Collection", _values, typeof (DualIndexedCollection<decimal>));
            info.AddValue("TimeSeries", TimeSeries, typeof (ITimeSeries));
            info.AddValue("Range", _range, typeof (int));
        }

        #endregion
    }
}
