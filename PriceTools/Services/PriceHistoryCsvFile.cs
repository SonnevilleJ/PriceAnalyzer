﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceTools.Services
{
    /// <summary>
    /// Represents a file store containing historical price data.
    /// </summary>
    public abstract class PriceHistoryCsvFile
    {
        #region Private Members

        private readonly DateTime? _fileHead;
        private readonly DateTime? _fileTail;

        #endregion

        #region Constructors

        private PriceHistoryCsvFile(DateTime head, DateTime tail)
        {
            _fileHead = head;
            _fileTail = tail;
        }

        /// <summary>
        /// Constructs a PriceHistoryCsvFile.
        /// </summary>
        /// <param name="stream">The CSV data stream to parse.</param>
        protected internal PriceHistoryCsvFile(Stream stream)
        {
            Parse(stream);
        }

        /// <summary>
        /// Constructs a PriceHistoryCsvFile.
        /// </summary>
        /// <param name="csvText">The raw CSV data to parse.</param>
        protected internal PriceHistoryCsvFile(string csvText)
        {
            Parse(csvText);
        }

        /// <summary>
        /// Constructs a PriceHistoryCsvFile.
        /// </summary>
        /// <param name="stream">The CSV data stream to parse.</param>
        /// <param name="head">The head of the price data contained in the CSV data.</param>
        /// <param name="tail">The tail of the price data contained in the CSV data.</param>
        protected internal PriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail)
            : this(head, tail)
        {
            Parse(stream);
        }

        /// <summary>
        /// Constructs a PriceHistoryCsvFile.
        /// </summary>
        /// <param name="csvText">The raw CSV data to parse.</param>
        /// <param name="head">The head of the price data contained in the CSV data.</param>
        /// <param name="tail">The tail of the price data contained in the CSV data.</param>
        protected internal PriceHistoryCsvFile(string csvText, DateTime head, DateTime tail)
            : this(head, tail)
        {
            Parse(csvText);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a list of all <see cref="IPricePeriod"/>s in the file.
        /// </summary>
        public IList<IPricePeriod> PricePeriods
        {
            get { return PriceSeries.PricePeriods; }
        }

        /// <summary>
        /// Gets an <see cref="IPriceSeries"/> containing the price data in the file.
        /// </summary>
        public IPriceSeries PriceSeries { get; private set; }

        #endregion

        #region Private Methods

        private void Parse(Stream stream)
        {
            Parse(new StreamReader(stream));
        }

        private void Parse(string csvText)
        {
            Parse(new StringReader(csvText));
        }

        private void Parse(TextReader stringReader)
        {
            using (CsvReader reader = new CsvReader(stringReader, true))
            {
                ParseData(reader);
            }
        }

        private void ParseData(CsvReader reader)
        {
            var map = MapHeaders(reader);
            var stagedPeriods = StagePeriods(reader, map);
            PriceSeries = BuildPriceSeries(stagedPeriods, _fileHead, _fileTail);
        }

        private IDictionary<PriceColumn, int> MapHeaders(CsvReader reader)
        {
            string[] headers = reader.GetFieldHeaders();
            var dictionary = new Dictionary<PriceColumn, int>();
            for (int i = 0; i < headers.Length; i++)
            {
                PriceColumn column = ParseColumnHeader(headers[i]);
                if (column != PriceColumn.None)
                {
                    dictionary.Add(column, i);
                }
            }
            return dictionary;
        }

        private IList<BasicPeriod> StagePeriods(CsvReader reader, IDictionary<PriceColumn, int> map)
        {
            IList<BasicPeriod> stagedPeriods = new List<BasicPeriod>();

            while (reader.ReadNextRecord())
            {
                DateTime date = ParseDateColumn(reader[map[PriceColumn.Date]]);
                decimal? open = ParsePriceColumn(reader[map[PriceColumn.Open]]);
                decimal? high = ParsePriceColumn(reader[map[PriceColumn.High]]);
                decimal? low = ParsePriceColumn(reader[map[PriceColumn.Low]]);
                decimal? close = ParsePriceColumn(reader[map[PriceColumn.Close]]);
                long? volume = ParseVolumeColumn(reader[map[PriceColumn.Volume]]);

                if (close == null) throw new ArgumentNullException("", Strings.ParseError_CSV_data_is_corrupt__closing_price_cannot_be_null_for_any_period);
                stagedPeriods.Add(new BasicPeriod {Date = date, Open = open, High = high, Low = low, Close = close.Value, Volume = volume});
            }
            return stagedPeriods;
        }

        #region Static parsing methods

        private static PriceSeries BuildPriceSeries(IList<BasicPeriod> stagedPeriods, DateTime? seriesHead, DateTime? seriesTail)
        {
            stagedPeriods = stagedPeriods.OrderBy(period => period.Date).ToList();
            var resolution = DetermineResolution(stagedPeriods);
            var priceSeries = new PriceSeries(resolution);

            for (int i = 0; i < stagedPeriods.Count; i++)
            {
                var stagedPeriod = stagedPeriods[i];
                var head = i == 0 && seriesHead.HasValue ? seriesHead.Value.GetCurrentOrFollowingTradingDay() : GetHead(stagedPeriod.Date, resolution);
                var tail = i == stagedPeriods.Count - 1 && seriesTail.HasValue ? seriesTail.Value : GetTail(stagedPeriod.Date, resolution);
                priceSeries.DataPeriods.Add(PricePeriodFactory.CreateStaticPricePeriod(head, tail, stagedPeriod.Open, stagedPeriod.High,
                                                                                       stagedPeriod.Low, stagedPeriod.Close, stagedPeriod.Volume));
            }
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

        private static Resolution DetermineResolution(IList<BasicPeriod> periods)
        {
            if (periods.Count >= 3)
            {
                DateTime time1 = periods[0].Date;
                DateTime time2 = periods[1].Date;

                for (int i = 2; i < periods.Count; i++)
                {
                    TimeSpan duration = time1 - time2;
                    DateTime time3 = periods[i].Date;
                    if (Math.Abs((time2 - time3).Ticks) == Math.Abs(duration.Ticks))
                    {
                        return SetResolution(duration);
                    }
                    time1 = time2;
                    time2 = time3;
                }
            }
            throw new InvalidOperationException(Strings.PriceHistoryCsvFile_DetermineResolution_Unable_to_determine_PriceSeriesResolution_of_data_periods_in_CSV_data_);
        }

        private static Resolution SetResolution(TimeSpan duration)
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

        private struct BasicPeriod
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
        /// Parses the column headers of a PriceHistoryCsvFile.
        /// </summary>
        /// <param name="header">A column header from the CSV file.</param>
        /// <returns>The <see cref="PriceColumn"/> of <paramref name="header"/>.</returns>
        protected virtual PriceColumn ParseColumnHeader(string header)
        {
            switch (header.ToLowerInvariant())
            {
                case "date":
                    return PriceColumn.Date;
                case "open":
                    return PriceColumn.Open;
                case "high":
                    return PriceColumn.High;
                case "low":
                    return PriceColumn.Low;
                case "close":
                    return PriceColumn.Close;
                case "volume":
                    return PriceColumn.Volume;
                default:
                    return PriceColumn.None;
            }
        }

        /// <summary>
        /// Parses data from the Date column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed <see cref="DateTime"/>.</returns>
        protected virtual DateTime ParseDateColumn(string text)
        {
            string result = text.Trim();
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException("text", Strings.ParseError_Parsed_date_was_returned_as_null_or_whitespace);
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
            string result = text.Trim();
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
            string result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? (long?) null
                       : Math.Abs(long.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
