﻿using System;
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
    public abstract class TransactionHistoryCsvFile
    {
        #region Private Members

        private readonly IDictionary<TransactionColumn, int> _map = new Dictionary<TransactionColumn, int>(5);
        private readonly Stream _stream;
        private readonly bool _useTotalBasis;
        private bool _tableParsed;
        private readonly List<ITransaction> _transactions = new List<ITransaction>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new TransactionHistoryCsvFile.
        /// </summary>
        /// <param name="csvStream">A <see cref="Stream"/> to the CSV data.</param>
        /// <param name="useTotalBasis">A value indicating whether or not TotalBasis should be used to calculate price.</param>
        protected TransactionHistoryCsvFile(Stream csvStream, bool useTotalBasis = false)
        {
            if (csvStream == null)
            {
                throw new ArgumentNullException("csvStream");
            }
            _stream = csvStream;
            _useTotalBasis = useTotalBasis;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a list of all ITransactions in the file.
        /// </summary>
        public IEnumerable<ITransaction> Transactions
        {
            get
            {
                if (!_tableParsed)
                {
                    Parse();
                }
                return _transactions;
            }
        }

        #endregion

        #region Private Methods

        private void MapHeaders(CsvReader reader)
        {
            string[] headers = reader.GetFieldHeaders();
            for (int i = 0; i < headers.Length; i++)
            {
                TransactionColumn column = ParseColumnHeader(headers[i]);
                if (column != TransactionColumn.None)
                {
                    _map.Add(column, i);
                }
            }
        }

        /// <summary>
        ///   Parses a <see cref = "DataTable" /> from a given CSV data stream.
        /// </summary>
        /// <returns>A <see cref = "DataTable" /> of the CSV data.</returns>
        private void Parse()
        {
            if (_tableParsed) return;
            using (CsvReader reader = new CsvReader(new StreamReader(_stream), true))
            {
                MapHeaders(reader);

                while (reader.ReadNextRecord())
                {
                    string ticker = string.Empty;
                    decimal price;

                    OrderType orderType = ParseOrderTypeColumn(reader[_map[TransactionColumn.OrderType]]);
                    DateTime settlementDate = ParseDateColumn(reader[_map[TransactionColumn.Date]]);
                    double shares = ParseSharesColumn(reader[_map[TransactionColumn.Shares]]);
                    decimal commission = ParseCommissionColumn(reader[_map[TransactionColumn.Commission]]);

                    if (orderType != OrderType.DividendReceipt)
                    {
                        // Portfolio currently can't support ticker symbols for dividend received, so ignore
                        ticker = ParseSymbolColumn(reader[_map[TransactionColumn.Symbol]]);
                    }
                    switch (orderType)
                    {
                        case OrderType.Buy:
                            price = _useTotalBasis
                                        ? (ParseTotalBasisColumn(reader[_map[TransactionColumn.TotalBasis]]) - commission)/(decimal) (shares)
                                        : ParsePriceColumn(reader[_map[TransactionColumn.PricePerShare]]);
                            break;
                        case OrderType.Sell:
                            price = _useTotalBasis
                                        ? (ParseTotalBasisColumn(reader[_map[TransactionColumn.TotalBasis]]) + commission)/(decimal) (shares)
                                        : ParsePriceColumn(reader[_map[TransactionColumn.PricePerShare]]);
                            break;
                        case OrderType.DividendReceipt:
                        case OrderType.DividendReinvestment:
                        case OrderType.Deposit:
                        case OrderType.Withdrawal:
                            price = _useTotalBasis
                                ? ParseTotalBasisColumn(reader[_map[TransactionColumn.TotalBasis]])
                                : ParsePriceColumn(reader[_map[TransactionColumn.PricePerShare]]);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    _transactions.Add(TransactionFactory.CreateTransaction(
                        settlementDate,
                        orderType,
                        ticker,
                        price,
                        shares,
                        commission));
                }
                _transactions.Sort((left, right) => DateTime.Compare(left.SettlementDate, right.SettlementDate));

                _tableParsed = true;
            }
        }

        #endregion

        #region Abstract/Virtual Methods

        /// <summary>
        /// Parses the column headers of a TransactionHistoryCsv file.
        /// </summary>
        /// <param name="header">A header from the CSV file</param>
        /// <returns>The <see cref="TransactionColumn"/> of <paramref name="header"/>.</returns>
        protected abstract TransactionColumn ParseColumnHeader(string header);

        /// <summary>
        /// Parses data from the Date column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed <see cref="DateTime"/>.</returns>
        protected virtual DateTime ParseDateColumn(string text)
        {
            string result = text.Trim();
            if(string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException("text", "Parsed date was returned as null or whitespace.");
            }
            return DateTime.Parse(result, CultureInfo.InvariantCulture);
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
            string result = text.Trim();
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException("text", "Parsed ticker symbol was returned as null or whitespace.");
            }
            return result.ToUpperInvariant();
        }

        /// <summary>
        /// Parses data from the Shares column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed number of shares.</returns>
        protected virtual double ParseSharesColumn(string text)
        {
            string result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? 0.0
                       : Math.Abs(double.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Parses data from the Price column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed per-share price.</returns>
        protected virtual decimal ParsePriceColumn(string text)
        {
            string result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? 0.00m
                       : Math.Abs(decimal.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Parses data from the Comission column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed comission price.</returns>
        protected virtual decimal ParseCommissionColumn(string text)
        {
            string result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? 0.00m
                       : Math.Abs(decimal.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Parses data from the TotalBasis column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed TotalBasis amount.</returns>
        protected virtual decimal ParseTotalBasisColumn(string text)
        {
            string result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? 0.00m
                       : Math.Abs(decimal.Parse(result, CultureInfo.InvariantCulture));
        }

        #endregion
    }
}