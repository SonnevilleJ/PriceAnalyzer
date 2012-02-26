using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A prior ownership of a financial security.
    /// </summary>
    public interface IHolding
    {
        /// <summary>
        /// The ticker symbol held.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        /// Gets the beginning of the holding period.
        /// </summary>
        DateTime Head { get; }

        /// <summary>
        /// Gets the end of the holding period.
        /// </summary>
        DateTime Tail { get; }

        /// <summary>
        /// The number of shares held.
        /// </summary>
        decimal Shares { get; }

        /// <summary>
        /// The per-share price of the opening transaction.
        /// </summary>
        decimal OpenPrice { get; }

        /// <summary>
        /// The commission paid for the opening transaction.
        /// </summary>
        decimal OpenCommission { get; set; }

        /// <summary>
        /// The per-share price of the closing transaction.
        /// </summary>
        decimal ClosePrice { get; }

        /// <summary>
        /// The commission paid for the closing transaction.
        /// </summary>
        decimal CloseCommission { get; set; }
    }
}
