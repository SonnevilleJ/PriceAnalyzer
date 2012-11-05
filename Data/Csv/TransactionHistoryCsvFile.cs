using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;

namespace Sonneville.PriceTools.Data.Csv
{
    /// <summary>
    ///   Parses a single <see cref = "SecurityBasket" /> from CSV data for an investment portfolio.
    /// </summary>
    public abstract class TransactionHistoryCsvFile : SecurityBasket
    {
        #region Private Members

        private readonly IDictionary<TransactionColumn, int> _map = new Dictionary<TransactionColumn, int>(5);
        private readonly bool _useTotalBasis;
        private bool _tableParsed;
        private readonly List<Transaction> _transactions = new List<Transaction>();

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
            _useTotalBasis = useTotalBasis;

            Parse(csvStream);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a list of all <see cref="Transaction"/>s in the file.
        /// </summary>
        public IEnumerable<Transaction> Transactions
        {
            get { return _transactions.OrderBy(t => t.SettlementDate).ThenBy(GetSortIndex); }
        }

        private static int GetSortIndex(Transaction transaction)
        {
            // must sort transactions in order
            // First, any transactions which yield proceeds
            // Second, any transactions which use funds
            // This appropriately ensures funds are available for use

            if (transaction is Deposit) return 0;
            if (transaction is DividendReceipt) return 1;
            if (transaction is DividendReinvestment) return 2;
            if (transaction is Sell) return 3;
            if (transaction is BuyToCover) return 4;
            if (transaction is Buy) return 5;
            if (transaction is SellShort) return 6;
            if (transaction is Withdrawal) return 7;
            return int.MaxValue;
        }

        #endregion

        #region Private Methods

        private void MapHeaders(CsvReader reader)
        {
            var headers = reader.GetFieldHeaders();
            for (var i = 0; i < headers.Length; i++)
            {
                var column = ParseColumnHeader(headers[i]);
                if (column != TransactionColumn.None)
                {
                    _map.Add(column, i);
                }
            }
        }

        /// <summary>
        ///   Parses a <see cref = "DataTable" /> from a given CSV data stream.
        /// </summary>
        private void Parse(Stream stream)
        {
            if (_tableParsed) return;
            using (var reader = new CsvReader(new StreamReader(stream), true))
            {
                MapHeaders(reader);

                while (reader.ReadNextRecord())
                {
                    var ticker = string.Empty;
                    decimal price;

                    if(!IsValidRow(reader[_map[TransactionColumn.OrderType]])) continue;

                    var orderType = ParseOrderTypeColumn(reader[_map[TransactionColumn.OrderType]]);
                    var settlementDate = ParseDateColumn(reader[_map[TransactionColumn.Date]]);
                    var shares = ParseSharesColumn(reader[_map[TransactionColumn.Shares]]);
                    var commission = ParsePriceColumn(reader[_map[TransactionColumn.Commission]]);

                    if (orderType != OrderType.Deposit &&
                        orderType != OrderType.Withdrawal &&
                        orderType != OrderType.DividendReceipt)
                    {
                        // Portfolio currently can't support ticker symbols for cash transactions, so ignore
                        ticker = ParseSymbolColumn(reader[_map[TransactionColumn.Symbol]]);
                    }
                    switch (orderType)
                    {
                        case OrderType.Buy:
                            price = _useTotalBasis
                                        ? (ParsePriceColumn(reader[_map[TransactionColumn.TotalBasis]]) - commission) / shares
                                        : ParsePriceColumn(reader[_map[TransactionColumn.PricePerShare]]);
                            break;
                        case OrderType.Sell:
                            price = _useTotalBasis
                                        ? (ParsePriceColumn(reader[_map[TransactionColumn.TotalBasis]]) + commission) / shares
                                        : ParsePriceColumn(reader[_map[TransactionColumn.PricePerShare]]);
                            break;
                        case OrderType.DividendReceipt:
                            price = _useTotalBasis
                                        ? ParsePriceColumn(reader[_map[TransactionColumn.TotalBasis]])
                                        : ParsePriceColumn(reader[_map[TransactionColumn.PricePerShare]]);
                            break;
                        case OrderType.DividendReinvestment:
                            price = _useTotalBasis
                                        ? ParsePriceColumn(reader[_map[TransactionColumn.TotalBasis]]) / shares
                                        : ParsePriceColumn(reader[_map[TransactionColumn.PricePerShare]]);
                            break;
                        case OrderType.Deposit:
                        case OrderType.Withdrawal:
                            price = _useTotalBasis
                                        ? ParsePriceColumn(reader[_map[TransactionColumn.TotalBasis]])
                                        : ParsePriceColumn(reader[_map[TransactionColumn.PricePerShare]]);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    _transactions.Add(TransactionFactory.ConstructTransaction(
                        orderType,
                        settlementDate,
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
        /// Parses the OrderType column and returns a value indicating if the row contains valid data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns></returns>
        protected virtual bool IsValidRow(string text)
        {
            return true;
        }

        /// <summary>
        /// Parses the column headers of a TransactionHistoryCsv file.
        /// </summary>
        /// <param name="header">A column header from the CSV file.</param>
        /// <returns>The <see cref="TransactionColumn"/> of <paramref name="header"/>.</returns>
        protected abstract TransactionColumn ParseColumnHeader(string header);

        /// <summary>
        /// Parses data from the Date column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed <see cref="DateTime"/>.</returns>
        protected virtual DateTime ParseDateColumn(string text)
        {
            var result = text.Trim();
            if(string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException("text", Strings.TransactionHistoryCsvFile_ParseDateColumn_Parsed_date_was_returned_as_null_or_whitespace_);
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
            var result = text.Trim();
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException("text", Strings.TransactionHistoryCsvFile_ParseSymbolColumn_Parsed_ticker_symbol_was_returned_as_null_or_whitespace_);
            }
            return result.ToUpperInvariant();
        }

        /// <summary>
        /// Parses data from the Shares column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed number of shares.</returns>
        protected virtual decimal ParseSharesColumn(string text)
        {
            var result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? 0m
                       : Math.Abs(decimal.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Parses data from one of the price columns of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed per-share price.</returns>
        protected virtual decimal ParsePriceColumn(string text)
        {
            var result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? 0.00m
                       : Math.Abs(decimal.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        #endregion

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimePeriod.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimePeriod as of the given DateTime.</returns>
        public decimal this[DateTime dateTime]
        {
            get
            {
                var allHoldings = this.CalculateHoldings(dateTime);
                if (allHoldings.Count == 0) return 0;

                var positionGroups = allHoldings.GroupBy(h => h.Ticker);

                return (from holdings in positionGroups
                        from holding in holdings
                        let open = holding.OpenPrice
                        let close = holding.ClosePrice
                        let profit = close - open
                        let shares = holding.Shares
                        select profit*shares).Sum();
            }
        }

        /// <summary>
        /// Gets the first DateTime for which a value exists.
        /// </summary>
        public DateTime Head
        {
            get
            {
                if (Transactions.Any())
                {
                    return Transactions.Min(t => t.SettlementDate);
                }
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the last DateTime for which a value exists.
        /// </summary>
        public DateTime Tail
        {
            get
            {
                if (Transactions.Any())
                {
                    return Transactions.Max(t => t.SettlementDate);
                }
                return DateTime.Now;
            }
        }
    }
}