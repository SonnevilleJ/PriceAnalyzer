using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.Fidelity
{
    public class FidelityBrokerageLinkTransactionHistoryCsvFile : ISecurityBasket
    {
        private readonly bool _useTotalBasis;
        private bool _tableParsed;
        private readonly ITransactionFactory _transactionFactory;
        private readonly IHoldingFactory _holdingFactory;
        private readonly ICsvReader _csvReader;

        public FidelityBrokerageLinkTransactionHistoryCsvFile(Stream csvStream, bool useTotalBasis = true)
            : this(new CsvReaderWrapper(csvStream), useTotalBasis)
        {
        }

        public FidelityBrokerageLinkTransactionHistoryCsvFile(ICsvReader csvStream, bool useTotalBasis = true)
        {
            Transactions = new List<ITransaction>();
            _transactionFactory = new TransactionFactory();
            _holdingFactory = new HoldingFactory();
            _csvReader = csvStream;
            if (_csvReader == null)
            {
                throw new ArgumentNullException("csvStream");
            }
            _useTotalBasis = useTotalBasis;

            Parse();
        }

        public IEnumerable<ITransaction> Transactions { get; private set; }

        private void Parse()
        {
            if (_tableParsed) return;

            var map = MapHeaders(_csvReader);

            Transactions = ReadAllTransactions(_csvReader, map);

            _tableParsed = true;
        }

        private IEnumerable<ITransaction> ReadAllTransactions(ICsvReader reader, IDictionary<TransactionColumn, int> map)
        {
            while (reader.ReadNextRecord())
            {
                var ticker = string.Empty;
                decimal pricePerShare;

                if (!IsValidRow(reader[map[TransactionColumn.OrderType]])) continue;

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
                        pricePerShare = _useTotalBasis
                            ? (ParsePriceColumn(reader[map[TransactionColumn.TotalBasis]]) - commission)/shares
                            : ParsePriceColumn(reader[map[TransactionColumn.PricePerShare]]);
                        break;
                    case OrderType.Sell:
                        pricePerShare = _useTotalBasis
                            ? (ParsePriceColumn(reader[map[TransactionColumn.TotalBasis]]) + commission)/shares
                            : ParsePriceColumn(reader[map[TransactionColumn.PricePerShare]]);
                        break;
                    case OrderType.DividendReinvestment:
                        pricePerShare = _useTotalBasis
                            ? ParsePriceColumn(reader[map[TransactionColumn.TotalBasis]])/shares
                            : ParsePriceColumn(reader[map[TransactionColumn.PricePerShare]]);
                        break;
                    case OrderType.Deposit:
                    case OrderType.Withdrawal:
                    case OrderType.DividendReceipt:
                        pricePerShare = _useTotalBasis
                            ? ParsePriceColumn(reader[map[TransactionColumn.TotalBasis]])
                            : ParsePriceColumn(reader[map[TransactionColumn.PricePerShare]]);
                        break;
                    default:
                        throw new NotSupportedException();
                }
                var transaction = _transactionFactory.ConstructTransaction(
                    orderType,
                    settlementDate,
                    ticker,
                    pricePerShare,
                    shares,
                    commission);
                yield return transaction;
            }
            _csvReader.Dispose();
        }

        private IDictionary<TransactionColumn, int> MapHeaders(ICsvReader reader)
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

        private TransactionColumn ParseColumnHeader(string header)
        {
            if(String.IsNullOrWhiteSpace(header))
            {
                return TransactionColumn.None;
            }

            switch (header.ToUpperInvariant())
            {
                case "TRADE DATE":
                    return TransactionColumn.Date;
                case "ACTION DESCRIPTION":
                    return TransactionColumn.OrderType;
                case "SYMBOL":
                    return TransactionColumn.Symbol;
                case "QUANTITY":
                    return TransactionColumn.Shares;
                case "PRICE ($)":
                    return TransactionColumn.PricePerShare;
                case "AMOUNT ($)":
                    return TransactionColumn.TotalBasis;
                case "COMMISSION ($)":
                    return TransactionColumn.Commission;
                default:
                    return TransactionColumn.None;
            }
        }

        private DateTime ParseDateColumn(string text)
        {
            var result = text.Trim();
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException("text", Strings.TransactionHistoryCsvFile_ParseDateColumn_Parsed_date_was_returned_as_null_or_whitespace_);
            }
            return DateTime.Parse(result, CultureInfo.InvariantCulture);
        }

        private string ParseSymbolColumn(string text)
        {
            var result = text.Trim();
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException("text", Strings.TransactionHistoryCsvFile_ParseSymbolColumn_Parsed_ticker_symbol_was_returned_as_null_or_whitespace_);
            }
            return result.ToUpperInvariant();
        }

        private decimal ParseSharesColumn(string text)
        {
            var result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? 0m
                       : Math.Abs(decimal.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        private decimal ParsePriceColumn(string text)
        {
            var result = text.Trim();
            return string.IsNullOrWhiteSpace(result)
                       ? 0.00m
                       : Math.Abs(decimal.Parse(text.Trim(), CultureInfo.InvariantCulture));
        }

        private OrderType ParseOrderTypeColumn(string text)
        {
            var trim = text.Trim();
            if(String.IsNullOrWhiteSpace(trim))
            {
                throw new ArgumentNullException("text");
            }

            var upperInvariant = trim.ToUpperInvariant();
            if ((upperInvariant.StartsWith("TRANSFERRED FROM") && upperInvariant.EndsWith("TO BROKERAGE OPTION")) ||
                upperInvariant == "PURCHASE INTO CORE ACCOUNT")
            {
                return OrderType.Deposit;
            }
            if (upperInvariant.StartsWith("YOU BOUGHT"))
            {
                return OrderType.Buy;
            }
            if (upperInvariant.StartsWith("YOU SOLD"))
            {
                return OrderType.Sell;
            }
            if (upperInvariant.StartsWith("DIVIDEND RECEIVED") ||
                upperInvariant.StartsWith("SHORT-TERM CAP GAIN") ||
                upperInvariant.StartsWith("LONG-TERM CAP GAIN"))
            {
                return OrderType.DividendReceipt;
            }
            if (upperInvariant.StartsWith("REINVESTMENT"))
            {
                return OrderType.DividendReinvestment;
            }
            throw new ArgumentOutOfRangeException("text", trim, String.Format(CultureInfo.CurrentCulture, "Unknown order type: {0}.", trim));
        }

        private bool IsValidRow(string text)
        {
            if (text.Trim() == "REDEMPTION FROM CORE ACCOUNT")
            {
                return false;
            }
            return true;
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
                        select profit * shares).Sum();
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