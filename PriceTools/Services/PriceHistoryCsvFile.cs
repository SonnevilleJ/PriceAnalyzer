using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        private readonly IList<PricePeriod> _pricePeriods = new List<PricePeriod>();

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

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a list of all <see cref="IPricePeriod"/>s in the file.
        /// </summary>
        public IList<PricePeriod> PricePeriods
        {
            get
            {
                return _pricePeriods;
            }
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
                MapHeaders(reader);

                while (reader.ReadNextRecord())
                {
                    DateTime head = ParseDateColumn(reader[_map[PriceColumn.Date]]);
                    DateTime tail = head.AddDays(1);
                    decimal? open = ParsePriceColumn(reader[_map[PriceColumn.Open]]);
                    decimal? high = ParsePriceColumn(reader[_map[PriceColumn.High]]);
                    decimal? low = ParsePriceColumn(reader[_map[PriceColumn.Low]]);
                    decimal? close = ParsePriceColumn(reader[_map[PriceColumn.Close]]);
                    long? volume = ParseVolumeColumn(reader[_map[PriceColumn.Volume]]);

                    _pricePeriods.Add(PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close.Value, volume));
                }
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
                throw new ArgumentNullException("text", "Parsed date was returned as null or whitespace.");
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
