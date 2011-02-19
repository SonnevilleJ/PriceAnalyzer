using System;
using System.Data;
using System.Globalization;
using System.Linq;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a portfolio of investments.
    /// </summary>
    public partial class Portfolio : IPortfolio
    {
        #region Private Members

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
            CashTicker = ticker;
            CashAccount = new CashAccount();
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
            Deposit(dateTime, openingDeposit);
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
                if (Positions.Count > 0)
                {
                    IShareTransaction first = Positions.OrderBy(position => position.Head).First().Transactions.OrderBy(trans => trans.SettlementDate).First();

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
                if (Positions.Count > 0)
                {
                    IShareTransaction first = Positions.OrderBy(position => position.Head).Last().Transactions.OrderBy(trans => trans.SettlementDate).Last();

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
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimeSeries has a valid value for the given date.</returns>
        public bool HasValue(DateTime settlementDate)
        {
            DateTime end = Tail;
            if (GetValue(settlementDate) != 0)
            {
                end = settlementDate;
            }
            return settlementDate >= Head && settlementDate <= end;
        }

        #endregion

        #region Implementation of IPortfolio

        /// <summary>
        ///   Gets the amount of uninvested cash in this Portfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        public decimal GetAvailableCash(DateTime settlementDate)
        {
            return CashAccount.GetCashBalance(settlementDate);
        }

        /// <summary>
        ///   Gets the current total value of this Portfolio.
        /// </summary>
        public decimal GetValue(DateTime settlementDate)
        {
            return GetAvailableCash(settlementDate) + Positions.Sum(position => position.GetInvestedValue(settlementDate));
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
            AddToPosition(ticker, type, settlementDate, shares, price);
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
            AddToPosition(ticker, type, settlementDate, 1.0, price);
        }

        /// <summary>
        /// Deposits cash to this Portfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the deposit.</param>
        /// <param name="cashAmount">The amount of cash deposited.</param>
        public void Deposit(DateTime settlementDate, decimal cashAmount)
        {
            CashAccount.Deposit(settlementDate, cashAmount);
        }

        /// <summary>
        /// Withdraws cash from this Portfolio. AvailableCash must be greater than or equal to the withdrawn amount.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the withdrawal.</param>
        /// <param name="cashAmount">The amount of cash withdrawn.</param>
        public void Withdraw(DateTime settlementDate, decimal cashAmount)
        {
            CashAccount.Withdraw(settlementDate, cashAmount);
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

        private void AddToPosition(string ticker, OrderType type, DateTime date, double shares, decimal price, decimal commission = 0.00m)
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
            Position position = Positions.Where(p => p.Ticker == ticker).FirstOrDefault();
            if(position == null)
            {
                position = new Position(ticker);
                Positions.Add(position);
            }
            return position;
        }
        
        #endregion

        #region Equality Checks

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
                int result = Positions.GetHashCode();
                result = (result * 397) ^ CashAccount.GetHashCode();
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
            if(left.Positions.Count == right.Positions.Count)
            {
                positionsMatch = left.Positions.All(position => right.Positions.Contains(position));
            }

            bool cashMatches = left.CashAccount == right.CashAccount;

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