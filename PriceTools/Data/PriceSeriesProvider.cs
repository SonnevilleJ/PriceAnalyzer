using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    ///   Parses an <see cref = "IPriceSeries" /> from CSV data for a single ticker symbol.
    /// </summary>
    public abstract class PriceSeriesProvider : IDisposable
    {
        private int _fileCloseColumn;
        private int _fileDateColumn;
        private int? _fileDividendsColumn;
        private int? _fileHighColumn;
        private int? _fileLowColumn;
        private int? _fileOpenColumn;
        private int? _fileVolumeColumn;
        private CsvReader _reader;
        private DataTable _table;
        private int _tableCloseColumn;
        private int _tableDateColumn;
        private int? _tableDividendsColumn;
        private int? _tableHighColumn;
        private int? _tableLowColumn;
        private int? _tableOpenColumn;
        private int? _tableVolumeColumn;

        #region Constructors

        /// <summary>
        ///   Constructs a new PriceSeriesProvider.
        /// </summary>
        protected PriceSeriesProvider()
        {
            _table = new DataTable {Locale = CultureInfo.InvariantCulture};
        }

        /// <summary>
        ///   Allows an <see cref = "T:System.Object" /> to attempt to free resources and perform other cleanup operations before the <see cref = "T:System.Object" /> is reclaimed by garbage collection.
        /// </summary>
        ~PriceSeriesProvider()
        {
            Dispose(true);
        }

        #endregion

        private ulong Records { get; set; }

        /// <summary>
        ///   Gets the ticker symbol for a <see cref = "StockIndex" /> used by this <see cref = "PriceSeriesProvider" />.
        /// </summary>
        /// <param name = "index">The <see cref = "StockIndex" /> to retrieve.</param>
        /// <returns>A string representing the ticker symbol of the requested <see cref = "StockIndex" />.</returns>
        public abstract string GetIndexTicker(StockIndex index);

        #region File Headers

        /// <summary>
        ///   Represents the string qualifier used in the Date column header.
        /// </summary>
        protected abstract string DateHeader { get; }

        /// <summary>
        ///   Represents the string qualifier used in the Opening Price column header.
        /// </summary>
        protected abstract string OpenHeader { get; }

        /// <summary>
        ///   Represents the string qualifier used in the High Price column header.
        /// </summary>
        protected abstract string HighHeader { get; }

        /// <summary>
        ///   Represents the string qualifier used in the Low Price column header.
        /// </summary>
        protected abstract string LowHeader { get; }

        /// <summary>
        ///   Represents the string qualifier used in the Closing Price column header.
        /// </summary>
        protected abstract string CloseHeader { get; }

        /// <summary>
        ///   Represents the string qualifier used in the Volume column header.
        /// </summary>
        protected abstract string VolumeHeader { get; }

        /// <summary>
        ///   Represents the string qualifier used in the Dividend Amount column header.
        /// </summary>
        protected abstract string DividendHeader { get; }

        #endregion

        #region URL Management

        /// <summary>
        ///   Gets the base component of the URL used to retrieve the price history.
        /// </summary>
        /// <returns>A URL scheme, host, path, and miscellaneous query string.</returns>
        protected abstract string GetUrlBase();

        /// <summary>
        ///   Gets the ticker symbol component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <param name = "symbol">The ticker symbol to retrieve.</param>
        /// <returns>A partial URL query string containing the given ticker symbol.</returns>
        protected abstract string GetUrlTicker(string symbol);

        /// <summary>
        ///   Gets the beginning date component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <param name = "head">The first period for which to request price history.</param>
        /// <returns>A partial URL query string containing the given beginning date.</returns>
        protected abstract string GetUrlHeadDate(DateTime head);

        /// <summary>
        ///   Gets the ending date component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <param name = "tail">The last period for which to request price history.</param>
        /// <returns>A partial URL query string containing the given ending date.</returns>
        protected abstract string GetUrlTailDate(DateTime tail);

        /// <summary>
        ///   Gets the <see cref = "PriceSeriesResolution" /> component of the URL query string used to retrieve price history.
        /// </summary>
        /// <param name = "resolution">The <see cref = "PriceSeriesResolution" /> to request.</param>
        /// <returns>A partial URL query string containing a marker which requests the given <see cref = "PriceSeriesResolution" />.</returns>
        protected abstract string GetUrlResolution(PriceSeriesResolution resolution);

        /// <summary>
        ///   Gets the dividend component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <returns>A partial URL query string containing a marker which requests dividend data.</returns>
        protected abstract string GetUrlDividends();

        /// <summary>
        ///   Gets the CSV marker component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <returns>A partial URL qery string containing a marker which requests CSV data.</returns>
        protected abstract string GetUrlCsvMarker();

        /// <summary>
        ///   Builds the entire URL used to retrieve the price history.
        /// </summary>
        /// <param name = "head">The first period in the requested price history period.</param>
        /// <param name = "tail">The last period in the requested price history period.</param>
        /// <param name = "symbol">The ticker symbol for which to request price history.</param>
        /// <param name = "resolution">The <see cref = "PriceSeriesResolution" /> of the price history to request.</param>
        /// <returns>A fully formed URL that will return the requested price history.</returns>
        protected virtual string FormUrlQuery(DateTime head, DateTime tail, string symbol,
                                              PriceSeriesResolution resolution)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(GetUrlBase());
            builder.Append(GetUrlTicker(symbol));
            builder.Append(GetUrlHeadDate(head));
            builder.Append(GetUrlTailDate(tail));
            builder.Append(GetUrlResolution(resolution));
            builder.Append(GetUrlCsvMarker());

            return builder.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///   Maps the column headers of the data file. Columns are tracked via a bitmask.
        /// </summary>
        /// <returns>A bitmask representing the columns found in the data file.</returns>
        private int MapHeaders()
        {
            int fields = 0x00000000;
            string[] headers = _reader.GetFieldHeaders();
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] == DateHeader)
                {
                    _fileDateColumn = i;
                    fields |= (int) PriceColumns.Date;
                }
                else if (headers[i] == OpenHeader)
                {
                    _fileOpenColumn = i;
                    fields |= (int) PriceColumns.Open;
                }
                else if (headers[i] == HighHeader)
                {
                    _fileHighColumn = i;
                    fields |= (int) PriceColumns.High;
                }
                else if (headers[i] == LowHeader)
                {
                    _fileLowColumn = i;
                    fields |= (int) PriceColumns.Low;
                }
                else if (headers[i] == CloseHeader)
                {
                    _fileCloseColumn = i;
                    fields |= (int) PriceColumns.Close;
                }
                else if (headers[i] == VolumeHeader)
                {
                    _fileVolumeColumn = i;
                    fields |= (int) PriceColumns.Volume;
                }
                else if (headers[i] == DividendHeader)
                {
                    _fileDividendsColumn = i;
                    fields |= (int) PriceColumns.Dividends;
                }
            }
            return fields;
        }

        private void InitializePriceTable()
        {
            int fields = MapHeaders();
            _table = new DataTable {Locale = CultureInfo.InvariantCulture};

            if ((fields & (int) PriceColumns.Date) == (int) PriceColumns.Date)
            {
                DataColumn dateColumn = new DataColumn("Date", typeof (DateTime));
                _table.Columns.Add(dateColumn);
                _tableDateColumn = _table.Columns.IndexOf(dateColumn);
            }
            if ((fields & (int) PriceColumns.Open) == (int) PriceColumns.Open)
            {
                DataColumn openColumn = new DataColumn("Open", typeof (decimal));
                _table.Columns.Add(openColumn);
                _tableOpenColumn = _table.Columns.IndexOf(openColumn);
            }
            if ((fields & (int) PriceColumns.High) == (int) PriceColumns.High)
            {
                DataColumn highColumn = new DataColumn("High", typeof (decimal));
                _table.Columns.Add(highColumn);
                _tableHighColumn = _table.Columns.IndexOf(highColumn);
            }
            if ((fields & (int) PriceColumns.Low) == (int) PriceColumns.Low)
            {
                DataColumn lowColumn = new DataColumn("Low", typeof (decimal));
                _table.Columns.Add(lowColumn);
                _tableLowColumn = _table.Columns.IndexOf(lowColumn);
            }
            if ((fields & (int) PriceColumns.Close) == (int) PriceColumns.Close)
            {
                DataColumn closeColumn = new DataColumn("Close", typeof (decimal));
                _table.Columns.Add(closeColumn);
                _tableCloseColumn = _table.Columns.IndexOf(closeColumn);
            }
            if ((fields & (int) PriceColumns.Volume) == (int) PriceColumns.Volume)
            {
                DataColumn volumeColumn = new DataColumn("Volume", typeof (decimal));
                _table.Columns.Add(volumeColumn);
                _tableVolumeColumn = _table.Columns.IndexOf(volumeColumn);
            }
            if ((fields & (int) PriceColumns.Dividends) == (int) PriceColumns.Dividends)
            {
                DataColumn dividendsColumn = new DataColumn("Dividends", typeof (decimal));
                _table.Columns.Add(dividendsColumn);
                _tableDividendsColumn = _table.Columns.IndexOf(dividendsColumn);
            }
        }

        private int CountFields()
        {
            int fields = MapHeaders();
            int count = 0;
            for (int i = 0; i < 8; i++, fields = fields >> 0x4)
            {
                if ((fields & 0x00000001) == 1)
                {
                    count++;
                }
            }
            return count;
        }

        private void ParseToDataTable()
        {
            while (_reader.ReadNextRecord())
            {
                object[] cells = new object[CountFields()];
                int count = 0;

                _tableDateColumn = count++;
                cells[_tableDateColumn] = Convert.ToDateTime(_reader[_fileDateColumn], CultureInfo.InvariantCulture);
                if (_fileOpenColumn != null)
                {
                    _tableOpenColumn = count++;
                    cells[(int) _tableOpenColumn] = Convert.ToDecimal(_reader[(int) _fileOpenColumn],
                                                                      CultureInfo.InvariantCulture);
                }
                if (_fileHighColumn != null)
                {
                    _tableHighColumn = count++;
                    cells[(int) _tableHighColumn] = Convert.ToDecimal(_reader[(int) _fileHighColumn],
                                                                      CultureInfo.InvariantCulture);
                }
                if (_fileLowColumn != null)
                {
                    _tableLowColumn = count++;
                    cells[(int) _tableLowColumn] = Convert.ToDecimal(_reader[(int) _fileLowColumn],
                                                                     CultureInfo.InvariantCulture);
                }
                _tableCloseColumn = count++;
                cells[_tableCloseColumn] = Convert.ToDecimal(_reader[_fileCloseColumn], CultureInfo.InvariantCulture);
                if (_fileVolumeColumn != null)
                {
                    _tableVolumeColumn = count++;
                    cells[(int) _tableVolumeColumn] = Convert.ToUInt64(_reader[(int) _fileVolumeColumn],
                                                                       CultureInfo.InvariantCulture);
                }
                if (_fileDividendsColumn != null)
                {
                    _tableDividendsColumn = count++;
                    cells[(int) _tableDividendsColumn] = Convert.ToDecimal(_reader[(int) _fileDividendsColumn],
                                                                           CultureInfo.InvariantCulture);
                }

                _table.Rows.Add(cells);
                Records++;
            }
        }

        private IPriceSeries BuildPriceSeries()
        {
            InitializePriceTable();
            ParseToDataTable();

            List<IPricePeriod> list = new List<IPricePeriod>();
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                DataRow row = _table.Rows[i];
                decimal? open = null;
                decimal? high = null;
                decimal? low = null;
                decimal close = 0m;
                UInt64? volume = null;
                decimal? dividend = null;

                DateTime date = (DateTime) row[_tableDateColumn];
                if (_fileOpenColumn != null) open = row[(int) _tableOpenColumn] as decimal?;
                if (_fileHighColumn != null) high = row[(int) _tableHighColumn] as decimal?;
                if (_fileLowColumn != null) low = row[(int) _tableLowColumn] as decimal?;
                close = (decimal) row[_tableCloseColumn];
                if (_fileVolumeColumn != null)
                    volume = Convert.ToUInt64(row[(int) _tableVolumeColumn], CultureInfo.InvariantCulture);
                if (_fileDividendsColumn != null) dividend = row[(int) _tableDividendsColumn] as decimal?;

                list.Add(new PricePeriod(date, date, open, high, low, close, volume));
            }
            return new PriceSeries(list.ToArray());
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Downloads a CSV data file containing daily price history.
        /// </summary>
        /// <param name = "head">The beginning of the date range to price.</param>
        /// <param name = "tail">The end of the date range to price.</param>
        /// <param name = "symbol">The ticker symbol of the security to price.</param>
        /// <returns>A <see cref = "Stream" /> containing the price data in CSV format.</returns>
        /// <exception cref = "System.Net.WebException"></exception>
        public Stream DownloadPricesToCsv(DateTime head, DateTime tail, string symbol)
        {
            return DownloadPricesToCsv(head, tail, symbol, PriceSeriesResolution.Days);
        }

        /// <summary>
        ///   Downloads a CSV data file containing price history.
        /// </summary>
        /// <param name = "head">The beginning of the date range to price.</param>
        /// <param name = "tail">The end of the date range to price.</param>
        /// <param name = "symbol">The ticker symbol of the security to price.</param>
        /// <param name = "resolution">The <see cref = "PriceSeriesResolution" /> to use when retrieving price data.</param>
        /// <returns>A <see cref = "Stream" /> containing the price data in CSV format.</returns>
        /// <exception cref = "System.Net.WebException"></exception>
        public Stream DownloadPricesToCsv(DateTime head, DateTime tail, string symbol, PriceSeriesResolution resolution)
        {
            Stream result;
            string url = FormUrlQuery(head, tail, symbol, resolution);
            WebClient client = new WebClient();
            try
            {
                result = client.OpenRead(url);
            }
            catch (WebException)
            {
                // Pass exception on to client code
                throw;
            }
            return result;
        }

        /// <summary>
        ///   Downloads a CSV data file
        /// </summary>
        /// <param name = "head">The beginning of the date range to price.</param>
        /// <param name = "tail">The end of the date range to price.</param>
        /// <param name = "index">The <see cref = "StockIndex" /> to price.</param>
        /// <param name = "resolution">The <see cref = "PriceSeriesResolution" /> to use when retrieving price data.</param>
        /// <returns>A <see cref = "Stream" /> containing the price data in CSV format.</returns>
        /// <exception cref = "System.Net.WebException"></exception>
        public Stream DownloadPricesToCsv(DateTime head, DateTime tail, StockIndex index,
                                          PriceSeriesResolution resolution)
        {
            return DownloadPricesToCsv(head, tail, GetIndexTicker(index), resolution);
        }

        /// <summary>
        ///   Parses an <see cref = "IPriceSeries" /> from Yahoo! CSV data.
        /// </summary>
        /// <param name = "csvStream">A Yahoo! CSV <see cref = "Stream" /> containing price data.</param>
        /// <returns>An <see cref = "IPriceSeries" /> containing the price data.</returns>
        public IPriceSeries ParsePriceSeries(Stream csvStream)
        {
            _reader = new CsvReader(new StreamReader(csvStream), true);
            return BuildPriceSeries();
        }

        #endregion

        #region IDisposable Implemenation

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting umanaged resources.
        /// </summary>
        /// <param name = "disposing">A value indicating whether or not the object should be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }
                if (_table != null)
                {
                    _table.Dispose();
                    _table = null;
                }
            }
        }

        #endregion
    }
}