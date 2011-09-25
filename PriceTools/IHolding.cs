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
        double Shares { get; }

        /// <summary>
        /// The value of the holding at the <see cref="Head"/>.
        /// </summary>
        decimal OpenPrice { get; }

        /// <summary>
        /// The value of the holding at the <see cref="Tail"/>.
        /// </summary>
        decimal ClosePrice { get; }
    }
}
