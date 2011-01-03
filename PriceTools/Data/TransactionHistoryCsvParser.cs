using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    ///   Parses a single <see cref = "IPortfolio" /> from CSV data for an investment portfolio.
    /// </summary>
    public abstract class TransactionHistoryCsvParser : IDisposable
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

        /// <summary>
        /// Constructs a new TransactionHistoryCsvParser.
        /// </summary>
        /// <param name="csvStream">A <see cref="Stream"/> to the CSV data.</param>
        protected TransactionHistoryCsvParser(Stream csvStream)
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
        ~TransactionHistoryCsvParser()
        {
            Dispose(false);
        }

        #endregion

        #region Abstract Properties

        /// <summary>
        /// Represents the string qualifier used in the Date column header.
        /// </summary>
        protected abstract string DateHeader { get; }

        /// <summary>
        /// Represents the string qualifier used in the TransactionType column header.
        /// </summary>
        protected abstract string TransactionTypeHeader { get; }

        /// <summary>
        /// Represents the string qualifier used in the Symbol column header.
        /// </summary>
        protected abstract string SymbolHeader { get; }

        /// <summary>
        /// Represents the string qualifier used in the Shares column header.
        /// </summary>
        protected abstract string SharesHeader { get; }

        /// <summary>
        /// Represents the string qualifier used in the PricePerShare column header.
        /// </summary>
        protected abstract string PricePerShareHeader { get; }

        /// <summary>
        /// Represents the string qualifier used in the Commission column header.
        /// </summary>
        protected abstract string CommissionHeader { get; }

        #endregion

        #region Private Properties

        private DataColumn DateColumn
        {
            get
            {
                return _dateColumn;
            }
            set
            {
                if(_dateColumn != null)
                {
                    _dateColumn.Dispose();
                }
                _dateColumn = value;
            }
        }

        private DataColumn OrderColumn
        {
            get
            {
                return _orderColumn;
            }
            set
            {
                if(_orderColumn != null)
                {
                    _orderColumn.Dispose();
                }
                _orderColumn = value;
            }
        }

        private DataColumn SymbolColumn
        {
            get
            {
                return _symbolColumn;
            }
            set
            {
                if (_symbolColumn != null)
                {
                    _symbolColumn.Dispose();
                }
                _symbolColumn = value;
            }
        }

        private DataColumn SharesColumn
        {
            get
            {
                return _sharesColumn;
            }
            set
            {
                if (_sharesColumn != null)
                {
                    _sharesColumn.Dispose();
                }
                _sharesColumn = value;
            }
        }

        private DataColumn PriceColumn
        {
            get
            {
                return _priceColumn;
            }
            set
            {
                if (_priceColumn != null)
                {
                    _priceColumn.Dispose();
                }
                _priceColumn = value;
            }
        }

        private DataColumn CommissionColumn
        {
            get
            {
                return _commissionColumn;
            }
            set
            {
                if (_commissionColumn != null)
                {
                    _commissionColumn.Dispose();
                }
                _commissionColumn = value;
            }
        }

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
                    _map.Add(TransactionColumns.PricePerShare, i);
                    count += (int) TransactionColumns.PricePerShare;
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

        private IPortfolio ParseDataTableToIPortfolio(DataTable table)
        {
            IPortfolio portfolio = new Portfolio(0.00m);
            foreach(DataRow row in table.Rows)
            {
                ITransaction transaction = null;
                switch ((OrderType) row[OrderColumn])
                {
                    case OrderType.Deposit:
                        transaction = new Deposit((DateTime) row[DateColumn], (decimal) row[PriceColumn]);
                        break;
                    case OrderType.Withdrwawal:
                        transaction = new Withdrawal((DateTime)row[DateColumn], (decimal)row[PriceColumn]);
                        break;
                    default:
                        transaction = new Transaction((DateTime) row[DateColumn],
                                                      (OrderType) row[OrderColumn],
                                                      (string) row[SymbolColumn],
                                                      (decimal) row[PriceColumn],
                                                      (double) row[SharesColumn],
                                                      (decimal) row[CommissionColumn]);
                        break;
                }
                portfolio.AddTransaction(transaction);
            }
            return portfolio;
        }

        private DataTable InitializePortfolioTable()
        {
            DateColumn = new DataColumn("Date", typeof (DateTime));
            OrderColumn = new DataColumn("Order Type", typeof (OrderType));
            SymbolColumn = new DataColumn("Symbol", typeof (string));
            SharesColumn = new DataColumn("Shares", typeof (double));
            PriceColumn = new DataColumn("Price", typeof (decimal));
            CommissionColumn = new DataColumn("Commission", typeof (decimal));

            using (DataTable table = new DataTable {Locale = CultureInfo.InvariantCulture})
            {
                table.Columns.Add(DateColumn);
                table.Columns.Add(OrderColumn);
                table.Columns.Add(SymbolColumn);
                table.Columns.Add(SharesColumn);
                table.Columns.Add(PriceColumn);
                table.Columns.Add(CommissionColumn);

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

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting umanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether or not the object should be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DateColumn = null;
                OrderColumn = null;
                SymbolColumn = null;
                SharesColumn = null;
                PriceColumn = null;
                CommissionColumn = null;
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
                    OrderType orderType = ParseOrderTypeColumn(reader[_map[TransactionColumns.OrderType]]);
                    DataRow row = table.NewRow();
                    row.BeginEdit();
                    row[DateColumn] = ParseDateColumn(reader[_map[TransactionColumns.Date]]);
                    row[OrderColumn] = orderType;
                    switch(orderType)
                    {
                        case OrderType.Buy:
                        case OrderType.Sell:
                            row[SymbolColumn] = ParseSymbolColumn(reader[_map[TransactionColumns.Symbol]]);
                            row[SharesColumn] = ParseSharesColumn(reader[_map[TransactionColumns.Shares]]);
                            row[PriceColumn] = ParsePriceColumn(reader[_map[TransactionColumns.PricePerShare]]);
                            row[CommissionColumn] = ParseCommissionColumn(reader[_map[TransactionColumns.Commission]]);
                            break;
                    }
                    row.EndEdit();
                }

                return table;
            }
        }

        #endregion

        #region Abstract/Virtual Methods

        /// <summary>
        /// Parses data from the Date column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed <see cref="DateTime"/>.</returns>
        protected virtual DateTime ParseDateColumn(string text)
        {
            return DateTime.Parse(text.Trim());
        }

        /// <summary>
        /// Parses data from the OrderType column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed <see cref="OrderType"/>.</returns>
        protected abstract OrderType ParseOrderTypeColumn(string text);

        /// <summary>
        /// Parses data from the Symbol column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed ticker symbol.</returns>
        protected virtual string ParseSymbolColumn(string text)
        {
            return text.Trim().ToUpperInvariant();
        }

        /// <summary>
        /// Parses data from the Shares column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed number of shares.</returns>
        protected virtual double ParseSharesColumn(string text)
        {
            return text.Trim().Length != 0 ? Math.Abs(double.Parse(text)) : 0.0;
        }

        /// <summary>
        /// Parses data from the Price column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed per-share price.</returns>
        protected virtual decimal ParsePriceColumn(string text)
        {
            return text.Trim().Length != 0 ? Math.Abs(decimal.Parse(text)) : 0.0m;
        }

        /// <summary>
        /// Parses data from the Comission column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed comission price.</returns>
        protected virtual decimal ParseCommissionColumn(string text)
        {
            return text.Trim().Length != 0 ? Math.Abs(decimal.Parse(text)) : 0.0m;
        }

        #endregion
    }
}