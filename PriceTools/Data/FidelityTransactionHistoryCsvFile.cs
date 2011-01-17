using System;
using System.IO;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    ///   Parses an <see cref = "IPortfolio" /> from Fidelity CSV data.
    /// </summary>
    public class FidelityTransactionHistoryCsvFile : TransactionHistoryCsvFile
    {
        #region Constructors

        /// <summary>
        ///   Constructs a TransactionHistoryCsvFile from Fidelity transaction data.
        /// </summary>
        /// <param name = "csvStream"></param>
        public FidelityTransactionHistoryCsvFile(Stream csvStream)
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
        protected override TransactionColumn ParseHeader(string header)
        {
            switch (header.ToUpperInvariant())
            {
                case "TRADE DATE":
                    return TransactionColumn.Date;
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
            switch (text.ToUpperInvariant())
            {
                case "ELECTRONIC FUNDS TRANSFER RECEIVED":
                    return OrderType.Deposit;
                case "YOU BOUGHT":
                    return OrderType.Buy;
                case "YOU SOLD":
                    return OrderType.Sell;
                case "DIVIDEND RECEIVED":
                    return OrderType.DividendReceipt;
                case "REINVESTMENT":
                    return OrderType.DividendReinvestment;
                default:
                    throw new ArgumentOutOfRangeException("text", text, String.Format("Unknown order type: {0}.", text));
            }
        }

        #endregion
    }
}