using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash withdrawal from an <see cref="IPortfolio"/>.
    /// </summary>
    public sealed class Withdrawal : CashOrderBase
    {
        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Withdrawal.</param>
        /// <param name="amount">The amount of cash withdrawn.</param>
        public Withdrawal(DateTime dateTime, decimal amount) : base(dateTime, amount)
        {
        }

        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Withdrawal.</param>
        /// <param name="ticker">The holding from which cash is withdrawn.</param>
        /// <param name="amount">The amount of cash withdrawn.</param>
        public Withdrawal(DateTime dateTime, string ticker, decimal amount) : base(dateTime, ticker, amount)
        {
        }

        #region Overrides of CashOrderBase

        /// <summary>
        ///   Gets the <see cref = "ITransaction.OrderType" /> of this <see cref="ITransaction"/>.
        /// </summary>
        public override OrderType OrderType
        {
            get { return OrderType.Withdrwawal; }
        }

        #endregion
    }
}