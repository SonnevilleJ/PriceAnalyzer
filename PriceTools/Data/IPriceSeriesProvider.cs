﻿using System;
using System.IO;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Parses an <see cref="IPriceSeries"/> from CSV data for a single ticker symbol.
    /// </summary>
    public interface IPriceSeriesProvider : IDisposable
    {
        /// <summary>
        /// Parses an <see cref="IPriceSeries"/> from CSV data.
        /// </summary>
        /// <param name="csvStream">A CSV <see cref="Stream"/> containing price data.</param>
        /// <returns>An <see cref="IPriceSeries"/> containing the price data.</returns>
        IPriceSeries ParsePriceSeries(Stream csvStream);

        /// <summary>
        /// Downloads a CSV data file from the provider.
        /// </summary>
        /// <param name="head">The beginning of the date range to price.</param>
        /// <param name="tail">The end of the date range to price.</param>
        /// <param name="symbol">The ticker symbol of the security to price.</param>
        /// <returns>A <see cref="Stream"/> containing the price data in CSV format.</returns>
        Stream DownloadPricesToCsv(DateTime head, DateTime tail, string symbol);

        /// <summary>
        /// Downloads a CSV data file from the provider.
        /// </summary>
        /// <param name="head">The beginning of the date range to price.</param>
        /// <param name="tail">The end of the date range to price.</param>
        /// <param name="symbol">The ticker symbol of the security to price.</param>
        /// <param name="resolution">The <see cref="PriceSeriesResolution"/> to use when retrieving price data.</param>
        /// <returns>A <see cref="Stream"/> containing the price data in CSV format.</returns>
        Stream DownloadPricesToCsv(DateTime head, DateTime tail, string symbol, PriceSeriesResolution resolution);

        /// <summary>
        /// Downloads a CSV data file from the provider.
        /// </summary>
        /// <param name="head">The beginning of the date range to price.</param>
        /// <param name="tail">The end of the date range to price.</param>
        /// <param name="index">The <see cref="StockIndex"/> to price.</param>
        /// <param name="resolution">The <see cref="PriceSeriesResolution"/> to use when retrieving price data.</param>
        /// <returns>A <see cref="Stream"/> containing the price data in CSV format.</returns>
        Stream DownloadPricesToCsv(DateTime head, DateTime tail, StockIndex index, PriceSeriesResolution resolution);

        /// <summary>
        /// Gets the ticker symbol of a stock index used by this provider.
        /// </summary>
        /// <param name="index">The <see cref="StockIndex"/> to lookup.</param>
        /// <returns></returns>
        string GetIndexTicker(StockIndex index);
    }
}
