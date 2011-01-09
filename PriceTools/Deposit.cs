using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash deposit to an <see cref="IPortfolio"/>.
    /// </summary>
    [Serializable]
    public class Deposit : CashOrderBase
    {
        #region Constructors

        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Deposit.</param>
        /// <param name="amount">The amount of cash deposited.</param>
        public Deposit(DateTime dateTime, decimal amount) : base(dateTime, OrderType.Deposit, amount)
        {
        }

        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Deposit.</param>
        /// <param name="amount">The amount of cash deposited.</param>
        /// <param name="ticker">The holding to which cash is deposited.</param>
        public Deposit(DateTime dateTime, decimal amount, string ticker) : base(dateTime, OrderType.Deposit, ticker, -1 * amount)
        {
        }

        #endregion
    }
}