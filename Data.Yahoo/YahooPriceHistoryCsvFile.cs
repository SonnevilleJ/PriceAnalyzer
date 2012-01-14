﻿using System;
using System.IO;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data;

namespace Data.Yahoo
{
    /// <summary>
    /// Represents a file store containing historical price data from Yahoo! Finance.
    /// </summary>
    public sealed class YahooPriceHistoryCsvFile : PriceHistoryCsvFile
    {
        #region Constructors

        /// <summary>
        /// Constructs a YahooPriceHistoryCsvFile.
        /// </summary>
        /// <param name="ticker">The ticker which should be assigned to the <see cref="IPriceSeries"/>.</param>
        /// <param name="stream">The CSV data stream to parse.</param>
        /// <param name="head">The head of the price data contained in the CSV data.</param>
        /// <param name="tail">The tail of the price data contained in the CSV data.</param>
        public YahooPriceHistoryCsvFile(string ticker, Stream stream, DateTime? head = null, DateTime? tail = null)
            : base(ticker, stream, head, tail)
        {
        }

        /// <summary>
        /// Constructs a YahooPriceHistoryCsvFile.
        /// </summary>
        /// <param name="ticker">The ticker which should be assigned to the <see cref="IPriceSeries"/>.</param>
        /// <param name="csvText">The raw CSV data to parse.</param>
        /// <param name="head">The head of the price data contained in the CSV data.</param>
        /// <param name="tail">The tail of the price data contained in the CSV data.</param>
        public YahooPriceHistoryCsvFile(string ticker, string csvText, DateTime? head = null, DateTime? tail = null)
            : base(ticker, csvText, head, tail)
        {
        }

        #endregion
    }
}