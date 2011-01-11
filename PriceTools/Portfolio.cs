using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a portfolio of investments.
    /// </summary>
    [Serializable]
    public class Portfolio : IPortfolio
    {
        #region Private Members

        private readonly string _cashTicker = String.Empty;
        private readonly IDictionary<string, IPosition> _positions = new Dictionary<string, IPosition>();
        private readonly ICollection<ITransaction> _cashTransactions = new List<ITransaction>();

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
        {
            
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit invested in a given ticker symbol.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        /// <param name="ticker">The ticker symbol the deposit is invested in.</param>
        public Portfolio(DateTime dateTime, decimal openingDeposit, string ticker)
        {
            _cashTicker = ticker.ToUpperInvariant();
            Deposit(dateTime, openingDeposit);
        }

        /// <summary>
        /// Constructs a Portfolio from a <see cref="TransactionHistoryCsvFile"/>.
        /// </summary>
        /// <param name="csvFile">The <see cref="TransactionHistoryCsvFile"/> containing transaction data.</param>
        public Portfolio(TransactionHistoryCsvFile csvFile)
        {
            AddTransactionHistory(csvFile);
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
            _cashTicker = info.GetString("Ticker");
            _positions = (IDictionary<string, IPosition>) info.GetValue("Positions", typeof (Dictionary<string, IPosition>));
            _cashTransactions = (IList<ITransaction>) info.GetValue("CashTransactions", typeof (List<ITransaction>));
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Ticker", _cashTicker);
            info.AddValue("Positions", _positions);
            info.AddValue("CashTransactions", _cashTransactions);
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
                // loops produce "Access to modified closure" warning in Resharper. This is expected/desired behavior.
                foreach (var position in Positions.Values.Where(position => position.Head < earliest))
                {
                    earliest = position.Head;
                }
                foreach (var transaction in CashTransactions.Where(transaction => transaction.SettlementDate < earliest))
                {
                    earliest = transaction.SettlementDate;
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
                // loops produce "Access to modified closure" warning in Resharper. This is expected/desired behavior.
                foreach (var position in Positions.Values.Where(position => position.Tail > latest))
                {
                    latest = position.Tail;
                }
                foreach (var transaction in CashTransactions.Where(transaction => transaction.SettlementDate > latest))
                {
                    latest = transaction.SettlementDate;
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
            DateTime end = Tail;
            if (GetValue(date) != 0)
            {
                end = date;
            }
            return date >= Head && date <= end;
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
        ///   Gets an <see cref="IList{T}"/> of cash transactions in this IPortfolio.
        /// </summary>
        public ICollection<ITransaction> CashTransactions
        {
            get { return _cashTransactions; }
        }

        /// <summary>
        ///   Gets the amount of uninvested cash in this IPortfolio.
        /// </summary>
        /// <param name="asOfDate">The <see cref="DateTime"/> to use.</param>
        public decimal GetAvailableCash(DateTime asOfDate)
        {
            decimal cashDeposited = CashTransactions.Sum(transaction => transaction.Price*(decimal) transaction.Shares);
            decimal cashInvested = Positions.Values.Aggregate<IPosition, decimal>(0, (current, position) => current - position.GetValue(asOfDate, true));
            return 0 - (cashDeposited + cashInvested);
        }

        /// <summary>
        /// Gets or sets the ticker to use for the holding of cash in this IPortfolio.
        /// </summary>
        public string CashTicker
        {
            get { return _cashTicker; }
        }

        /// <summary>
        ///   Gets the current total value of this IPortfolio.
        /// </summary>
        public decimal GetValue(DateTime asOfDate)
        {
            return Positions.Values.Sum(position => position.GetValue(asOfDate)) + GetAvailableCash(asOfDate);
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
            AddToPosition(ticker, type, date, shares, price, commission);
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
            AddToPosition(ticker, type, date, shares, price, Position.DefaultCommission);
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
            AddToPosition(ticker, type, date, 1.0, price, Position.DefaultCommission);
        }

        /// <summary>
        /// Deposits cash to this IPortfolio.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> of the deposit.</param>
        /// <param name="cashAmount">The amount of cash deposited.</param>
        public void Deposit(DateTime dateTime, decimal cashAmount)
        {
            ITransaction deposit = TransactionFactory.CreateDeposit(dateTime, cashAmount, CashTicker);
            _cashTransactions.Add(deposit);
        }

        /// <summary>
        /// Withdraws cash from this IPortfolio. AvailableCash must be greater than or equal to the withdrawn amount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> of the withdrawal.</param>
        /// <param name="cashAmount">The amount of cash withdrawn.</param>
        public void Withdraw(DateTime dateTime, decimal cashAmount)
        {
            VerifyAvailableCash(dateTime, cashAmount);
            Withdrawal withdrawal = new Withdrawal(dateTime, cashAmount, CashTicker);
            _cashTransactions.Add(withdrawal);
        }

        /// <summary>
        /// Adds transaction history from a CSV file to the Portfolio.
        /// </summary>
        /// <param name="csvFile">The CSV file containing the transactions to add.</param>
        public void AddTransactionHistory(TransactionHistoryCsvFile csvFile)
        {
            csvFile.Parse();

            foreach (DataRow row in csvFile.DataTable.Rows)
            {
                AddTransaction((DateTime)row[csvFile.DateColumn],
                               (OrderType)row[csvFile.OrderColumn],
                               (string)row[csvFile.SymbolColumn],
                               (decimal)row[csvFile.PriceColumn],
                               (double)row[csvFile.SharesColumn],
                               (decimal)row[csvFile.CommissionColumn]);
            }
        }

        #endregion

        #region Private Methods

        private void AddToPosition(string ticker, OrderType type, DateTime date, double shares, decimal price, decimal commission)
        {
            if(type == OrderType.Deposit)
            {
                VerifyCashTicker(ticker);
                Deposit(date, (decimal)shares * price);
            }
            if(type == OrderType.Withdrawal)
            {
                VerifyCashTicker(ticker);
                Withdraw(date, (decimal)shares * price);
            }

            IPosition position;
            if (!_positions.TryGetValue(ticker, out position))
            {
                position = new Position(ticker);
                _positions[ticker] = position;
            }

            switch (type)
            {
                case OrderType.Buy:
                    VerifyAvailableCash(date, shares, price, commission);
                    position.Buy(date, shares, price, commission);
                    break;
                case OrderType.SellShort:
                    VerifyAvailableCash(date, shares, price, commission);
                    position.Sell(date, shares, price, commission);
                    break;
                case OrderType.Sell:
                    position.Sell(date, shares, price, commission);
                    break;
                case OrderType.BuyToCover:
                    position.BuyToCover(date, shares, price, commission);
                    break;
            }
        }

        private void VerifyAvailableCash(DateTime date, double shares, decimal price, decimal commission)
        {
            var cost = (decimal)shares * price + commission;
            VerifyAvailableCash(date, cost);
        }

        private void VerifyAvailableCash(DateTime date, decimal amount)
        {
            decimal availableCash = GetAvailableCash(date);
            if (amount > availableCash)
            {
                throw new InvalidOperationException(
                    String.Format("This transaction requires more cash than available at {0}.", date));
            }
        }

        private void VerifyCashTicker(string ticker)
        {
            if (ticker != _cashTicker)
            {
                throw new InvalidOperationException(String.Format("Cash transactions for this Portfolio must use ticker {0}.", _cashTicker));
            }
        }

        #endregion

        #region Equality Checks

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ITimeSeries other)
        {
            return Equals((object)other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IPortfolio other)
        {
            return Equals((object)other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Portfolio)) return false;
            return this == (Portfolio)obj;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = _positions.GetHashCode();
                result = (result * 397) ^ _cashTicker.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Portfolio left, Portfolio right)
        {
            bool positionsMatch = false;
            if(left._positions.Count == right._positions.Count)
            {
                positionsMatch = left._positions.Values.All(position => right._positions.Values.Contains(position));
            }

            bool cashMatches = false;
            if (left._cashTransactions.Count == right._cashTransactions.Count)
            {
                cashMatches = left._cashTransactions.All(transaction => right._cashTransactions.Contains(transaction));
            }

            bool tickersMatch = left._cashTicker == right._cashTicker;

            return positionsMatch && cashMatches && tickersMatch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Portfolio left, Portfolio right)
        {
            return !(left == right);
        }

        #endregion
    }
}