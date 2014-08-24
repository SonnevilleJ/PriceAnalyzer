using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace Sonneville.PriceTools.Data.Csv
{
    public abstract class PriceHistoryCsvFile
    {
        private static readonly IDictionary<PriceColumn, string> DefaultColumnHeaders = new Dictionary<PriceColumn, string>
                                                                                            {
                                                                                                {PriceColumn.Date, "Date"},
                                                                                                {PriceColumn.Open, "Open"},
                                                                                                {PriceColumn.High, "High"},
                                                                                                {PriceColumn.Low, "Low"},
                                                                                                {PriceColumn.Close, "Close"},
                                                                                                {PriceColumn.Volume, "Volume"}
                                                                                            };

        protected PriceHistoryCsvFile()
        {
        }

        protected internal PriceHistoryCsvFile(Stream stream, DateTime? impliedHead = null, DateTime? impliedTail = null, Resolution? impliedResolution = null)
        {
            Read(stream, impliedHead, impliedTail, impliedResolution);
        }

        public IList<IPricePeriod> PricePeriods { get; private set; }

        public void Read(Stream stream, DateTime? impliedHead = null, DateTime? impliedTail = null, Resolution? impliedResolution = null)
        {
            using (var reader = new StreamReader(stream))
            {
                Read(reader, impliedHead, impliedTail, impliedResolution);
            }
        }

        public void Read(TextReader textReader, DateTime? impliedHead = null, DateTime? impliedTail = null, Resolution? impliedResolution = null)
        {
            using (var csvReader = new CsvReader(textReader, true))
            {
                var map = MapHeaders(csvReader);
                var stagedPeriods = StagePeriods(csvReader, map);
                PricePeriods = BuildPricePeriods(stagedPeriods, impliedHead, impliedTail, impliedResolution);
            }
        }

        public void Write(Stream stream)
        {
            Write(new StreamWriter(stream));
        }

        public void Write(TextWriter writer)
        {
            WriteHeaders(writer);
            foreach (var period in PricePeriods)
            {
                writer.Write(BuildCSVRecord(period));
                writer.Write(Environment.NewLine);
            }
        }

        private IDictionary<PriceColumn, int> MapHeaders(CsvReader reader)
        {
            var headers = reader.GetFieldHeaders();
            var dictionary = new Dictionary<PriceColumn, int>();
            for (var i = 0; i < headers.Length; i++)
            {
                var column = ParseColumnHeader(headers[i]);
                if (column != PriceColumn.None)
                {
                    dictionary.Add(column, i);
                }
            }
            return dictionary;
        }

        private void WriteHeaders(TextWriter writer)
        {
            for (var i = 0; i < ColumnHeaders.Count; i++)
            {
                writer.Write(ColumnHeaders.ElementAt(i).Value);

                if (i < ColumnHeaders.Count - 1) writer.Write(",");
            }
            writer.Write(Environment.NewLine);
        }

        private IList<SingleDatePeriod> StagePeriods(CsvReader reader, IDictionary<PriceColumn, int> map)
        {
            var stagedPeriods = new List<SingleDatePeriod>();

            while (reader.ReadNextRecord())
            {
                var date = ParseDateColumn(reader[map[PriceColumn.Date]]);
                var open = ParsePriceColumn(reader[map[PriceColumn.Open]]);
                var high = ParsePriceColumn(reader[map[PriceColumn.High]]);
                var low = ParsePriceColumn(reader[map[PriceColumn.Low]]);
                var close = ParsePriceColumn(reader[map[PriceColumn.Close]]);
                var volume = ParseVolumeColumn(reader[map[PriceColumn.Volume]]);

                if (close == null) throw new ArgumentNullException("", Strings.ParseError_CSV_data_is_corrupt__closing_price_cannot_be_null_for_any_period);
                stagedPeriods.Add(new SingleDatePeriod {Date = date, Open = open, High = high, Low = low, Close = close.Value, Volume = volume});
            }
            return stagedPeriods;
        }

        private string BuildCSVRecord(IPricePeriod period)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < ColumnHeaders.Count; i++)
            {
                var priceColumn = ColumnHeaders.ElementAt(i);
                switch (priceColumn.Key)
                {
                    case PriceColumn.Date:
                        builder.Append(period.Head.ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern));
                        break;
                    case PriceColumn.Open:
                        builder.Append(period.Open.ToString(CultureInfo.InvariantCulture));
                        break;
                    case PriceColumn.High:
                        builder.Append(period.High.ToString(CultureInfo.InvariantCulture));
                        break;
                    case PriceColumn.Low:
                        builder.Append(period.Low.ToString(CultureInfo.InvariantCulture));
                        break;
                    case PriceColumn.Close:
                        builder.Append(period.Close.ToString(CultureInfo.InvariantCulture));
                        break;
                    case PriceColumn.Volume:
                        builder.Append(period.Volume.ToString());
                        break;
                    case PriceColumn.None:
                        break;
                    default:
                        throw new InvalidOperationException(String.Format("Cannot write information for column {0}", priceColumn));
                }

                if (i < ColumnHeaders.Count - 1) builder.Append(",");
            }
            return builder.ToString();
        }

        private PriceColumn ParseColumnHeader(string header)
        {
            var li = header.ToLowerInvariant();
            var results = ColumnHeaders.Where(kvp => kvp.Value.ToLowerInvariant() == li).Select(kvp => kvp.Key).ToArray();

            return results.Count() == 1 ? results.First() : PriceColumn.None;
        }

        private static IList<IPricePeriod> BuildPricePeriods(IList<SingleDatePeriod> stagedPeriods, DateTime? impliedHead, DateTime? impliedTail, Resolution? impliedResolution)
        {
            stagedPeriods = stagedPeriods.OrderBy(period => period.Date).ToList();
            var resolution = SetResolution(stagedPeriods, impliedResolution);
            var pricePeriods = new List<IPricePeriod>();

            for (var i = 0; i < stagedPeriods.Count; i++)
            {
                var stagedPeriod = stagedPeriods[i];
                DateTime head;
                DateTime tail;

                // first head
                if (i == 0 && impliedHead.HasValue)
                {
                    // correct weekend
                    if (!impliedHead.Value.IsInTradingPeriod(resolution))
                    {
                        head = impliedHead.Value.NextTradingPeriodOpen(resolution);
                    }
                    else
                    {
                        // TODO: Replace else branch and introduce CalculateFirstSensibleHead()
                        head = impliedHead.Value;
                    }
                }
                else
                {
                    head = stagedPeriod.Date.CurrentPeriodOpen(resolution);
                }

                // last tail
                if (i == stagedPeriods.Count - 1 && impliedTail.HasValue)
                {
                    //correct weekend
                    if (!impliedTail.Value.IsInTradingPeriod(resolution))
                    {
                        impliedTail = impliedTail.Value.PreviousTradingPeriodClose(resolution);
                    }

                    // find the latest possible tail which makes sense
                    var lastSensibleTail = CalculateLastSensibleTail(pricePeriods, resolution, stagedPeriod);

                    // assign appropriate tail
                    tail = impliedTail.Value > lastSensibleTail
                        ? lastSensibleTail
                        : impliedTail.Value;
                }
                else
                {
                    tail = stagedPeriod.Date.CurrentPeriodClose(resolution);
                }

                if (head > tail) break; // provider gave us extra data beyond what we asked for, so stop here

                var open = stagedPeriod.Open ?? stagedPeriod.Close;
                var high = stagedPeriod.High ?? stagedPeriod.Close;
                var low = stagedPeriod.Low ?? stagedPeriod.Close;
                var period = new PricePeriod(head, tail, open, high, low, stagedPeriod.Close, stagedPeriod.Volume);
                pricePeriods.Add(period);
            }
            return pricePeriods;
        }

        private static DateTime CalculateLastSensibleTail(IReadOnlyList<IPricePeriod> pricePeriods, Resolution resolution, SingleDatePeriod stagedPeriod)
        {
            return pricePeriods.Count > 0
                       ? pricePeriods[pricePeriods.Count - 1].Tail.NextPeriodClose(resolution)
                       : stagedPeriod.Date.CurrentPeriodClose(resolution);
        }

        private static Resolution SetResolution(IList<SingleDatePeriod> periods, Resolution? impliedResolution)
        {
            if (impliedResolution.HasValue) return impliedResolution.Value;
            if (periods.Count >= 3)
            {
                var time1 = periods[0].Date;
                var time2 = periods[1].Date;
                DateTime time3;
                
                for (var i = 2; i < periods.Count; i++, time1 = time2, time2 = time3)
                {
                    var duration = time1 - time2;
                    time3 = periods[i].Date;
                    if (Math.Abs((time2 - time3).Ticks) == Math.Abs(duration.Ticks))
                    {
                        return DetermineResolution(duration);
                    }
                }
            }
            throw new InvalidOperationException(Strings.PriceHistoryCsvFile_DetermineResolution_Unable_to_determine_PriceSeriesResolution_of_data_periods_in_CSV_data_);
        }

        private static Resolution DetermineResolution(TimeSpan duration)
        {
            // ensure positive time, not negative time
            duration = new TimeSpan(Math.Abs(duration.Ticks));

            if (duration <= new TimeSpan(0, 0, 1))          // test for second periods
            {
                return Resolution.Seconds;
            }
            if (duration <= new TimeSpan(0, 1, 0))          // test for minute periods
            {
                return Resolution.Minutes;
            }
            if (duration <= new TimeSpan(1, 0, 0))          // test for hourly periods
            {
                return Resolution.Hours;
            }
            if (duration <= new TimeSpan(1, 0, 0, 0))       // test for daily periods
            {
                return Resolution.Days;
            }
            if (duration <= new TimeSpan(7, 0, 0, 0))       // test for weekly periods
            {
                return Resolution.Weeks;
            }
            if (duration <= new TimeSpan(31, 0, 0, 0))      // test for monthly periods
            {
                return Resolution.Months;
            }
            throw new ArgumentOutOfRangeException("duration", duration, Strings.PriceHistoryCsvFile_SetResolution_Given_duration_represents_an_unknown_PriceSeriesResolution_);
        }

        private struct SingleDatePeriod
        {
            public DateTime Date;
            public decimal? Open;
            public decimal? High;
            public decimal? Low;
            public decimal Close;
            public long? Volume;
        }

        protected virtual IDictionary<PriceColumn, string> ColumnHeaders
        {
            get { return DefaultColumnHeaders; }
        }

        protected virtual DateTime ParseDateColumn(string text)
        {
            var result = text.Trim();
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException("text", Strings.PriceHistoryCsvFile_ParseDateColumn_Parsed_date_was_returned_as_null_or_whitespace_);
            }
            return DateTime.Parse(result, CultureInfo.InvariantCulture);
        }

        protected virtual decimal? ParsePriceColumn(string text)
        {
            var result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? (decimal?) null
                       : Math.Abs(decimal.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        protected virtual long? ParseVolumeColumn(string text)
        {
            var result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? (long?) null
                       : Math.Abs(long.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }
    }
}
