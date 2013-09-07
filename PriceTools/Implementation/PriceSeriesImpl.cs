using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a time series of price data.
    /// </summary>
    internal class PriceSeriesImpl : PricePeriodImpl, IPriceSeries
    {
        /// <summary>
        /// The default <see cref="Resolution"/> of a PriceSeries.
        /// </summary>
        private const Resolution DefaultResolution = Resolution.Days;

        #region Private Members

        private readonly Resolution _resolution;

        private readonly IList<IPricePeriod> _dataPeriods = new List<IPricePeriod>();

        #endregion

        #region Constructors
        
        /// <summary>
        /// Constructs a PriceSeries object.
        /// </summary>
        /// <param name="resolution"></param>
        protected internal PriceSeriesImpl(Resolution resolution = DefaultResolution)
        {
            _resolution = resolution;
        }

        #endregion

        #region Overrides of PriceSeries

        /// <summary>
        /// Gets the closing price for the PriceSeries.
        /// </summary>
        public override decimal Close
        {
            get { return DataPeriods.OrderBy(p => p.Tail).Last().Close; }
        }

        /// <summary>
        /// Gets the highest price that occurred during the PriceSeries.
        /// </summary>
        public override decimal High
        {
            get { return DataPeriods.Max(p => p.High); }
        }

        /// <summary>
        /// Gets the lowest price that occurred during the PriceSeries.
        /// </summary>
        public override decimal Low
        {
            get { return DataPeriods.Min(p => p.Low); }
        }

        /// <summary>
        /// Gets the opening price for the PriceSeries.
        /// </summary>
        public override decimal Open
        {
            get { return DataPeriods.OrderBy(p => p.Head).First().Open; }
        }

        /// <summary>
        /// Gets the total volume of trades during the PriceSeries.
        /// </summary>
        public override long? Volume
        {
            get { return DataPeriods.Sum(p => p.Volume); }
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the PriceSeries.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the PriceSeries as of the given DateTime.</returns>
        public override decimal this[DateTime dateTime]
        {
            get
            {
                return dateTime < Head ? 0.0m : GetLatestPrice(this, dateTime);
            }
        }

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries.
        /// </summary>
        public IEnumerable<ITimePeriod> TimePeriods
        {
            get { return PricePeriods.Cast<ITimePeriod>().ToList(); }
        }

        /// <summary>
        /// Gets the first DateTime in the PriceSeries.
        /// </summary>
        public override DateTime Head
        {
            get
            {
                var dataPeriods = DataPeriods;
                if(!dataPeriods.Any()) throw new InvalidOperationException(Strings.PriceSeriesImpl_Price_series_contains_no_price_periods_);
                return dataPeriods.Min(p => p.Head);
            }
        }

        /// <summary>
        /// Gets the last DateTime in the PriceSeries.
        /// </summary>
        public override DateTime Tail
        {
            get
            {
                var dataPeriods = DataPeriods;
                if (!dataPeriods.Any()) throw new InvalidOperationException(Strings.PriceSeriesImpl_Price_series_contains_no_price_periods_);
                return dataPeriods.Max(p => p.Tail);
            }
        }

        /// <summary>
        /// Gets the ticker symbol priced by this PriceSeries.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Gets a collection of the <see cref="PricePeriodImpl"/>s in this PriceSeries.
        /// </summary>
        public IEnumerable<IPricePeriod> PricePeriods { get { return DataPeriods; } }

        /// <summary>
        /// Adds price data to the PriceSeries.
        /// </summary>
        /// <param name="pricePeriod"></param>
        public void AddPriceData(IPricePeriod pricePeriod)
        {
            AddPriceData(new [] {pricePeriod});
        }

        /// <summary>
        /// Adds price data to the PriceSeries.
        /// </summary>
        /// <param name="pricePeriods"></param>
        public void AddPriceData(IEnumerable<IPricePeriod> pricePeriods)
        {
            var list = pricePeriods.Where(period => !this.HasValueInRange(period.Head) && !this.HasValueInRange(period.Tail)).ToList();

            if (list.Any())
            {
                foreach (var pricePeriod in list)
                {
                    _dataPeriods.Add(pricePeriod);
                }
            }
        }

        /// <summary>
        /// Gets or sets the resolution of PricePeriods to retrieve.
        /// </summary>
        public override Resolution Resolution
        {
            get { return _resolution; }
        }

        private ParallelQuery<IPricePeriod> DataPeriods
        {
            get { return _dataPeriods.AsParallel(); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the most recent price at or before <paramref name="settlementDate"/>.
        /// </summary>
        /// <param name="priceSeries"> </param>
        /// <param name="settlementDate">The DateTime to price.</param>
        /// <exception cref="InvalidOperationException">Throws if no price is available at or before <paramref name="settlementDate"/>.</exception>
        /// <returns>The most recent price at or before <paramref name="settlementDate"/>.</returns>
        private static decimal GetLatestPrice(IPriceSeries priceSeries, DateTime settlementDate)
        {
            var matchingPeriods = priceSeries.PricePeriods.Where(p => p.HasValueInRange(settlementDate)).ToList();
            if (matchingPeriods.Any()) return matchingPeriods.OrderBy(p => p.Tail).Last()[settlementDate];

            if (priceSeries.PricePeriods.Any()) return priceSeries.PricePeriods.OrderBy(p => p.Tail).Last(p => p.Tail <= settlementDate).Close;

            throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, Strings.PriceSeries_GetLatestPrice_No_price_data_available_for_settlement_date___0_, settlementDate));
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
        public override bool Equals(IPricePeriod other)
        {
            return Equals(other as IPriceSeries);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IPriceSeries other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Resolution == other.Resolution &&
                   Ticker == other.Ticker &&
                   other.PricePeriods.All(pricePeriod => PricePeriods.Contains(pricePeriod));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="obj"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="obj">An object to compare with this object.</param>
        public override bool Equals(object obj)
        {
            return Equals(obj as IPriceSeries);
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
                return Ticker.GetHashCode();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(PriceSeriesImpl left, PriceSeriesImpl right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(PriceSeriesImpl left, PriceSeriesImpl right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
