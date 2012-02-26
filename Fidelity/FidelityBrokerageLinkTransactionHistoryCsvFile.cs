using System;
using System.Globalization;
using System.IO;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Fidelity
{
    /// <summary>
    ///   Parses an <see cref = "Portfolio" /> from Fidelity CSV data.
    /// </summary>
    public class FidelityBrokerageLinkTransactionHistoryCsvFile : TransactionHistoryCsvFile
    {
        #region Constructors

        /// <summary>
        ///   Constructs a TransactionHistoryCsvFile from Fidelity transaction data.
        /// </summary>
        /// <param name = "csvStream"></param>
        public FidelityBrokerageLinkTransactionHistoryCsvFile(Stream csvStream)
            : base(csvStream, true)
        {
        }

        #endregion

        #region Overrides of TransactionHistoryCsvFile

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

        /// <summary>
        /// Parses data from the OrderType column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed <see cref="OrderType"/>.</returns>
        protected override OrderType ParseOrderTypeColumn(string text)
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

        protected override bool IsValidRow(string text)
        {
            if (text.Trim() == "REDEMPTION FROM CORE ACCOUNT")
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}