using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// An averager creates moving averages of <see cref="PriceSeries"/> objects.
    /// </summary>
    public class MovingAverage : Indicator
    {
        private readonly MovingAverageMethod _method;

        /// <summary>
        /// Constructs a new Averager using the specified <see cref="MovingAverageMethod"/>
        /// </summary>
        /// <param name="series">The IPriceSeries containing the data to be averaged.</param>
        /// <param name="range">The number of periods to average together.</param>
        /// <param name="movingAverageMethod">The calculation method to use when averaging.</param>
        public MovingAverage(ITimeSeries series, int range, MovingAverageMethod movingAverageMethod = MovingAverageMethod.Simple)
        {
            _TimeSeries = series;
            _Dictionary = new Dictionary<int, decimal>(series.Span - range);
            _Range = range;
            _method = movingAverageMethod;
        }

        /// <summary>
        /// Calculates a single value of this MovingAverage.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        /// <returns>The value of this MovingAverage for the given period.</returns>
        protected override decimal Calculate(int index)
        {
            switch (_method)
            {
                case MovingAverageMethod.Simple:
                    decimal sum = 0;
                    for (int i = _TimeSeries.Span + (index - Range); i < _TimeSeries.Span + index; i++)
                    {
                        sum += _TimeSeries[i];
                    }
                    lock (_Padlock)
                    {
                        return _Dictionary[index] = sum / Range;
                    }
                case MovingAverageMethod.Exponential:
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
