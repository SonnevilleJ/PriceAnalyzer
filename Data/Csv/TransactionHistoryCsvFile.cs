using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.Data.Csv
{
    public abstract class TransactionHistoryCsvFile : ISecurityBasket
    {
        private readonly bool _useTotalBasis;
        private bool _tableParsed;
        private List<ITransaction> _transactions = new List<ITransaction>();
        private readonly ITransactionFactory _transactionFactory;
        private readonly IHoldingFactory _holdingFactory;

        protected TransactionHistoryCsvFile(Stream csvStream, bool useTotalBasis = false)
        {
            _transactionFactory = new TransactionFactory();
            if (csvStream == null)
            {
                throw new ArgumentNullException("csvStream");
            }
            _useTotalBasis = useTotalBasis;

            Parse(csvStream);
            _holdingFactory = new HoldingFactory();
        }

        public IEnumerable<ITransaction> Transactions
        {
            get
            {
                return _transactions;
            }
        }

        private IDictionary<TransactionColumn, int> MapHeaders(CsvReader reader)
        {
            var map = new Dictionary<TransactionColumn, int>(5);
            var headers = reader.GetFieldHeaders();
            for (var i = 0; i < headers.Length; i++)
            {
                var column = ParseColumnHeader(headers[i]);
                if (column != TransactionColumn.None)
                {
                    map.Add(column, i);
                }
            }
            return map;
        }

        private void Parse(Stream stream)
        {
            if (_tableParsed) return;
            using (var reader = new CsvReader(new StreamReader(stream), true))
            {
                var map = MapHeaders(reader);

                var transactions = _transactions;
                while (reader.ReadNextRecord())
                {
                    var ticker = string.Empty;
                    decimal price;

                    if(!IsValidRow(reader[map[TransactionColumn.OrderType]])) continue;

                    var orderType = ParseOrderTypeColumn(reader[map[TransactionColumn.OrderType]]);
                    var settlementDate = ParseDateColumn(reader[map[TransactionColumn.Date]]);
                    var shares = ParseSharesColumn(reader[map[TransactionColumn.Shares]]);
                    var commission = ParsePriceColumn(reader[map[TransactionColumn.Commission]]);

                    if (orderType != OrderType.Deposit &&
                        orderType != OrderType.Withdrawal &&
                        orderType != OrderType.DividendReceipt)
                    {
                        // Portfolio currently can't support ticker symbols for cash transactions, so ignore
                        ticker = ParseSymbolColumn(reader[map[TransactionColumn.Symbol]]);
                    }
                    switch (orderType)
                    {
                        case OrderType.Buy:
                            price = _useTotalBasis
                                        ? (ParsePriceColumn(reader[map[TransactionColumn.TotalBasis]]) - commission) / shares
                                        : ParsePriceColumn(reader[map[TransactionColumn.PricePerShare]]);
                            break;
                        case OrderType.Sell:
                            price = _useTotalBasis
                                        ? (ParsePriceColumn(reader[map[TransactionColumn.TotalBasis]]) + commission) / shares
                                        : ParsePriceColumn(reader[map[TransactionColumn.PricePerShare]]);
                            break;
                        case OrderType.DividendReceipt:
                            price = _useTotalBasis
                                        ? ParsePriceColumn(reader[map[TransactionColumn.TotalBasis]])
                                        : ParsePriceColumn(reader[map[TransactionColumn.PricePerShare]]);
                            break;
                        case OrderType.DividendReinvestment:
                            price = _useTotalBasis
                                        ? ParsePriceColumn(reader[map[TransactionColumn.TotalBasis]]) / shares
                                        : ParsePriceColumn(reader[map[TransactionColumn.PricePerShare]]);
                            break;
                        case OrderType.Deposit:
                        case OrderType.Withdrawal:
                            price = _useTotalBasis
                                        ? ParsePriceColumn(reader[map[TransactionColumn.TotalBasis]])
                                        : ParsePriceColumn(reader[map[TransactionColumn.PricePerShare]]);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    transactions.Add(_transactionFactory.ConstructTransaction(
                        orderType,
                        settlementDate,
                        ticker,
                        price,
                        shares,
                        commission));
                }
                _transactions = transactions.ToList();

                _tableParsed = true;
            }
        }

        protected virtual bool IsValidRow(string text)
        {
            return true;
        }

        protected abstract TransactionColumn ParseColumnHeader(string header);

        protected virtual DateTime ParseDateColumn(string text)
        {
            var result = text.Trim();
            if(string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException("text", Strings.TransactionHistoryCsvFile_ParseDateColumn_Parsed_date_was_returned_as_null_or_whitespace_);
            }
            return DateTime.Parse(result, CultureInfo.InvariantCulture);
        }

        protected abstract OrderType ParseOrderTypeColumn(string text);

        protected virtual string ParseSymbolColumn(string text)
        {
            var result = text.Trim();
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException("text", Strings.TransactionHistoryCsvFile_ParseSymbolColumn_Parsed_ticker_symbol_was_returned_as_null_or_whitespace_);
            }
            return result.ToUpperInvariant();
        }

        protected virtual decimal ParseSharesColumn(string text)
        {
            var result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? 0m
                       : Math.Abs(decimal.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        protected virtual decimal ParsePriceColumn(string text)
        {
            var result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? 0.00m
                       : Math.Abs(decimal.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        public decimal this[DateTime dateTime]
        {
            get
            {
                var allHoldings = _holdingFactory.CalculateHoldings(this, dateTime);
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