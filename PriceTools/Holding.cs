using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A prior ownership of a financial security.
    /// </summary>
    public struct Holding : IHolding
    {
        #region Implementation of IHolding

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
        public double Shares { get; set; }

        /// <summary>
        /// The value of the holding at the <see cref="IHolding.Head"/>.
        /// </summary>
        public decimal OpenPrice { get; set; }

        /// <summary>
        /// The value of the holding at the <see cref="IHolding.Tail"/>.
        /// </summary>
        public decimal ClosePrice { get; set; }

        #endregion
    }
}