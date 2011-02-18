using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

        private readonly IDictionary<string, IPosition> _positions = new Dictionary<string, IPosition>();
        private readonly CashAccount _cashAccount;
        private readonly string _cashTicker;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a Portfolio.
        /// </summary>
        public Portfolio()
            : this(String.Empty)
        {            
        }

        /// <summary>
        /// Constructs a Portfolio and assigns a ticker symbol to use as the Portfolio's <see cref="ICashAccount"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="ICashAccount"/>.</param>
        public Portfolio(string ticker)
        {
            _cashTicker = ticker;
            _cashAccount = new CashAccount();
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
            : this(ticker)
        {
            if (openingDeposit > 0)
            {
                Deposit(dateTime, openingDeposit);
            }
        }

        /// <summary>
        /// Constructs a Portfolio from a <see cref="TransactionHistoryCsvFile"/>.
        /// </summary>
        /// <param name="csvFile">The <see cref="TransactionHistoryCsvFile"/> containing transaction data.</param>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="ICashAccount"/>.</param>
        public Portfolio(TransactionHistoryCsvFile csvFile, string ticker)
            : this(ticker)
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
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _positions = (IDictionary<string, IPosition>) info.GetValue("Positions", typeof (Dictionary<string, IPosition>));
            _cashAccount = (CashAccount)info.GetValue("CashAccount", typeof(CashAccount));
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("Positions", _positions);
            info.AddValue("CashAccount", _cashAccount);
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
                if(CashAccount.Transactions.Count > 0)
                {
                    ICashTransaction first = CashAccount.Transactions.OrderBy(transaction => transaction.SettlementDate).First();
                    
                    earliest = first.SettlementDate;
                }
                if (Positions.Values.Count > 0)
                {
                    IShareTransaction first = Positions.Values.OrderBy(position => position.Head).First().Transactions.OrderBy(trans => trans.SettlementDate).First();

                    earliest = first.SettlementDate;
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
                DateTime latest = DateTime.Now;
                if (Positions.Values.Count > 0)
                {
                    IShareTransaction first = Positions.Values.OrderBy(position => position.Head).Last().Transactions.OrderBy(trans => trans.SettlementDate).Last();

                    latest = first.SettlementDate;
                }
                if (CashAccount.Transactions.Count > 0)
                {
                    ICashTransaction first = CashAccount.Transactions.OrderBy(transaction => transaction.SettlementDate).Last();

                    latest = first.SettlementDate;
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
        ///   Gets an <see cref = "IList{T}" /> of positions held in this Portfolio.
        /// </summary>
        public IDictionary<string, IPosition> Positions
        {
            get { return _positions; }
        }

        /// <summary>
        ///   Gets the <see cref="ICashAccount"/> used by this Portfolio.
        /// </summary>
        public ICashAccount CashAccount
        {
            get { return _cashAccount; }
        }



        /// <summary>
        ///   Gets the amount of uninvested cash in this Portfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        public decimal GetAvailableCash(DateTime settlementDate)
        {
            return _cashAccount.GetCashBalance(settlementDate);
        }

        /// <summary>
        /// Gets or sets the ticker to use for the holding of cash in this Portfolio.
        /// </summary>
        public string CashTicker
        {
            get { return _cashTicker; }
        }

        /// <summary>
        ///   Gets the current total value of this Portfolio.
        /// </summary>
        public decimal GetValue(DateTime settlementDate)
        {
            return GetAvailableCash(settlementDate) + Positions.Values.Sum(position => position.GetInvestedValue(settlementDate));
        }

        /// <summary>
        ///   Adds an <see cref="IShareTransaction"/> to this Portfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the transaction.</param>
        /// <param name="type">The <see cref="OrderType"/> of the transaction.</param>
        /// <param name="ticker">The ticker symbol to use for the transaction.</param>
        /// <param name="price">The per-share price of the ticker symbol.</param>
        /// <param name="shares">The number of shares.</param>
        /// <param name="commission">The commission charge for the transaction.</param>
        public void AddTransaction(DateTime settlementDate, OrderType type, string ticker, decimal price, double shares, decimal commission)
        {
            AddToPosition(ticker, type, settlementDate, shares, price, commission);
        }

        /// <summary>
        ///   Adds an <see cref="IShareTransaction"/> to this Portfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the transaction.</param>
        /// <param name="type">The <see cref="OrderType"/> of the transaction.</param>
        /// <param name="ticker">The ticker symbol to use for the transaction.</param>
        /// <param name="price">The per-share price of the ticker symbol.</param>
        /// <param name="shares">The number of shares.</param>
        public void AddTransaction(DateTime settlementDate, OrderType type, string ticker, decimal price, double shares)
        {
            AddToPosition(ticker, type, settlementDate, shares, price, Position.DefaultCommission);
        }

        /// <summary>
        ///   Adds an <see cref="IShareTransaction"/> to this Portfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the transaction.</param>
        /// <param name="type">The <see cref="OrderType"/> of the transaction.</param>
        /// <param name="ticker">The ticker symbol to use for the transaction.</param>
        /// <param name="price">The per-share price of the ticker symbol.</param>
        public void AddTransaction(DateTime settlementDate, OrderType type, string ticker, decimal price)
        {
            AddToPosition(ticker, type, settlementDate, 1.0, price, Position.DefaultCommission);
        }

        /// <summary>
        /// Deposits cash to this Portfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the deposit.</param>
        /// <param name="cashAmount">The amount of cash deposited.</param>
        public void Deposit(DateTime settlementDate, decimal cashAmount)
        {
            _cashAccount.Deposit(settlementDate, cashAmount);
        }

        /// <summary>
        /// Withdraws cash from this Portfolio. AvailableCash must be greater than or equal to the withdrawn amount.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the withdrawal.</param>
        /// <param name="cashAmount">The amount of cash withdrawn.</param>
        public void Withdraw(DateTime settlementDate, decimal cashAmount)
        {
            _cashAccount.Withdraw(settlementDate, cashAmount);
        }
        /// <summary>
        /// Adds transaction history from a CSV file to the Portfolio.
        /// </summary>
        /// <param name="csvFile">The CSV file containing the transactions to add.</param>
        public void AddTransactionHistory(TransactionHistoryCsvFile csvFile)
        {
            if (csvFile == null)
            {
                throw new ArgumentNullException("csvFile");
            }

            csvFile.Parse();

            // need to add transactions IN ORDER (oldest to newest)
            DataRow[] rows = csvFile.DataTable.Select(null, "Date ASC");

            foreach (DataRow row in rows)
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
            switch (type)
            {
                case OrderType.DividendReceipt:
                case OrderType.Deposit:
                    if (ticker == CashTicker || String.IsNullOrWhiteSpace(ticker))
                    {
                        Deposit(date, price);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(String.Format(CultureInfo.CurrentCulture, "Deposits must use ticker: {0}.", CashTicker));
                    }
                    break;
                case OrderType.Withdrawal:
                    Withdraw(date, price);
                    break;
                case OrderType.DividendReinvestment:
                    if (ticker == CashTicker)
                    {
                        // DividendReceipt already deposited into cash account, so no need to "buy" the CashTicker. Do nothing.
                    }
                    else
                    {
                        Withdraw(date, (price * (decimal)shares) + commission);
                        GetPosition(ticker).Buy(date, shares, price, commission);
                    }
                    break;
                case OrderType.Buy:
                    Withdraw(date, (price * (decimal)shares) + commission);
                    GetPosition(ticker).Buy(date, shares, price, commission);
                    break;
                case OrderType.SellShort:
                    Withdraw(date, (price * (decimal)shares) + commission);
                    GetPosition(ticker).Sell(date, shares, price, commission);
                    break;
                case OrderType.Sell:
                    GetPosition(ticker).Sell(date, shares, price, commission);
                    Deposit(date, ((decimal)shares * price) - commission);
                    break;
                case OrderType.BuyToCover:
                    GetPosition(ticker).BuyToCover(date, shares, price, commission);
                    Deposit(date, ((decimal)shares * price) - commission);
                    break;
            }
        }

        private IPosition GetPosition(string ticker)
        {
            IPosition position;
            if (!_positions.TryGetValue(ticker, out position))
            {
                position = new Position(ticker);
                _positions[ticker] = position;
            }
            return position;
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
                result = (result * 397) ^ _cashAccount.GetHashCode();
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

            bool cashMatches = left._cashAccount == right._cashAccount;

            return positionsMatch && cashMatches;
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