using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// A prior ownership of a financial security.
    /// </summary>
    public struct Holding
    {
        /// <summary>
        /// The ticker symbol held.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Gets the beginning of the holding period.
        /// </summary>
        public DateTime Head { get; set; }

        /// <summary>
        /// Gets the end of the holding period.
        /// </summary>
        public DateTime Tail { get; set; }

        /// <summary>
        /// The number of shares held.
        /// </summary>
        public decimal Shares { get; set; }

        /// <summary>
        /// The per-share price of the opening transaction.
        /// </summary>
        public decimal OpenPrice { get; set; }

        /// <summary>
        /// The commission paid for the opening transaction.
        /// </summary>
        public decimal OpenCommission { get; set; }

        /// <summary>
        /// The per-share price of the closing transaction.
        /// </summary>
        public decimal ClosePrice { get; set; }

        /// <summary>
        /// The commission paid for the closing transaction.
        /// </summary>
        public decimal CloseCommission { get; set; }
    }
}