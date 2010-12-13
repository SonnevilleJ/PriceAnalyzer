using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Parses an <see cref="IPriceSeries"/> from Yahoo CSV files.
    /// </summary>
    public class YahooPriceSeriesCsvParser : IPriceSeriesCsvParser
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

        internal YahooPriceSeriesCsvParser()
        {
        }

        #endregion

        #region Private Methods

        private string[] GetHeaders(CsvReader reader)
        {
            throw new NotImplementedException();
        }

        private DataTable InitializeTransactionTable()
        {
            _dateColumn = new DataColumn("Date", typeof (DateTime));
            _orderColumn = new DataColumn("Order Type", typeof (OrderType));
            _symbolColumn = new DataColumn("Symbol", typeof (string));
            _sharesColumn = new DataColumn("Shares", typeof (double));
            _priceColumn = new DataColumn("Price", typeof (decimal));
            _commissionColumn = new DataColumn("Commission", typeof (decimal));

            DataTable table = new DataTable();
            table.Columns.Add(_dateColumn);
            table.Columns.Add(_orderColumn);
            table.Columns.Add(_symbolColumn);
            table.Columns.Add(_sharesColumn);
            table.Columns.Add(_priceColumn);
            table.Columns.Add(_commissionColumn);

            return table;
        }

        private DataTable InitializePriceTable()
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Parses an <see cref="IPriceSeries"/> from Yahoo CSV data.
        /// </summary>
        /// <param name="csvStream">A Yahoo CSV <see cref="Stream"/> containing price data.</param>
        /// <returns>An <see cref="IPriceSeries"/> containing the price data.</returns>
        public IPriceSeries ParsePriceSeries(Stream csvStream)
        {
            throw new NotImplementedException();
        }
    }
}
