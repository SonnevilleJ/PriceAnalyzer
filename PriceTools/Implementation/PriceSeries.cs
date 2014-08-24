using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    internal class PriceSeries : IPriceSeries
    {
        private const Resolution DefaultResolution = Resolution.Days;

        private readonly Resolution _resolution;

        private readonly IList<IPricePeriod> _dataPeriods = new List<IPricePeriod>();
        private readonly ITimeSeriesUtility _timeSeriesUtility;

        protected internal PriceSeries(Resolution resolution = DefaultResolution)
        {
            _resolution = resolution;
            _timeSeriesUtility = new TimeSeriesUtility();
        }

        public decimal Close
        {
            get { return DataPeriods.OrderBy(p => p.Tail).Last().Close; }
        }

        public decimal High
        {
            get { return DataPeriods.Max(p => p.High); }
        }

        public decimal Low
        {
            get { return DataPeriods.Min(p => p.Low); }
        }

        public decimal Open
        {
            get { return DataPeriods.OrderBy(p => p.Head).First().Open; }
        }

        public long? Volume
        {
            get { return DataPeriods.Sum(p => p.Volume); }
        }

        public decimal this[DateTime dateTime]
        {
            get
            {
                return dateTime < Head ? 0.0m : GetLatestPrice(this, dateTime);
            }
        }

        public IEnumerable<ITimePeriod<decimal>> TimePeriods
        {
            get { return PricePeriods.Cast<ITimePeriod<decimal>>().ToList(); }
        }

        public DateTime Head
        {
            get
            {
                var dataPeriods = DataPeriods;
                if(!dataPeriods.Any()) throw new InvalidOperationException(Strings.PriceSeriesImpl_Price_series_contains_no_price_periods_);
                return dataPeriods.Min(p => p.Head);
            }
        }

        public DateTime Tail
        {
            get
            {
                var dataPeriods = DataPeriods;
                if (!dataPeriods.Any()) throw new InvalidOperationException(Strings.PriceSeriesImpl_Price_series_contains_no_price_periods_);
                return dataPeriods.Max(p => p.Tail);
            }
        }

        public string Ticker { get; set; }

        public IEnumerable<IPricePeriod> PricePeriods { get { return DataPeriods; } }

        public void AddPriceData(IPricePeriod pricePeriod)
        {
            AddPriceData(new [] {pricePeriod});
        }

        public void AddPriceData(IEnumerable<IPricePeriod> pricePeriods)
        {
            var list = pricePeriods.Where(period => !_timeSeriesUtility.HasValueInRange(this, period.Head) && !_timeSeriesUtility.HasValueInRange(this, period.Tail)).ToList();

            if (list.Any())
            {
                foreach (var pricePeriod in list)
                {
                    _dataPeriods.Add(pricePeriod);
                }
            }
        }

        public Resolution Resolution
        {
            get { return _resolution; }
        }

        private ParallelQuery<IPricePeriod> DataPeriods
        {
            get { return _dataPeriods.AsParallel(); }
        }

        private static decimal GetLatestPrice(IPriceSeries priceSeries, DateTime settlementDate)
        {
            var matchingPeriods = priceSeries.PricePeriods.Where(p => p.HasValueInRange(settlementDate)).ToList();
            if (matchingPeriods.Any()) return matchingPeriods.OrderBy(p => p.Tail).Last()[settlementDate];

            if (priceSeries.PricePeriods.Any()) return priceSeries.PricePeriods.OrderBy(p => p.Tail).Last(p => p.Tail <= settlementDate).Close;

            throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, Strings.PriceSeries_GetLatestPrice_No_price_data_available_for_settlement_date___0_, settlementDate));
        }

        #region Equality

        public bool Equals(IPricePeriod other)
        {
            return Equals(other as IPriceSeries);
        }

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

        public override bool Equals(object obj)
        {
            return Equals(obj as IPriceSeries);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Ticker.GetHashCode();
            }
        }

        public static bool operator ==(PriceSeries left, PriceSeries right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PriceSeries left, PriceSeries right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
