using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash order (<see cref="Deposit"/> or <see cref="Withdrawal"/>) for an <see cref="IPortfolio"/>.
    /// </summary>
    [Serializable]
    public abstract class CashOrderBase : Transaction
    {
        #region Constructors

        /// <summary>
        /// Constructs a CashOrderBase.
        /// </summary>
        /// <param name="dateTime">The DateTime of the CashOrderBase.</param>
        /// <param name="type">The <see cref="OrderType"/> of the CashOrderBase</param>
        /// <param name="amount">The amount of cash in this order.</param>
        protected internal CashOrderBase(DateTime dateTime, OrderType type, decimal amount)
            : this(dateTime, type, string.Empty, amount)
        {
        }

        /// <summary>
        /// Constructs a CashOrderBase.
        /// </summary>
        /// <param name="dateTime">The DateTime of the CashOrderBase.</param>
        /// <param name="type">The <see cref="OrderType"/> of the CashOrderBase</param>
        /// <param name="ticker">The holding which cash is deposited to or withdrawn from.</param>
        /// <param name="amount">The amount of cash in this order.</param>
        protected internal CashOrderBase(DateTime dateTime, OrderType type, string ticker, decimal amount)
            : base(dateTime, type, ticker, amount)
        {
        }

        #endregion

        #region Overrides of Transaction

        /// <summary>
        ///   Gets the amount of securities traded in this <see cref="ITransaction"/>.
        /// </summary>
        public override double Shares
        {
            get { return (double) Price; }
        }

        /// <summary>
        ///   Gets the commission charged for this <see cref="ITransaction"/>.
        /// </summary>
        public override decimal Commission
        {
            get { return 0.0m; }
        }

        #endregion
    }
}