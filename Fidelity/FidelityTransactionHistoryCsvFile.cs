using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Fidelity
{
    /// <summary>
    ///   Parses an <see cref = "Portfolio" /> from Fidelity CSV data.
    /// </summary>
    public class FidelityTransactionHistoryCsvFile : TransactionHistoryCsvFile
    {
        /// <summary>
        ///   Constructs a TransactionHistoryCsvFile from Fidelity transaction data.
        /// </summary>
        /// <param name = "csvStream"></param>
        public FidelityTransactionHistoryCsvFile(Stream csvStream)
            : base(csvStream, true)
        {
        }

        /// <summary>
        /// Parses the headers of a TransactionHistoryCsv file.
        /// </summary>
        /// <param name="header">A header from the CSV file</param>
        /// <returns>The <see cref="TransactionColumn"/> of <paramref name="header"/>.</returns>
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

        /// <summary>
        /// Parses data from the OrderType column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed <see cref="OrderType"/>.</returns>
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