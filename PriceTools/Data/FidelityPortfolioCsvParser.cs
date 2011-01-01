using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    ///   Parses an <see cref = "IPortfolio" /> from Fidelity CSV data.
    /// </summary>
    public class FidelityPortfolioCsvParser : IPortfolioCsvParser
    {
        private readonly IDictionary<TransactionColumns, int> _map =
            new Dictionary<TransactionColumns, int>(5);

        private readonly Stream _stream;
        private DataColumn _commissionColumn;
        private DataColumn _dateColumn;
        private DataColumn _orderColumn;
        private DataColumn _priceColumn;
        private DataColumn _sharesColumn;
        private DataColumn _symbolColumn;

        #region Constructors

        /// <summary>
        ///   Creates a
        /// </summary>
        /// <param name = "csvStream"></param>
        public FidelityPortfolioCsvParser(Stream csvStream)
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
        ~FidelityPortfolioCsvParser()
        {
            Dispose(false);
        }

        #endregion

        #region IPortfolioCsvParser Members

        /// <summary>
        ///   Parses an <see cref = "IPortfolio" /> from a given Fidelity CSV data stream.
        /// </summary>
        /// <returns>An <see cref = "IPortfolio" />.</returns>
        public IPortfolio ParsePortfolio()
        {
            CsvReader reader = new CsvReader(new StreamReader(_stream), true);
            MapHeaders(reader);

            DataTable table = ParsePortfolioToDataTable(reader);
            return ParseDataTableToIPortfolio(table);
        }

        #endregion

        private static IPortfolio ParseDataTableToIPortfolio(DataTable table)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Parses a <see cref = "DataTable" /> from a given Fidelity CSV data stream.
        /// </summary>
        /// <param name = "reader">The <see cref = "Stream" /> to parse.</param>
        /// <returns>A <see cref = "DataTable" /> of the CSV data.</returns>
        public DataTable ParsePortfolioToDataTable(CsvReader reader)
        {
            DataTable table = InitializePortfolioTable();

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

        #region Private Methods

        private void MapHeaders(CsvReader reader)
        {
            string[] headers = reader.GetFieldHeaders();
            int count = 0;
            for (int i = 0; i < headers.Length; i++)
            {
                switch (headers[i])
                {
                    case "Trade Date":
                        _map.Add(TransactionColumns.Date, i);
                        count += (int) TransactionColumns.Date;
                        break;
                    case "Action":
                        _map.Add(TransactionColumns.OrderType, i);
                        count += (int) TransactionColumns.OrderType;
                        break;
                    case "Symbol":
                        _map.Add(TransactionColumns.Symbol, i);
                        count += (int) TransactionColumns.Symbol;
                        break;
                    case "Quantity":
                        _map.Add(TransactionColumns.Shares, i);
                        count += (int) TransactionColumns.Shares;
                        break;
                    case "Price ($)":
                        _map.Add(TransactionColumns.PerSharePrice, i);
                        count += (int) TransactionColumns.PerSharePrice;
                        break;
                    case "Commission ($)":
                        _map.Add(TransactionColumns.Commission, i);
                        count += (int) TransactionColumns.Commission;
                        break;
                    default:
                        // ignore other columns
                        break;
                }
            }
            if (count != 15)
            {
                throw new InvalidDataException("One or more data columns is missing in the stream.");
            }
        }

        private DataTable InitializePortfolioTable()
        {
            _dateColumn = new DataColumn("Date", typeof (DateTime));
            _orderColumn = new DataColumn("Order Type", typeof (OrderType));
            _symbolColumn = new DataColumn("Symbol", typeof (string));
            _sharesColumn = new DataColumn("Shares", typeof (double));
            _priceColumn = new DataColumn("Price", typeof (decimal));
            _commissionColumn = new DataColumn("Commission", typeof (decimal));

            DataTable table = new DataTable {Locale = CultureInfo.InvariantCulture};
            table.Columns.Add(_dateColumn);
            table.Columns.Add(_orderColumn);
            table.Columns.Add(_symbolColumn);
            table.Columns.Add(_sharesColumn);
            table.Columns.Add(_priceColumn);
            table.Columns.Add(_commissionColumn);

            return table;
        }

        #endregion

        #region IDisposable

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
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
    }
}