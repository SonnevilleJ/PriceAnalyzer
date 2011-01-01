using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    ///   Parses an <see cref = "IPortfolio" /> from CSV data for a single investment portfolio.
    /// </summary>
    public abstract class PortfolioCsvParser : IDisposable
    {
        #region Private Members

        private readonly IDictionary<TransactionColumns, int> _map = new Dictionary<TransactionColumns, int>(5);
        private readonly Stream _stream;
        private DataColumn _commissionColumn;
        private DataColumn _dateColumn;
        private DataColumn _orderColumn;
        private DataColumn _priceColumn;
        private DataColumn _sharesColumn;
        private DataColumn _symbolColumn;

        #endregion

        #region Constructors

        protected PortfolioCsvParser(Stream csvStream)
        {
            if (csvStream == null)
            {
                throw new ArgumentNullException("csvStream");
            }
            _stream = csvStream;
        }

        /// <summary>
        ///   Allows an <see cref = "T:System.Object" /> to attempt to free resources and perform other cleanup operations before the <see cref = "T:System.Object" /> is reclaimed by garbage collection.
        /// </summary>
        ~PortfolioCsvParser()
        {
            Dispose(false);
        }

        #endregion

        #region Abstract Properties

        protected abstract string DateHeader { get; }
        protected abstract string TransactionTypeHeader { get; }
        protected abstract string SymbolHeader { get; }
        protected abstract string SharesHeader { get; }
        protected abstract string PricePerShareHeader { get; }
        protected abstract string CommissionHeader { get; }

        #endregion

        #region Private Methods

        private void MapHeaders(CsvReader reader)
        {
            string[] headers = reader.GetFieldHeaders();
            int count = 0;
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] == DateHeader)
                {
                    _map.Add(TransactionColumns.Date, i);
                    count += (int) TransactionColumns.Date;
                }
                else if (headers[i] == TransactionTypeHeader)
                {
                    _map.Add(TransactionColumns.OrderType, i);
                    count += (int) TransactionColumns.OrderType;
                }
                else if (headers[i] == SymbolHeader)
                {
                    _map.Add(TransactionColumns.Symbol, i);
                    count += (int) TransactionColumns.Symbol;
                }
                else if (headers[i] == SharesHeader)
                {
                    _map.Add(TransactionColumns.Shares, i);
                    count += (int) TransactionColumns.Shares;
                }
                else if (headers[i] == PricePerShareHeader)
                {
                    _map.Add(TransactionColumns.PerSharePrice, i);
                    count += (int) TransactionColumns.PerSharePrice;
                }
                else if (headers[i] == CommissionHeader)
                {
                    _map.Add(TransactionColumns.Commission, i);
                    count += (int) TransactionColumns.Commission;
                }
            }
            if (count != 15)
            {
                throw new InvalidDataException("One or more data columns is missing in the stream.");
            }
        }

        private static IPortfolio ParseDataTableToIPortfolio(DataTable table)
        {
            throw new NotImplementedException();
        }

        private DataTable InitializePortfolioTable()
        {
            _dateColumn = new DataColumn("Date", typeof (DateTime));
            _orderColumn = new DataColumn("Order Type", typeof (OrderType));
            _symbolColumn = new DataColumn("Symbol", typeof (string));
            _sharesColumn = new DataColumn("Shares", typeof (double));
            _priceColumn = new DataColumn("Price", typeof (decimal));
            _commissionColumn = new DataColumn("Commission", typeof (decimal));

            using (DataTable table = new DataTable {Locale = CultureInfo.InvariantCulture})
            {
                table.Columns.Add(_dateColumn);
                table.Columns.Add(_orderColumn);
                table.Columns.Add(_symbolColumn);
                table.Columns.Add(_sharesColumn);
                table.Columns.Add(_priceColumn);
                table.Columns.Add(_commissionColumn);

                return table;
            }
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dateColumn.Dispose();
                _dateColumn = null;

                _orderColumn.Dispose();
                _orderColumn = null;

                _symbolColumn.Dispose();
                _symbolColumn = null;

                _sharesColumn.Dispose();
                _sharesColumn = null;

                _priceColumn.Dispose();
                _priceColumn = null;

                _commissionColumn.Dispose();
                _commissionColumn = null;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Parses an <see cref = "IPortfolio" /> from CSV data.
        /// </summary>
        /// <returns>An <see cref = "IPortfolio" /> containing the data from the CSV data.</returns>
        public IPortfolio ParsePortfolio()
        {
            using (CsvReader reader = new CsvReader(new StreamReader(_stream), true))
            {
                MapHeaders(reader);

                using (DataTable table = ParsePortfolioToDataTable(reader))
                {
                    return ParseDataTableToIPortfolio(table);
                }
            }
        }

        /// <summary>
        ///   Parses a <see cref = "DataTable" /> from a given CSV data stream.
        /// </summary>
        /// <param name = "reader">The <see cref = "Stream" /> to parse.</param>
        /// <returns>A <see cref = "DataTable" /> of the CSV data.</returns>
        public DataTable ParsePortfolioToDataTable(CsvReader reader)
        {
            using (DataTable table = InitializePortfolioTable())
            {
                while (reader.ReadNextRecord())
                {
                    DataRow row = table.NewRow();
                    row.BeginEdit();
                    row[_dateColumn] = reader[_map[TransactionColumns.Date]];
                    row[_orderColumn] = reader[_map[TransactionColumns.OrderType]];
                    row[_symbolColumn] = reader[_map[TransactionColumns.Symbol]];
                    row[_sharesColumn] = reader[_map[TransactionColumns.Shares]];
                    row[_priceColumn] = reader[_map[TransactionColumns.PerSharePrice]];
                    row[_commissionColumn] = reader[_map[TransactionColumns.Commission]];
                    row.EndEdit();
                }

                return table;
            }
        }

        #endregion
    }
}