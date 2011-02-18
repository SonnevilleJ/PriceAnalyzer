using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash withdrawal from an <see cref="IPortfolio"/>.
    /// </summary>
    public sealed partial class Withdrawal : CashTransaction
    {
        #region Constructors

        private Withdrawal()
        {
            OrderType = OrderType.Withdrawal;
        }

        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Withdrawal.</param>
        /// <param name="amount">The amount of cash withdrawn.</param>
        public Withdrawal(DateTime dateTime, decimal amount)
            : base(dateTime, OrderType.Withdrawal, amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, "Amount of withdrawal must be greater than zero.");
            }
        }

        #endregion
    }
}