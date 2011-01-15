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
    public abstract class TransactionHistoryCsvFile : IDisposable
    {
        #region Private Members

        private readonly IDictionary<TransactionColumn, int> _map = new Dictionary<TransactionColumn, int>(5);
        private Stream _stream;
        private DataTable _dataTable;
        private bool _tableParsed;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new TransactionHistoryCsvFile.
        /// </summary>
        /// <param name="csvStream">A <see cref="Stream"/> to the CSV data.</param>
        protected TransactionHistoryCsvFile(Stream csvStream)
            : this(csvStream, false)
        {
        }

        /// <summary>
        /// Constructs a new TransactionHistoryCsvFile.
        /// </summary>
        /// <param name="csvStream">A <see cref="Stream"/> to the CSV data.</param>
        /// <param name="useTotalBasis">A value indicating whether or not TotalBasis should be used to calculate price.</param>
        protected TransactionHistoryCsvFile(Stream csvStream, bool useTotalBasis)
        {
            if (csvStream == null)
            {
                throw new ArgumentNullException("csvStream");
            }
            _stream = csvStream;
            UseTotalBasis = useTotalBasis;

            InitializeDataTable();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a <see cref="DataTable"/> containing the data in this CSV file.
        /// </summary>
        public DataTable DataTable
        {
            get
            {
                return _dataTable;
            }
            private set
            {
                if (_dataTable != null)
                {
                    _dataTable.Dispose();
                }
                _dataTable = value;
            }
        }

        /// <summary>
        /// Gets the index of the Date column in DataTable.
        /// </summary>
        public int DateColumn { get; private set; }

        /// <summary>
        /// Gets the index of the Order column in DataTable.
        /// </summary>
        public int OrderColumn { get; private set; }

        /// <summary>
        /// Gets the index of the Symbol column in DataTable.
        /// </summary>
        public int SymbolColumn { get; private set; }

        /// <summary>
        /// Gets the index of the Shares column in DataTable.
        /// </summary>
        public int SharesColumn { get; private set; }

        /// <summary>
        /// Gets the index of the PRice column in DataTable.
        /// </summary>
        public int PriceColumn { get; private set; }

        /// <summary>
        /// Gets the index of the Commission column in DataTable.
        /// </summary>
        public int CommissionColumn { get; private set; }

        /// <summary>
        /// Gets the index of the Total Basis column in DataTable.
        /// </summary>
        public int TotalBasisColumn { get; private set; }

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether the TotalBasis column should be used to calculate price.
        /// </summary>
        /// <remarks>Use the TotalBasis column when raw price is not included in the CSV file.</remarks>
        private bool UseTotalBasis { get; set; }

        #region Private Methods

        private void MapHeaders(CsvReader reader)
        {
            string[] headers = reader.GetFieldHeaders();
            for (int i = 0; i < headers.Length; i++)
            {
                TransactionColumn column = ParseHeader(headers[i]);
                if (column != TransactionColumn.None)
                {
                    _map.Add(column, i);
                }
            }
        }

        private void InitializeDataTable()
        {
            // create columns
            DataColumn dateColumn = new DataColumn("Date", typeof (DateTime));
            DataColumn orderColumn = new DataColumn("Order Type", typeof(OrderType));
            DataColumn symbolColumn = new DataColumn("Symbol", typeof(string));
            DataColumn sharesColumn = new DataColumn("Shares", typeof(double));
            DataColumn priceColumn = new DataColumn("Price", typeof(decimal));
            DataColumn commissionColumn = new DataColumn("Commission", typeof(decimal)) { DefaultValue = 0.00m };

            // create table
            DataTable table = new DataTable {Locale = CultureInfo.InvariantCulture};

            // add columns to table
            table.Columns.Add(dateColumn);
            table.Columns.Add(orderColumn);
            table.Columns.Add(symbolColumn);
            table.Columns.Add(sharesColumn);
            table.Columns.Add(priceColumn);
            table.Columns.Add(commissionColumn);

            // fill column indexes
            DateColumn = table.Columns.IndexOf(dateColumn);
            OrderColumn = table.Columns.IndexOf(orderColumn);
            SymbolColumn = table.Columns.IndexOf(symbolColumn);
            SharesColumn = table.Columns.IndexOf(sharesColumn);
            PriceColumn = table.Columns.IndexOf(priceColumn);
            CommissionColumn = table.Columns.IndexOf(commissionColumn);

            DataTable = table;
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
                if(_stream != null)
                {
                    _stream.Dispose();
                    _stream = null;
                }
                if(_dataTable != null)
                {
                    _dataTable.Dispose();
                    _dataTable = null;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Parses a <see cref = "DataTable" /> from a given CSV data stream.
        /// </summary>
        /// <returns>A <see cref = "DataTable" /> of the CSV data.</returns>
        public void Parse()
        {
            if (!_tableParsed)
            {
                using (CsvReader reader = new CsvReader(new StreamReader(_stream), true))
                {
                    MapHeaders(reader);

                    InitializeDataTable();
                    while (reader.ReadNextRecord())
                    {
                        OrderType orderType = ParseOrderTypeColumn(reader[_map[TransactionColumn.OrderType]]);
                        DataRow row = DataTable.NewRow();
                        row.BeginEdit();
                        row[DateColumn] = ParseDateColumn(reader[_map[TransactionColumn.Date]]);
                        row[OrderColumn] = orderType;
                        row[SymbolColumn] = ParseSymbolColumn(reader[_map[TransactionColumn.Symbol]]);
                        row[SharesColumn] = ParseSharesColumn(reader[_map[TransactionColumn.Shares]]);
                        row[CommissionColumn] = ParseCommissionColumn(reader[_map[TransactionColumn.Commission]]);
                        switch (orderType)
                        {
                            case OrderType.Buy:
                            case OrderType.Sell:
                                if (UseTotalBasis)
                                {
                                    row[PriceColumn] = ParseTotalBasisColumn(reader[_map[TransactionColumn.TotalBasis]]) / decimal.Parse(row[SharesColumn].ToString());
                                }
                                else
                                {
                                    row[PriceColumn] = ParsePriceColumn(reader[_map[TransactionColumn.PricePerShare]]);
                                }
                                break;
                            case OrderType.Deposit:
                            case OrderType.Withdrawal:
                                if (UseTotalBasis)
                                {
                                    row[PriceColumn] = ParseTotalBasisColumn(reader[_map[TransactionColumn.TotalBasis]]);
                                }
                                else
                                {
                                    row[PriceColumn] = ParsePriceColumn(reader[_map[TransactionColumn.PricePerShare]]);
                                }
                                break;
                        }
                        row.EndEdit();
                        DataTable.Rows.Add(row);
                    }
                    _tableParsed = true;
                }
            }
        }

        #endregion

        #region Abstract/Virtual Methods

        /// <summary>
        /// Parses the headers of a TransactionHistoryCsv file.
        /// </summary>
        /// <param name="header">A header from the CSV file</param>
        /// <returns>The <see cref="TransactionColumn"/> of <paramref name="header"/>.</returns>
        protected abstract TransactionColumn ParseHeader(string header);

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
            return text.Trim().Length != 0 ? Math.Abs(double.Parse(text.Trim())) : 0.0;
        }

        /// <summary>
        /// Parses data from the Price column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed per-share price.</returns>
        protected virtual decimal ParsePriceColumn(string text)
        {
            return text.Trim().Length != 0 ? Math.Abs(decimal.Parse(text.Trim())) : 0.0m;
        }

        /// <summary>
        /// Parses data from the Comission column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed comission price.</returns>
        protected virtual decimal ParseCommissionColumn(string text)
        {
            return text.Trim().Length != 0 ? Math.Abs(decimal.Parse(text.Trim())) : 0.0m;
        }

        /// <summary>
        /// Parses data from the TotalBasis column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed TotalBasis amount.</returns>
        protected virtual decimal ParseTotalBasisColumn(string text)
        {
            return text.Trim().Length != 0 ? Math.Abs(decimal.Parse(text.Trim())) : 0.0m;
        }

        #endregion
    }
}