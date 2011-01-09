using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash deposit to an <see cref="IPortfolio"/>.
    /// </summary>
    [Serializable]
    public class Deposit : CashOrderBase
    {
        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Deposit.</param>
        /// <param name="amount">The amount of cash deposited.</param>
        public Deposit(DateTime dateTime, decimal amount) : base(dateTime, amount)
        {
        }

        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Deposit.</param>
        /// <param name="amount">The amount of cash deposited.</param>
        /// <param name="ticker">The holding to which cash is deposited.</param>
        public Deposit(DateTime dateTime, decimal amount, string ticker) : base(dateTime, ticker, -1 * amount)
        {
        }

        #region Overrides of CashOrderBase

        /// <summary>
        ///   Gets the <see cref = "ITransaction.OrderType" /> of this <see cref="ITransaction"/>.
        /// </summary>
        public override OrderType OrderType
        {
            get { return OrderType.Deposit; }
        }

        /// <summary>
        /// Gets the per-share price paid for this <see cref="ITransaction"/>.
        /// </summary>
        public override decimal Price
        {
            get { return 1.0m; }
        }

        #endregion
    }
}