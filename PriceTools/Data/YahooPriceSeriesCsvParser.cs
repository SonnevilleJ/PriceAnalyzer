using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Parses an <see cref="IPriceSeries"/> from Yahoo! CSV files.
    /// </summary>
    public sealed class YahooPriceSeriesCsvParser : IPriceSeriesCsvParser
    {
        #region Private Members

        private DataTable _table = new DataTable();
        private int _tableDateColumn;
        private int? _tableOpenColumn;
        private int? _tableHighColumn;
        private int? _tableLowColumn;
        private int _tableCloseColumn;
        private int? _tableVolumeColumn;
        private int? _tableDividendsColumn;
        private int _fileDateColumn;
        private int? _fileOpenColumn;
        private int? _fileHighColumn;
        private int? _fileLowColumn;
        private int _fileCloseColumn;
        private int? _fileVolumeColumn;
        private int? _fileDividendsColumn;
        private CsvReader _reader;

        #endregion

        #region Constructors

        internal YahooPriceSeriesCsvParser()
        {
        }

        /// <summary>
        /// Allows an <see cref="T:System.Object"/> to attempt to free resources and perform other cleanup operations before the <see cref="T:System.Object"/> is reclaimed by garbage collection.
        /// </summary>
        ~YahooPriceSeriesCsvParser()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        private void InitializePriceTable()
        {
            int fields = MapHeaders();
            _table = new DataTable();

            if ((fields & (int)PriceColumns.Date) == (int)PriceColumns.Date)
            {
                var dateColumn = new DataColumn("Date", typeof(DateTime));
                _table.Columns.Add(dateColumn);
                _tableDateColumn = _table.Columns.IndexOf(dateColumn);
            }
            if ((fields & (int)PriceColumns.Open) == (int)PriceColumns.Open)
            {
                var openColumn = new DataColumn("Open", typeof (decimal));
                _table.Columns.Add(openColumn);
                _tableOpenColumn = _table.Columns.IndexOf(openColumn);
            }
            if ((fields & (int)PriceColumns.High) == (int)PriceColumns.High)
            {
                var highColumn = new DataColumn("High", typeof(decimal));
                _table.Columns.Add(highColumn);
                _tableHighColumn = _table.Columns.IndexOf(highColumn);
            }
            if ((fields & (int)PriceColumns.Low) == (int)PriceColumns.Low)
            {
                var lowColumn = new DataColumn("Low", typeof(decimal));
                _table.Columns.Add(lowColumn);
                _tableLowColumn = _table.Columns.IndexOf(lowColumn);
            }
            if ((fields & (int)PriceColumns.Close) == (int)PriceColumns.Close)
            {
                var closeColumn = new DataColumn("Close", typeof(decimal));
                _table.Columns.Add(closeColumn);
                _tableCloseColumn = _table.Columns.IndexOf(closeColumn);
            }
            if ((fields & (int)PriceColumns.Volume) == (int)PriceColumns.Volume)
            {
                var volumeColumn = new DataColumn("Volume", typeof(decimal));
                _table.Columns.Add(volumeColumn);
                _tableVolumeColumn = _table.Columns.IndexOf(volumeColumn);
            }
            if ((fields & (int)PriceColumns.Dividends) == (int)PriceColumns.Dividends)
            {
                var dividendsColumn = new DataColumn("Dividends", typeof(decimal));
                _table.Columns.Add(dividendsColumn);
                _tableDividendsColumn = _table.Columns.IndexOf(dividendsColumn);
            }
        }

        private int CountFields()
        {
            int fields = MapHeaders();
            int count = 0;
            for(int i = 0; i < 8; i++, fields = fields >> 0x4)
            {
                if ((fields & 0x00000001) == 1)
                {
                    count++;
                }
            }
            return count;
        }

        private int MapHeaders()
        {
            int fields = 0x00000000;
            string[] headers = _reader.GetFieldHeaders();
            for (int i = 0; i < headers.Length; i++)
            {
                switch(headers[i])
                {
                    case "Date":
                        _fileDateColumn = i;
                        fields |= (int) PriceColumns.Date;
                        break;
                    case "Open":
                        _fileOpenColumn = i;
                        fields |= (int) PriceColumns.Open;
                        break;
                    case "High":
                        _fileHighColumn = i;
                        fields |= (int)PriceColumns.High;
                        break;
                    case "Low":
                        _fileLowColumn = i;
                        fields |= (int)PriceColumns.Low;
                        break;
                    case "Close":
                        _fileCloseColumn = i;
                        fields |= (int)PriceColumns.Close;
                        break;
                    case "Volume":
                        _fileVolumeColumn = i;
                        fields |= (int)PriceColumns.Volume;
                        break;
                    case "Dividends":
                        _fileDividendsColumn = i;
                        fields |= (int)PriceColumns.Dividends;
                        break;
                    default:
                        // ignore unknown columns
                        break;
                }
            }
            return fields;
        }
        
        private void ParseToDataTable()
        {
            while(_reader.ReadNextRecord())
            {
                object[] cells = new object[CountFields()];
                int count = 0;

                if (DoDate)
                {
                    _tableDateColumn = count++;
                    cells[(int) _tableDateColumn] = Convert.ToDateTime(_reader[(int) _fileDateColumn]);
                }
                if (DoOpen)
                {
                    _tableOpenColumn = count++;
                    cells[(int) _tableOpenColumn] = Convert.ToDecimal(_reader[(int) _fileOpenColumn]);
                }
                if (DoHigh)
                {
                    _tableHighColumn = count++;
                    cells[(int) _tableHighColumn] = Convert.ToDecimal(_reader[(int) _fileHighColumn]);
                }
                if(DoLow)
                {
                    _tableLowColumn = count++;
                    cells[(int) _tableLowColumn] = Convert.ToDecimal(_reader[(int) _fileLowColumn]);
                }
                if(DoClose)
                {
                    _tableCloseColumn = count++;
                    cells[(int) _tableCloseColumn] = Convert.ToDecimal(_reader[(int) _fileCloseColumn]);
                }
                if (DoVolume)
                {
                    _tableVolumeColumn = count++;
                    cells[(int) _tableVolumeColumn] = Convert.ToUInt64(_reader[(int) _fileVolumeColumn]);
                }
                if(DoDividends)
                {
                    _tableDividendsColumn = count++;
                    cells[(int) _tableDividendsColumn] = Convert.ToDecimal(_reader[(int) _fileDividendsColumn]);
                }

                _table.Rows.Add(cells);
                Records++;
            }
        }

        private IPriceSeries BuildPriceSeries()
        {
            ParseToDataTable();

            List<IPricePeriod> list = new List<IPricePeriod>();
            for(int i = 0; i < _table.Rows.Count; i++)
            {
                DataRow row = _table.Rows[i];
                DateTime date = new DateTime();
                decimal? open = null;
                decimal? high = null;
                decimal? low = null;
                decimal close = 0m;
                UInt64? volume = null;
                decimal? dividend = null;

                if (DoDate) date = (DateTime) row[(int) _tableDateColumn];
                if (DoOpen) open = row[(int) _tableOpenColumn] as decimal?;
                if (DoHigh) high = row[(int) _tableHighColumn] as decimal?;
                if (DoLow) low = row[(int) _tableLowColumn] as decimal?;
                if (DoClose) close = (decimal) row[(int) _tableCloseColumn];
                if (DoVolume) volume = Convert.ToUInt64(row[(int) _tableVolumeColumn]);
                if (DoDividends) dividend = row[(int) _tableDividendsColumn] as decimal?;

                list.Add(new PricePeriod(date, date, open, high, low, close, volume));
            }
            return new PriceSeries(list.ToArray());
        }

        #endregion

        #region Private Properties

        private bool DoDate
        {
            get { return _fileDateColumn != null; }
        }

        private bool DoOpen
        {
            get { return _fileOpenColumn != null; }
        }

        private bool DoHigh
        {
            get { return _fileHighColumn != null; }
        }

        private bool DoLow
        {
            get { return _fileLowColumn != null; }
        }

        private bool DoClose
        {
            get { return _fileCloseColumn != null; }
        }

        private bool DoVolume
        {
            get { return _fileVolumeColumn != null; }
        }

        private bool DoDividends
        {
            get { return _fileDividendsColumn != null; }
        }

        private ulong Records { get; set; }

        #endregion

        /// <summary>
        /// Parses an <see cref="IPriceSeries"/> from Yahoo! CSV data.
        /// </summary>
        /// <param name="csvStream">A Yahoo! CSV <see cref="Stream"/> containing price data.</param>
        /// <returns>An <see cref="IPriceSeries"/> containing the price data.</returns>
        public IPriceSeries ParsePriceSeries(Stream csvStream)
        {
            _reader = new CsvReader(new StreamReader(csvStream), true);
            InitializePriceTable();
            return BuildPriceSeries();
        }

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }
                if(_table != null)
                {
                    _table.Dispose();
                    _table = null;
                }
            }
        }

        #endregion
    }
}
