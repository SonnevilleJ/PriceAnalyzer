using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Parses an <see cref="IPortfolio"/> from Fidelity CSV data.
    /// </summary>
    public class FidelityPortfolioCsvParser : IPortfolioCsvParser
    {
        #region Private Members

        private DataColumn _dateColumn;
        private DataColumn _orderColumn;
        private DataColumn _symbolColumn;
        private DataColumn _sharesColumn;
        private DataColumn _priceColumn;
        private DataColumn _commissionColumn;

        #endregion

        #region Constructors

        /// <summary>
        /// Allows an <see cref="T:System.Object"/> to attempt to free resources and perform other cleanup operations before the <see cref="T:System.Object"/> is reclaimed by garbage collection.
        /// </summary>
        ~FidelityPortfolioCsvParser()
        {
            Dispose(false);
        }

        #endregion

        /// <summary>
        /// Parses an <see cref="IPortfolio"/> from a given Fidelity CSV data stream.
        /// </summary>
        /// <param name="csvStream">The <see cref="Stream"/> to parse.</param>
        /// <returns>An <see cref="IPortfolio"/>.</returns>
        public IPortfolio ParsePortfolio(Stream csvStream)
        {
            var reader = new CsvReader(new StreamReader(csvStream), true);
            MapHeaders(reader);

            DataTable table = ParsePortfolioToDataTable(reader);
            return null;
        }

        /// <summary>
        /// Parses a <see cref="DataTable"/> from a given Fidelity CSV data stream.
        /// </summary>
        /// <param name="reader">The <see cref="Stream"/> to parse.</param>
        /// <returns>A <see cref="DataTable"/> of the CSV data.</returns>
        public DataTable ParsePortfolioToDataTable(CsvReader reader)
        {
            DataTable table = InitializePortfolioTable();
            MapHeaders(reader);

            while (reader.ReadNextRecord())
            {
                DataRow row = table.NewRow();
                row.BeginEdit();

                row.EndEdit();
            }

            return table;
        }

        #region Private Methods

        private void MapHeaders(CsvReader reader)
        {
            throw new NotImplementedException();
        }

        private DataTable InitializePortfolioTable()
        {
            _dateColumn = new DataColumn("Date", typeof(DateTime));
            _orderColumn = new DataColumn("Order Type", typeof(OrderType));
            _symbolColumn = new DataColumn("Symbol", typeof(string));
            _sharesColumn = new DataColumn("Shares", typeof(double));
            _priceColumn = new DataColumn("Price", typeof(decimal));
            _commissionColumn = new DataColumn("Commission", typeof(decimal));

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
            if(disposing)
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
