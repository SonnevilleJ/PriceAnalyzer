using System;
using System.IO;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    ///   Parses an <see cref = "IPortfolio" /> from Fidelity CSV data.
    /// </summary>
    public class FidelityTransactionHistoryCsvParser : TransactionHistoryCsvParser
    {
        #region Constructors

        /// <summary>
        ///   Creates a
        /// </summary>
        /// <param name = "csvStream"></param>
        public FidelityTransactionHistoryCsvParser(Stream csvStream)
            : base(csvStream)
        {}

        #endregion

        #region Overrides of TransactionHistoryCsvParser

        /// <summary>
        /// Represents the string qualifier used in the Date column header.
        /// </summary>
        protected override string DateHeader { get { return "Trade Date"; } }

        /// <summary>
        /// Represents the string qualifier used in the TransactionType column header.
        /// </summary>
        protected override string TransactionTypeHeader { get { return "Action"; } }

        /// <summary>
        /// Represents the string qualifier used in the Symbol column header.
        /// </summary>
        protected override string SymbolHeader { get { return "Symbol"; } }

        /// <summary>
        /// Represents the string qualifier used in the Shares column header.
        /// </summary>
        protected override string SharesHeader { get { return "Quantity"; } }

        /// <summary>
        /// Represents the string qualifier used in the PricePerShare column header.
        /// </summary>
        protected override string PricePerShareHeader { get { return "Price ($)"; } }

        /// <summary>
        /// Represents the string qualifier used in the Commission column header.
        /// </summary>
        protected override string CommissionHeader { get { return "Commission ($)"; } }

        /// <summary>
        /// Parses data from the OrderType column of the CSV data.
        /// </summary>
        /// <param name="text">The raw CSV data to parse.</param>
        /// <returns>The parsed <see cref="OrderType"/>.</returns>
        protected override OrderType ParseOrderTypeColumn(string text)
        {
            OrderType type;
            switch (text)
            {
                case "Electronic Funds Transfer Received":
                    type = OrderType.Deposit;
                    break;
                case "YOU SOLD":
                    type = OrderType.Sell;
                    break;
                case "DIVIDEND RECEIVED":
                    type = OrderType.Dividend;
                    break;
                case "REINVESTMENT":
                case "YOU BOUGHT":
                    type = OrderType.Buy;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("text", text, String.Format("Unknown order type: {0}.", text));
            }
            return type;
        }

        #endregion
    }
}