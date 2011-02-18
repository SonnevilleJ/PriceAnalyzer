using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash deposit to an <see cref="IPortfolio"/>.
    /// </summary>
    public sealed partial class Deposit : CashTransaction
    {
        #region Constructors

        private Deposit()
        {
            OrderType = OrderType.Deposit;
        }

        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Deposit.</param>
        /// <param name="amount">The amount of cash deposited.</param>
        public Deposit(DateTime dateTime, decimal amount)
            : base(dateTime, OrderType.Deposit, amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, "Amount of deposit must be greater than zero.");
            }
        }

        #endregion
    }
}