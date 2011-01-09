using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash withdrawal from an <see cref="IPortfolio"/>.
    /// </summary>
    [Serializable]
    public sealed class Withdrawal : CashOrderBase
    {
        #region Constructors

        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Withdrawal.</param>
        /// <param name="amount">The amount of cash withdrawn.</param>
        public Withdrawal(DateTime dateTime, decimal amount) : base(dateTime, PriceTools.OrderType.Withdrawal, amount)
        {
        }

        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Withdrawal.</param>
        /// <param name="amount">The amount of cash withdrawn.</param>
        /// <param name="ticker">The holding from which cash is withdrawn.</param>
        public Withdrawal(DateTime dateTime, decimal amount, string ticker) : base(dateTime, PriceTools.OrderType.Withdrawal, ticker, amount)
        {
        }

        #endregion
    }
}