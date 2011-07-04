using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;

namespace Sonneville.PriceTools.Services
{
    /// <summary>
    /// Represents a file store containing historical price data.
    /// </summary>
    public abstract class PriceHistoryCsvFile
    {
        #region Private Members

        private readonly IDictionary<PriceColumn, int> _map = new Dictionary<PriceColumn, int>();
        private readonly PriceSeries _priceSeries = new PriceSeries();

        #endregion

        #region Constructors

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

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a list of all <see cref="IPricePeriod"/>s in the file.
        /// </summary>
        public IList<IPricePeriod> PricePeriods
        {
            get { return _priceSeries.PricePeriods; }
        }

        /// <summary>
        /// Gets a <see cref="PriceSeries"/> containing the price data in the file.
        /// </summary>
        public PriceSeries PriceSeries
        {
            get { return _priceSeries; }
        }

        #endregion

        #region Private Methods

        private void MapHeaders(CsvReader reader)
        {
            string[] headers = reader.GetFieldHeaders();
            for (int i = 0; i < headers.Length; i++)
            {
                PriceColumn column = ParseColumnHeader(headers[i]);
                if (column != PriceColumn.None)
                {
                    _map.Add(column, i);
                }
            }
        }

        private void Parse(Stream stream)
        {
            using (CsvReader reader = new CsvReader(new StreamReader(stream), true))
            {
                ParseData(reader);
            }
        }

        private void Parse(string csvText)
        {
            using (CsvReader reader = new CsvReader(new StringReader(csvText), true))
            {
                ParseData(reader);
            }
        }

        private void ParseData(CsvReader reader)
        {
            MapHeaders(reader);

            while (reader.ReadNextRecord())
            {
                DateTime head = ParseDateColumn(reader[_map[PriceColumn.Date]]);
                DateTime tail = head;
                decimal? open = ParsePriceColumn(reader[_map[PriceColumn.Open]]);
                decimal? high = ParsePriceColumn(reader[_map[PriceColumn.High]]);
                decimal? low = ParsePriceColumn(reader[_map[PriceColumn.Low]]);
                decimal? close = ParsePriceColumn(reader[_map[PriceColumn.Close]]);
                long? volume = ParseVolumeColumn(reader[_map[PriceColumn.Volume]]);

                if (close == null) throw new ArgumentNullException("", Strings.ParseError_CSV_data_is_corrupt__closing_price_cannot_be_null_for_any_period_);
                _priceSeries.DataPeriods.Add(PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close.Value, volume));
            }
        }

        #endregion

        #region Abstract/Virtual Methods

        /// <summary>
        /// Parses the column headers of a PriceHistoryCsvFile.
        /// </summary>
        /// <param name="header">A column header from the CSV file.</param>
        /// <returns>The <see cref="PriceColumn"/> of <paramref name="header"/>.</returns>
        protected abstract PriceColumn ParseColumnHeader(string header);

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
