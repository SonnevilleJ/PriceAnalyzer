using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash deposit to an <see cref="IPortfolio"/>.
    /// </summary>
    [Serializable]
    public sealed class Deposit : Transaction
    {
        internal static readonly string DefaultTicker = String.Empty;

        #region Constructors

        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Deposit.</param>
        /// <param name="amount">The amount of cash deposited.</param>
        public Deposit(DateTime dateTime, decimal amount) : this(dateTime, amount, DefaultTicker)
        {
        }

        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Deposit.</param>
        /// <param name="amount">The amount of cash deposited.</param>
        /// <param name="ticker">The holding to which cash is deposited.</param>
        public Deposit(DateTime dateTime, decimal amount, string ticker) : base(dateTime, OrderType.Deposit, ticker, 1.0m, (double)amount, 0.00m)
        {
        }

        #endregion
    }
}