using System;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public abstract class PriceSeriesIndicator : TimeSeriesIndicator, IPriceSeriesIndicator
    {
        #region Private Members

        private IPriceSeries _measuredPriceSeries;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an PriceSeriesIndicator for a given <see cref="MeasuredPriceSeries"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to transform.</param>
        /// <param name="lookback">The lookback of this PriceSeriesIndicator which specifies how many periods are required for the first indicator value.</param>
        protected PriceSeriesIndicator(IPriceSeries priceSeries, int lookback)
            : base(priceSeries, lookback)
        {
        }

        #endregion

        #region Implementation of IPriceSeriesIndicator

        /// <summary>
        /// The underlying data which is to be analyzed by this PriceSeriesIndicator.
        /// </summary>
        public override ITimeSeries MeasuredTimeSeries
        {
            get { return MeasuredPriceSeries; }
            protected set { MeasuredPriceSeries = value as IPriceSeries; }
        }

        /// <summary>
        /// The underlying data which is to be analyzed by this PriceSeriesIndicator.
        /// </summary>
        public IPriceSeries MeasuredPriceSeries
        {
            get { return _measuredPriceSeries; }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (value.TimeSpan() < new TimeSpan(Lookback * (long)value.Resolution))
                {
                    // not enough data to calculate at least one indicator value
                    //throw new InvalidOperationException("The TimeSpan of priceSeries is too narrow for the given lookback duration.");
                }

                _measuredPriceSeries = value;
            }
        }

        #endregion

    }
}