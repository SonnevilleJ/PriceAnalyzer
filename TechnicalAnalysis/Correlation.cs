using System;
using System.Collections.Generic;
using System.Linq;
using Statistics;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// Indicates the level of correlation between two <see cref="ITimePeriod"/>.
    /// </summary>
    public class Correlation : TimeSeriesIndicator
    {
        //
        // The algorithms in the Correlation class are based on the following article:
        // http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:correlation_coeffici
        //

        #region Private Members

        private readonly ITimeSeries _target;

        #endregion

        #region Constructors

        public Correlation(IPriceSeries priceSeries, int lookback, ITimeSeries target)
            : base(priceSeries, lookback)
        {
            _target = target;
        }

        #endregion

        protected override decimal Calculate(DateTime index)
        {
            var myPeriods = MeasuredTimeSeries.GetPreviousTimePeriods(Lookback, index).Select(x => x[index]);
            var otherPeriods = _target.GetPreviousTimePeriods(Lookback, index).Select(x => x[index]);
            return myPeriods.Correlation(otherPeriods);
        }

        ///// <summary>
        ///// Calculates the correlation coefficient for a single period.
        ///// </summary>
        ///// <param name="index">The index of the value to calculate.</param>
        //protected decimal? Calculate(int index)
        //{
        //    var timeSeriesValues = GetValues(MeasuredTimeSeries, index);
        //    var targetValues = GetValues(_target, index);

        //    var timeSeriesTask = new Task<IList<decimal>>(() => SquareElements(index, timeSeriesValues));
        //    var targetTask = new Task<IList<decimal>>(() => SquareElements(index, targetValues));
        //    Task.WaitAll(timeSeriesTask, targetTask);

        //    var timeSeriesSquares = timeSeriesTask.Result;
        //    var targetSquares = targetTask.Result;

        //    var squares = MultiplyElements(index, timeSeriesSquares, targetSquares);

        //    var timeSeriesSquare = timeSeriesValues.Average();
        //    var timeSeriesVariance = timeSeriesSquares.Average() - (timeSeriesSquare * timeSeriesSquare);
        //    var targetSquare = targetValues.Average();
        //    var targetVariance = targetSquares.Average() - (targetSquare * targetSquare);
        //    var covariance = squares.Average() - (timeSeriesSquare * targetSquare);
        //    return covariance / (decimal)Math.Sqrt((double)(timeSeriesVariance * targetVariance));

        //    throw new NotImplementedException();
        //}

        //private IList<decimal> GetValues(ITimePeriod timePeriod, int index)
        //{
        //    var result = new List<decimal>();
        //    for (var i = index - (Lookback - 1); i <= index; i++)
        //    {
        //        result.Add(timePeriod[ConvertIndexToDateTime(i)]);
        //    }
        //    return result;
        //}

        //private IList<decimal> SquareElements(int index, IList<decimal> series)
        //{
        //    return MultiplyElements(index, series, series);
        //}

        //private IList<decimal> MultiplyElements(int index, IList<decimal> series1, IList<decimal> series2)
        //{
        //    var count = Lookback - 1;
        //    if (index >= count)
        //    {
        //        var result = new List<decimal>();
        //        for (var i = index - count; i <= index; i++)
        //        {
        //            result.Add(series1[i] * series2[i]);
        //        }
        //        return result;
        //    }
        //    return null;
        //}
    }
}