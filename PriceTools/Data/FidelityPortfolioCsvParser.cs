using System;
using System.IO;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    ///   Parses an <see cref = "IPortfolio" /> from Fidelity CSV data.
    /// </summary>
    public class FidelityPortfolioCsvParser : PortfolioCsvParser
    {
        #region Constructors

        /// <summary>
        ///   Creates a
        /// </summary>
        /// <param name = "csvStream"></param>
        public FidelityPortfolioCsvParser(Stream csvStream)
            : base(csvStream)
        {}

        #endregion

        #region Overrides of PortfolioCsvParser

        protected override string DateHeader { get { return "Trade Date"; } }

        protected override string TransactionTypeHeader { get { return "Action"; } }

        protected override string SymbolHeader { get { return "Symbol"; } }

        protected override string SharesHeader { get { return "Quantity"; } }

        protected override string PricePerShareHeader { get { return "Price ($)"; } }

        protected override string CommissionHeader { get { return "Commission ($)"; } }

        #endregion
    }
}