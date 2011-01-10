using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash withdrawal from an <see cref="IPortfolio"/>.
    /// </summary>
    [Serializable]
    public sealed class Withdrawal : Transaction
    {
        #region Constructors

        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Withdrawal.</param>
        /// <param name="amount">The amount of cash withdrawn.</param>
        public Withdrawal(DateTime dateTime, decimal amount) : this(dateTime, amount, Deposit.DefaultTicker)
        {
        }

        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Withdrawal.</param>
        /// <param name="amount">The amount of cash withdrawn.</param>
        /// <param name="ticker">The holding from which cash is withdrawn.</param>
        public Withdrawal(DateTime dateTime, decimal amount, string ticker) : base(dateTime, OrderType.Withdrawal, ticker, 1.0m, (double)amount, 0.00m)
        {
        }

        #endregion

        public override decimal Price
        {
            get
            {
                return -1 * base.Price;
            }
        }
    }
}