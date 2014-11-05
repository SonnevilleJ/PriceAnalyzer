using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Fidelity
{
    public class FidelityTransactionHistoryCsvFile : TransactionHistoryCsvFile
    {
        public FidelityTransactionHistoryCsvFile(Stream csvStream)
            : base(csvStream, true)
        {
        }

        protected override TransactionColumn ParseColumnHeader(string header)
        {
            if(String.IsNullOrWhiteSpace(header))
            {
                return TransactionColumn.None;
            }

            switch (header.ToUpperInvariant())
            {
                case "TRADE DATE":
                    return TransactionColumn.Date;
                case "DESCRIPTION":
                case "ACTION":
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

        protected override OrderType ParseOrderTypeColumn(string text)
        {
            if(String.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException("text");
            }

            var s = text.Split(' ').Where(str => !String.IsNullOrWhiteSpace(str)).Aggregate(String.Empty, (current, str) => current + (" " + str)).Trim();
            var upperInvariant = s.ToUpperInvariant();
            if (upperInvariant.StartsWith("ELECTRONIC FUNDS TRANSFER RECEIVED") ||
                upperInvariant.StartsWith("DIRECT DEPOSIT") ||
                upperInvariant == "PURCHASE INTO CORE ACCOUNT")
            {
                return OrderType.Deposit;
            }
            if (upperInvariant.StartsWith("ELECTRONIC FUNDS TRANSFER PAID") ||
                upperInvariant.StartsWith("FEE CHARGED"))
            {
                return OrderType.Withdrawal;
            }
            if (upperInvariant.StartsWith("YOU BOUGHT"))
            {
                return OrderType.Buy;
            }
            if (upperInvariant.StartsWith("YOU SOLD"))
            {
                return OrderType.Sell;
            }
            if (upperInvariant == "DIVIDEND RECEIVED" ||
                upperInvariant.StartsWith("SHORT-TERM CAP GAIN") ||
                upperInvariant.StartsWith("LONG-TERM CAP GAIN"))
            {
                return OrderType.DividendReceipt;
            }
            if (upperInvariant == "REINVESTMENT")
            {
                return OrderType.DividendReinvestment;
            }
            throw new ArgumentOutOfRangeException("text", s, String.Format(CultureInfo.CurrentCulture, "Unknown order type: {0}.", s));
        }

        protected override bool IsValidRow(string text)
        {
            var trim = text.Trim().ToUpperInvariant();
            if (trim == "REDEMPTION FROM CORE ACCOUNT" ||
                trim.StartsWith("NAME CHANGED"))
            {
                return false;
            }
            return true;
        }
    }
}