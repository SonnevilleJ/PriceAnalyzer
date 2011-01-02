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

        protected override string DateHeader { get { return "Trade Date"; } }

        protected override string TransactionTypeHeader { get { return "Action"; } }

        protected override string SymbolHeader { get { return "Symbol"; } }

        protected override string SharesHeader { get { return "Quantity"; } }

        protected override string PricePerShareHeader { get { return "Price ($)"; } }

        protected override string CommissionHeader { get { return "Commission ($)"; } }

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