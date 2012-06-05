﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceTools.Data.Csv
{
    /// <summary>
    /// Represents a file store containing historical price data.
    /// </summary>
    public abstract class PriceHistoryCsvFile
    {
        #region Private Members

        private static readonly IDictionary<PriceColumn, string> DefaultColumnHeaders = new Dictionary<PriceColumn, string>
                                                                                            {
                                                                                                {PriceColumn.Date, "Date"},
                                                                                                {PriceColumn.Open, "Open"},
                                                                                                {PriceColumn.High, "High"},
                                                                                                {PriceColumn.Low, "Low"},
                                                                                                {PriceColumn.Close, "Close"},
                                                                                                {PriceColumn.Volume, "Volume"}
                                                                                            };

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a PriceHistoryCsvFile.
        /// </summary>
        protected PriceHistoryCsvFile()
        {
        }

        /// <summary>
        /// Constructs a PriceHistoryCsvFile.
        /// </summary>
        /// <param name="ticker">The ticker which should be assigned to the <see cref="PriceTools.PriceSeries"/>.</param>
        /// <param name="stream">The CSV data stream to parse.</param>
        /// <param name="impliedHead">The head of the price data contained in the CSV data.</param>
        /// <param name="impliedTail">The tail of the price data contained in the CSV data.</param>
        protected internal PriceHistoryCsvFile(string ticker, Stream stream, DateTime? impliedHead = null, DateTime? impliedTail = null)
        {
            Read(ticker, stream, impliedHead, impliedTail);
        }

        /// <summary>
        /// Constructs a PriceHistoryCsvFile.
        /// </summary>
        /// <param name="ticker">The ticker which should be assigned to the <see cref="PriceTools.PriceSeries"/>.</param>
        /// <param name="csvText">The raw CSV data to parse.</param>
        /// <param name="impliedHead">The head of the price data contained in the CSV data.</param>
        /// <param name="impliedTail">The tail of the price data contained in the CSV data.</param>
        protected internal PriceHistoryCsvFile(string ticker, string csvText, DateTime? impliedHead = null, DateTime? impliedTail = null)
        {
            Read(ticker, new StringReader(csvText), impliedHead, impliedTail);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a list of all <see cref="PricePeriod"/>s in the file.
        /// </summary>
        public IList<PricePeriod> PricePeriods
        {
            get { return PriceSeries.PricePeriods; }
        }

        /// <summary>
        /// Gets an <see cref="PriceTools.PriceSeries"/> containing the price data in the file.
        /// </summary>
        public PriceSeries PriceSeries { get; set; }
        
        #endregion

        #region Public Methods

        #region Read Methods

        /// <summary>
        /// Reads the contents of a Price History CSV file from disk.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="path">The path of the CSV file.</param>
        /// <param name="impliedHead"></param>
        /// <param name="impliedTail"></param>
        public void Read(string ticker, string path, DateTime? impliedHead = null, DateTime? impliedTail = null)
        {
            if (!File.Exists(path))
                throw new ArgumentOutOfRangeException("path", path, Strings.PriceHistoryCsvFile_Read_The_given_path_was_not_valid_or_the_file_could_not_be_found_);
            using (var fileStream = File.OpenRead(path))
            {
                Read(ticker, fileStream, impliedHead, impliedTail);
            }
        }

        /// <summary>
        /// Reads the contents of a Price History CSV file from a stream.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="stream">The stream to read.</param>
        /// <param name="impliedHead"></param>
        /// <param name="impliedTail"></param>
        public void Read(string ticker, Stream stream, DateTime? impliedHead = null, DateTime? impliedTail = null)
        {
            using (var reader = new StreamReader(stream))
            {
                Parse(reader, ticker, impliedHead, impliedTail);
            }
        }

        /// <summary>
        /// Reads price history information from CSV data.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="reader"></param>
        /// <param name="impliedHead"></param>
        /// <param name="impliedTail"></param>
        public void Read(string ticker, TextReader reader, DateTime? impliedHead = null, DateTime? impliedTail = null)
        {
            Parse(reader, ticker, impliedHead, impliedTail);
        }

        #endregion

        #region Write Methods

        public void Write(string path)
        {
            Write(File.OpenWrite(path));
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

        #endregion

        #endregion

        #region Private Methods

        private void Parse(TextReader stringReader, string ticker, DateTime? impliedHead, DateTime? impliedTail)
        {
            using (var reader = new CsvReader(stringReader, true))
            {
                var map = MapHeaders(reader);
                var stagedPeriods = StagePeriods(reader, map);
                PriceSeries = BuildPriceSeries(ticker, stagedPeriods, impliedHead, impliedTail);
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
            for (var i = 0; i < PriceColumnHeaders.Count; i++)
            {
                writer.Write(PriceColumnHeaders.ElementAt(i).Value);

                if (i < PriceColumnHeaders.Count - 1) writer.Write(",");
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

        private string BuildCSVRecord(PricePeriod period)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < PriceColumnHeaders.Count; i++)
            {
                var priceColumn = PriceColumnHeaders.ElementAt(i);
                switch (priceColumn.Key)
                {
                    case PriceColumn.Date:
                        builder.Append(period.Head.ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern));
                        break;
                    case PriceColumn.Open:
                        builder.Append(period.Open.ToString());
                        break;
                    case PriceColumn.High:
                        builder.Append(period.High.ToString());
                        break;
                    case PriceColumn.Low:
                        builder.Append(period.Low.ToString());
                        break;
                    case PriceColumn.Close:
                        builder.Append(period.Close.ToString());
                        break;
                    case PriceColumn.Volume:
                        builder.Append(period.Volume.ToString());
                        break;
                    case PriceColumn.None:
                        break;
                    default:
                        throw new InvalidOperationException(String.Format("Cannot write information for column {0}", priceColumn));
                }

                if (i < PriceColumnHeaders.Count - 1) builder.Append(",");
            }
            return builder.ToString();
        }

        /// <summary>
        /// Parses the column headers of a PriceHistoryCsvFile.
        /// </summary>
        /// <param name="header">A column header from the CSV file.</param>
        /// <returns>The <see cref="PriceColumn"/> of <paramref name="header"/>.</returns>
        private PriceColumn ParseColumnHeader(string header)
        {
            var li = header.ToLowerInvariant();
            var results = PriceColumnHeaders.Where(kvp => kvp.Value.ToLowerInvariant() == li).Select(kvp => kvp.Key);

            return results.Count() == 1 ? results.First() : PriceColumn.None;
        }

        #region Static parsing methods

        private static PriceSeries BuildPriceSeries(string ticker, IList<SingleDatePeriod> stagedPeriods, DateTime? impliedHead, DateTime? impliedTail)
        {
            stagedPeriods = stagedPeriods.OrderBy(period => period.Date).ToList();
            var resolution = SetResolution(stagedPeriods);
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker, resolution);
            var pricePeriods = new List<PricePeriod>();

            for (var i = 0; i < stagedPeriods.Count; i++)
            {
                var stagedPeriod = stagedPeriods[i];
                DateTime head;
                DateTime tail;

                // first head
                if (i == 0 && impliedHead.HasValue)
                {
                    // correct weekend
                    if (impliedHead.Value.DayOfWeek == DayOfWeek.Sunday || impliedHead.Value.DayOfWeek == DayOfWeek.Saturday)
                    {
                        head = impliedHead.Value.GetFollowingOpen();
                    }
                    else
                    {
                        head = impliedHead.Value;
                    }
                }
                else
                {
                    head = GetHead(stagedPeriod.Date, resolution);
                }

                // last tail
                if (i == stagedPeriods.Count - 1 && impliedTail.HasValue)
                {
                    //correct weekend
                    if (impliedTail.Value.DayOfWeek == DayOfWeek.Sunday || impliedTail.Value.DayOfWeek == DayOfWeek.Saturday)
                    {
                        impliedTail = impliedTail.Value.GetMostRecentClose();
                    }

                    // find the latest possible tail
                    var lastLogicalTail = pricePeriods[pricePeriods.Count - 1].Tail.AddPeriod(resolution);

                    // assign appropriate tail
                    tail = impliedTail.Value > lastLogicalTail ? lastLogicalTail : impliedTail.Value;
                }
                else
                {
                    tail = GetTail(stagedPeriod.Date, resolution);
                }

                var period = PricePeriodFactory.CreateStaticPricePeriod(head, tail, stagedPeriod.Open, stagedPeriod.High, stagedPeriod.Low, stagedPeriod.Close, stagedPeriod.Volume);
                pricePeriods.Add(period);
            }
            priceSeries.AddPriceData(pricePeriods);
            return priceSeries;
        }

        private static DateTime GetHead(DateTime date, Resolution resolution)
        {
            switch (resolution)
            {
                case Resolution.Days:
                    return date.GetMostRecentOpen();
                case Resolution.Weeks:
                    return date.GetMostRecentWeeklyOpen();
                default:
                    throw new ArgumentOutOfRangeException(null, String.Format(Strings.PriceHistoryCsvFile_GetHead_Unable_to_get_head_using_Price_Series_Resolution, resolution));
            }
        }

        private static DateTime GetTail(DateTime date, Resolution resolution)
        {
            switch (resolution)
            {
                case Resolution.Days:
                    return date.GetFollowingClose();
                case Resolution.Weeks:
                    return date.GetFollowingWeeklyClose();
                default:
                    throw new ArgumentOutOfRangeException(null, String.Format(Strings.PriceHistoryCsvFile_GetTail_Unable_to_get_tail_using_Price_Series_Resolution, resolution));
            }
        }

        private static Resolution SetResolution(IList<SingleDatePeriod> periods)
        {
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

        #endregion

        private struct SingleDatePeriod
        {
            public DateTime Date;
            public decimal? Open;
            public decimal? High;
            public decimal? Low;
            public decimal Close;
            public long? Volume;
        }

        #endregion

        #region Abstract/Virtual Methods

        /// <summary>
        /// Returns a dictionary with an ordered list of <see cref="PriceColumn"/>s and their corresponding string values.
        /// </summary>
        /// <remarks>
        /// When overridden in a derived class, this property defines localizable strings for column headers as well as the preferred order of columns.
        /// </remarks>
        protected virtual IDictionary<PriceColumn, string> PriceColumnHeaders
        {
            get { return DefaultColumnHeaders; }
        }

        /// <summary>
        /// Parses data from the Date column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed <see cref="DateTime"/>.</returns>
        protected virtual DateTime ParseDateColumn(string text)
        {
            var result = text.Trim();
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException("text", Strings.PriceHistoryCsvFile_ParseDateColumn_Parsed_date_was_returned_as_null_or_whitespace_);
            }
            return DateTime.Parse(result, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses data from one of the price columns of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed per-share price.</returns>
        protected virtual decimal? ParsePriceColumn(string text)
        {
            var result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? (decimal?) null
                       : Math.Abs(decimal.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Parses data from the volume column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed per-share price.</returns>
        protected virtual long? ParseVolumeColumn(string text)
        {
            var result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? (long?) null
                       : Math.Abs(long.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
