using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// An averager creates moving averages of <see cref="PriceSeries"/> objects.
    /// </summary>
    public class MovingAverage : ITimeSeries
    {
        private ITimeSeries _series;
        private IDictionary<int, decimal> _dictionary;
        private readonly int _length;
        private MovingAverageMethod _method;
        private readonly object _padlock = new Object();

        /// <summary>
        /// Constructs a new Averager using the specified <see cref="MovingAverageMethod"/>
        /// </summary>
        /// <param name="series">The IPriceSeries containing the data to be averaged.</param>
        /// <param name="length">The number of periods to average together.</param>
        /// <param name="movingAverageMethod">The calculation method to use when averaging.</param>
        public MovingAverage(ITimeSeries series, int length, MovingAverageMethod movingAverageMethod = MovingAverageMethod.Simple)
        {
            _series = series;
            _dictionary = new Dictionary<int, decimal>(series.Length - length);
            _length = length;
            _method = movingAverageMethod;
        }

        /// <summary>
        /// Gets the most recent value of this MovingAverage.
        /// </summary>
        public decimal Last
        {
            get { return this[0]; }
        }

        /// <summary>
        /// Gets the value stored at a given index of this MovingAverage.
        /// </summary>
        /// <param name="i">The index of the value to retrieve. Because MovingAverage is a lagging indicator, indexes must be negative or zero.</param>
        /// <returns>The value at the given index.</returns>
        public decimal this[int i]
        {
            get
            {
                decimal value;
                if(_dictionary.TryGetValue(i, out value))
                {
                    return value;
                }
                else
                {
                    return Calculate(i);
                }
            }
        }

        /// <summary>
        /// Gets the length of this MovingAverage.
        /// </summary>
        public int Length
        {
            get { return _dictionary.Count; }
        }

        /// <summary>
        /// Pre-caches all values for the moving average.
        /// </summary>
        public void CalculateAll()
        {
            // TODO: Rewrite this method to be more efficient
            for(int i = 0; i < _series.Length - _length; i++)
            {
                Calculate(-i);
            }
        }

        private decimal Calculate(int index)
        {
            switch (_method)
            {
                case MovingAverageMethod.Simple:
                    decimal sum = 0;
                    for (int i = _series.Length + (index - _length); i < _series.Length + index; i++)
                    {
                        sum += _series[i];
                    }
                    lock (_padlock)
                    {
                        _dictionary[index] = sum/_length;
                    }
                    return _dictionary[index];
                case MovingAverageMethod.Exponential:
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
