using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a portfolio of investments.
    /// </summary>
    public class Portfolio : IPortfolio
    {
        #region Private Members

        private readonly string _ticker = String.Empty;
        private readonly IDictionary<string, IPosition> _positions = new Dictionary<string, IPosition>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a Portfolio.
        /// </summary>
        public Portfolio()
        {
            
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        public Portfolio(DateTime dateTime, decimal openingDeposit)
            : this(dateTime, openingDeposit, String.Empty)
        {}

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        public Portfolio(DateTime dateTime, decimal openingDeposit, string ticker)
        {
            _ticker = ticker.ToUpperInvariant();
            Deposit(dateTime, openingDeposit);
        }

        #endregion

        #region Implementation of ISerializable

        /// <summary>
        /// Deserializes a Portfolio.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected Portfolio(SerializationInfo info, StreamingContext context)
        {
            _ticker = info.GetString("Ticker");
            _positions = (IDictionary<string, IPosition>) info.GetValue("Positions", typeof (Dictionary<string, IPosition>));
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Ticker", _ticker);
            info.AddValue("Positions", _positions);
        }

        #endregion

        #region Implementation of ITimeSeries

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        public decimal this[DateTime index]
        {
            get { return GetValue(index); }
        }

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public DateTime Head
        {
            get
            {
                DateTime earliest = DateTime.Now;
                // next line produces "Access to modified closure" warning in Resharper. This is expected/desired behavior.
                foreach (var position in Positions.Values.Where(position => position.Head <= earliest))
                {
                    earliest = position.Head;
                }
                return earliest;
            }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public DateTime Tail
        {
            get
            {
                DateTime latest = new DateTime();
                // next line produces "Access to modified closure" warning in Resharper. This is expected/desired behavior.
                foreach (var position in Positions.Values.Where(position => position.Tail >= latest))
                {
                    latest = position.Head;
                }
                return latest;
            }
        }

        /// <summary>
        /// Determines if the ITimeSeries has a valid value for a given date.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns>A value indicating if the ITimeSeries has a valid value for the given date.</returns>
        public bool HasValue(DateTime date)
        {
            return date > Head && date < Tail;
        }

        #endregion

        #region Implementation of IPortfolio

        /// <summary>
        ///   Gets an <see cref = "IList{T}" /> of positions held in this IPortfolio.
        /// </summary>
        public IDictionary<string, IPosition> Positions
        {
            get { return _positions; }
        }

        /// <summary>
        ///   Gets the amount of uninvested cash in this IPortfolio.
        /// </summary>
        /// <param name="asOfDate">The <see cref="DateTime"/> to use.</param>
        public decimal GetAvailableCash(DateTime asOfDate)
        {
            return Positions.Values.Where(position => position.Ticker == CashTicker).Sum(position => position.GetValue(asOfDate));
        }

        /// <summary>
        /// Gets or sets the ticker to use for the holding of cash in this IPortfolio.
        /// </summary>
        public string CashTicker
        {
            get { return _ticker; }
        }

        /// <summary>
        ///   Gets the current total value of this IPortfolio.
        /// </summary>
        public decimal GetValue(DateTime asOfDate)
        {
            return Positions.Values.Sum(position => position.GetValue(asOfDate));
        }

        /// <summary>
        ///   Adds an <see cref="ITransaction"/> to this IPortfolio.
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/> of the transaction.</param>
        /// <param name="type">The <see cref="OrderType"/> of the transaction.</param>
        /// <param name="ticker">The ticker symbol to use for the transaction.</param>
        /// <param name="price">The per-share price of the ticker symbol.</param>
        /// <param name="shares">The number of shares.</param>
        /// <param name="commission">The commission charge for the transaction.</param>
        public void AddTransaction(DateTime date, OrderType type, string ticker, decimal price, double shares, decimal commission)
        {
            ITransaction transaction = new Transaction(date, type, ticker, price, shares, commission);
            AddToPosition(transaction);
        }

        /// <summary>
        ///   Adds an <see cref="ITransaction"/> to this IPortfolio.
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/> of the transaction.</param>
        /// <param name="type">The <see cref="OrderType"/> of the transaction.</param>
        /// <param name="ticker">The ticker symbol to use for the transaction.</param>
        /// <param name="price">The per-share price of the ticker symbol.</param>
        /// <param name="shares">The number of shares.</param>
        public void AddTransaction(DateTime date, OrderType type, string ticker, decimal price, double shares)
        {
            ITransaction transaction = new Transaction(date, type, ticker, price, shares);
            AddToPosition(transaction);
        }

        /// <summary>
        ///   Adds an <see cref="ITransaction"/> to this IPortfolio.
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/> of the transaction.</param>
        /// <param name="type">The <see cref="OrderType"/> of the transaction.</param>
        /// <param name="ticker">The ticker symbol to use for the transaction.</param>
        /// <param name="price">The per-share price of the ticker symbol.</param>
        public void AddTransaction(DateTime date, OrderType type, string ticker, decimal price)
        {
            ITransaction transaction = new Transaction(date, type, ticker, price);
            AddToPosition(transaction);
        }

        /// <summary>
        /// Deposits cash to this IPortfolio.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> of the deposit.</param>
        /// <param name="cashAmount">The amount of cash deposited.</param>
        public void Deposit(DateTime dateTime, decimal cashAmount)
        {
            Deposit deposit = new Deposit(dateTime, cashAmount, CashTicker);
            AddToPosition(deposit);
        }

        /// <summary>
        /// Withdraws cash from this IPortfolio. AvailableCash must be greater than or equal to the withdrawn amount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> of the withdrawal.</param>
        /// <param name="cashAmount">The amount of cash withdrawn.</param>
        public void Withdraw(DateTime dateTime, decimal cashAmount)
        {
            Withdrawal withdrawal = new Withdrawal(dateTime, cashAmount, CashTicker);
            AddToPosition(withdrawal);
        }

        #endregion

        #region Private Methods

        private void AddToPosition(ITransaction transaction)
        {
            string ticker = transaction.Ticker;
            IPosition position;
            if (_positions.TryGetValue(ticker, out position))
            {
                position.AddTransaction(transaction);
            }
            else
            {
                position = new Position(transaction);
                _positions[ticker] = position;
            }
        }

        #endregion
    }
}