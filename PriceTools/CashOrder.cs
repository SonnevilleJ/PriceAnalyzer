using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash order (<see cref="Deposit"/> or <see cref="Withdrawal"/>) for an <see cref="IPortfolio"/>.
    /// </summary>
    public abstract class CashOrder : ITransaction
    {
        #region Private Members

        private readonly DateTime _date;
        private readonly string _ticker;
        private readonly decimal _amount;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a CashOrder.
        /// </summary>
        /// <param name="dateTime">The DateTime of the CashOrder.</param>
        /// <param name="amount">The amount of cash in this order.</param>
        protected internal CashOrder(DateTime dateTime, decimal amount)
            : this(dateTime, string.Empty, amount)
        {}

        /// <summary>
        /// Constructs a CashOrder.
        /// </summary>
        /// <param name="dateTime">The DateTime of the CashOrder.</param>
        /// <param name="ticker">The holding which cash is deposited to or withdrawn from.</param>
        /// <param name="amount">The amount of cash in this order.</param>
        protected internal CashOrder(DateTime dateTime, string ticker, decimal amount)
        {
            _date = dateTime;
            _ticker = ticker;
            _amount = amount;
        }

        #endregion

        #region Implementation of ITransaction

        /// <summary>
        ///   Gets the DateTime that the <see cref="ITransaction"/> occurred.
        /// </summary>
        public DateTime SettlementDate
        {
            get { return _date; }
        }

        /// <summary>
        ///   Gets the <see cref = "ITransaction.OrderType" /> of this <see cref="ITransaction"/>.
        /// </summary>
        public abstract OrderType OrderType { get; }

        /// <summary>
        ///   Gets the ticker symbol of the security traded in this <see cref="ITransaction"/>.
        /// </summary>
        public string Ticker
        {
            get { return _ticker; }
        }

        /// <summary>
        ///   Gets the amount of securities traded in this <see cref="ITransaction"/>.
        /// </summary>
        public double Shares
        {
            get { return double.Parse(_amount.ToString()); }
        }

        /// <summary>
        ///   Gets the value of all securities traded in this <see cref="ITransaction"/>.
        /// </summary>
        public decimal Price
        {
            get { return _amount; }
        }

        /// <summary>
        ///   Gets the commission charged for this <see cref="ITransaction"/>.
        /// </summary>
        public decimal Commission
        {
            get { return 0.0m; }
        }

        #endregion
    }
}