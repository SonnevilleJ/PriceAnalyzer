using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a portfolio of investments.
    /// </summary>
    public class Portfolio : IPortfolio
    {
        #region Private Members

        private static readonly string DefaultCashTicker = String.Empty;
        private readonly CashAccount _cashAccount = new CashAccount();
        private readonly ICollection<IPosition> _positions = new List<IPosition>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a Portfolio.
        /// </summary>
        public Portfolio()
            : this(DefaultCashTicker)
        {
        }

        /// <summary>
        /// Constructs a Portfolio and assigns a ticker symbol to use as the Portfolio's <see cref="ICashAccount"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="ICashAccount"/>.</param>
        public Portfolio(string ticker)
        {
            CashTicker = ticker;
            _cashAccount = new CashAccount();
        }

        /// <summary>
        /// Constructs a Portfolio with an opening deposit.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposit in the Portfolio.</param>
        /// <param name="openingDeposit">The cash amount deposited into the Portfolio.</param>
        public Portfolio(DateTime dateTime, decimal openingDeposit)
            : this(dateTime, openingDeposit, DefaultCashTicker)
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
        public Portfolio(TransactionHistoryCsvFile csvFile)
            : this(csvFile, DefaultCashTicker)
        {
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
            get { return CalculateValue(index); }
        }

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public DateTime Head
        {
            get
            {
                var earliest = DateTime.Now;
                if (_cashAccount.Transactions.Count > 0)
                {
                    var first = _cashAccount.Transactions.OrderBy(transaction => transaction.SettlementDate).First();

                    earliest = first.SettlementDate;
                }
                // An opening deposit is required before transactions will be processed.
                // Therefore, the below code should never be necessary.
                //
                //if (Positions.Count > 0)
                //{
                //    IShareTransaction first = Positions.OrderBy(position => position.Head).First().Transactions.OrderBy(trans => trans.SettlementDate).First();

                //    earliest = first.SettlementDate;
                //}

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
                DateTime? latest = null;
                if (Positions.Count > 0)
                    latest = Positions.OrderBy(position => position.Tail).Last().Transactions.OrderBy(trans => trans.SettlementDate).Last().SettlementDate;
                var cashTransactions = _cashAccount.Transactions.Where(transaction => transaction.SettlementDate > (latest ?? DateTime.MinValue));
                if (cashTransactions.Count() > 0)
                    latest = cashTransactions.Max(t=>t.SettlementDate);

                return latest ?? DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the <see cref="ITimeSeries.Resolution"/> of price data stored within the ITimeSeries.
        /// </summary>
        public Resolution Resolution
        {
            get { return Positions.Min(p => p.Resolution); }
        }

        /// <summary>
        /// Gets the values stored within the ITimeSeries.
        /// </summary>
        public IDictionary<DateTime, decimal> Values
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Determines if the ITimeSeries has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimeSeries has a valid value for the given date.</returns>
        public bool HasValueInRange(DateTime settlementDate)
        {
            var end = Tail;
            if (CalculateValue(settlementDate) != 0)
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
            return _cashAccount.GetCashBalance(settlementDate);
        }

        /// <summary>
        /// Gets or sets the ticker to use for the holding of cash in this IPortfolio.
        /// </summary>
        public string CashTicker { get; private set; }

        /// <summary>
        ///   Gets the value of this Portfolio, excluding any commissions, as of a given date..
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        public decimal CalculateValue(DateTime settlementDate)
        {
            var availableCash = GetAvailableCash(settlementDate);
            var proceeds = Positions.Sum(position => position.CalculateProceeds(settlementDate));
            var commissionsPaid = Positions.Sum(position => position.CalculateCommissions(settlementDate));
            var value = Positions.Sum(position => position.CalculateValue(settlementDate));
            return availableCash - proceeds + commissionsPaid + value;
        }

        /// <summary>
        ///   Gets the total value of the Portfolio, including any commissions, as of a given date.
        /// </summary>
        /// <param name="provider">The <see cref="PriceSeriesProvider"/> to use when requesting price data.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total value of the Portfolio as of the given date.</returns>
        public decimal CalculateTotalValue(PriceSeriesProvider provider, DateTime settlementDate)
        {
            var cash = GetAvailableCash(settlementDate);
            var invested = Positions.Sum(position => position.CalculateInvestedValue(provider, settlementDate));
            return cash + invested;
        }

        /// <summary>
        ///   Gets the gross investment of this Portfolio, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases.</returns>
        public decimal CalculateCost(DateTime settlementDate)
        {
            return Positions.Sum(p => p.CalculateCost(settlementDate));
        }

        /// <summary>
        ///   Gets the gross proceeds of this Portfolio, ignoring all totalCosts and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales.</returns>
        public decimal CalculateProceeds(DateTime settlementDate)
        {
            return Positions.Sum(p => p.CalculateProceeds(settlementDate));
        }

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "IShareTransaction" />s.</returns>
        public decimal CalculateCommissions(DateTime settlementDate)
        {
            return Positions.Sum(p => p.CalculateCommissions(settlementDate));
        }

        /// <summary>
        ///   Gets an enumeration of all <see cref = "IShareTransaction" />s in this IPosition.
        /// </summary>
        public IList<ITransaction> Transactions
        {
            get
            {
                var list = new List<ITransaction>();
                foreach (var p in Positions)
                {
                    list.AddRange(p.Transactions);
                }
                list.AddRange(_cashAccount.Transactions);

                return list.OrderBy(t => t.SettlementDate).ToList();
            }
        }

        /// <summary>
        ///   Gets the value of all shares held the Portfolio as of a given date.
        /// </summary>
        /// <param name="provider">The <see cref="PriceSeriesProvider"/> to use when requesting price data.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the shares held in the Portfolio as of the given date.</returns>
        public decimal CalculateInvestedValue(PriceSeriesProvider provider, DateTime settlementDate)
        {
            return Positions.Sum(p => p.CalculateInvestedValue(provider, settlementDate));
        }

        /// <summary>
        ///   Adds an <see cref="ITransaction"/> to this Portfolio.
        /// </summary>
        public void AddTransaction(ITransaction transaction)
        {
            switch (transaction.OrderType)
            {
                case OrderType.DividendReceipt:
                    _cashAccount.Deposit((DividendReceipt)transaction);
                    break;
                case OrderType.Deposit:
                    Deposit((Deposit)transaction);
                    break;
                case OrderType.Withdrawal:
                    Withdraw((Withdrawal)transaction);
                    break;
                case OrderType.DividendReinvestment:
                    var dr = ((DividendReinvestment)transaction);
                    if (dr.Ticker == CashTicker)
                    {
                        // DividendReceipt already deposited into cash account,
                        // so no need to "buy" the CashTicker. Do nothing.
                    }
                    else
                    {
                        Withdraw(dr.SettlementDate, dr.TotalValue);
                        AddToPosition(dr);
                    }
                    break;
                case OrderType.Buy:
                    var buy = ((Buy)transaction);
                    Withdraw(buy.SettlementDate, buy.TotalValue);
                    AddToPosition(buy);
                    break;
                case OrderType.SellShort:
                    var sellShort = ((SellShort)transaction);
                    Withdraw(sellShort.SettlementDate, sellShort.TotalValue);
                    AddToPosition(sellShort);
                    break;
                case OrderType.Sell:
                    var sell = ((Sell)transaction);
                    AddToPosition(sell);
                    Deposit(sell.SettlementDate, sell.TotalValue);
                    break;
                case OrderType.BuyToCover:
                    var buyToCover = ((BuyToCover)transaction);
                    AddToPosition(buyToCover);
                    Deposit(buyToCover.SettlementDate, buyToCover.TotalValue);
                    break;
            }
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
        /// Deposits cash to this Portfolio.
        /// </summary>
        /// <param name="deposit">The <see cref="PriceTools.Deposit"/> to deposit.</param>
        public void Deposit(Deposit deposit)
        {
            _cashAccount.Deposit(deposit);
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
        /// Withdraws cash from this Portfolio. Available cash must be greater than or equal to the withdrawn amount.
        /// </summary>
        /// <param name="withdrawal">The <see cref="PriceTools.Withdrawal"/> to withdraw.</param>
        public void Withdraw(Withdrawal withdrawal)
        {
            _cashAccount.Withdraw(withdrawal);
        }

        /// <summary>
        /// Adds transaction history from a CSV file to the Portfolio.
        /// </summary>
        /// <param name="csvFile">The CSV file containing the transactions to add.</param>
        public void AddTransactionHistory(TransactionHistoryCsvFile csvFile)
        {
            foreach (var transaction in csvFile.Transactions)
            {
                AddTransaction(transaction);
            }
        }

        /// <summary>
        /// Gets an <see cref="IList{IHolding}"/> from the transactions in the Portfolio.
        /// </summary>
        /// <param name="settlementDate">The latest date used to include a transaction in the calculation.</param>
        /// <returns>An <see cref="IList{IHolding}"/> of the transactions in the Portfolio.</returns>
        public IList<IHolding> CalculateHoldings(DateTime settlementDate)
        {
            var holdings = new List<IHolding>();
            foreach (var position in Positions)
            {
                holdings.AddRange(position.CalculateHoldings(settlementDate));
            }
            return holdings.OrderByDescending(h=>h.Tail).ToList();
        }

        /// <summary>
        ///   Gets an <see cref = "IList{T}" /> of positions held in this IPortfolio.
        /// </summary>
        public ICollection<IPosition> Positions
        {
            get { return _positions; }
        }

        /// <summary>
        ///   Retrieves the <see cref="IPosition"/> with Ticker <paramref name="ticker"/>.
        /// </summary>
        /// <param name="ticker">The Ticker symbol of the position to retrieve.</param>
        /// <returns>The <see cref="IPosition"/> with the requested Ticker. Returns null if no <see cref="IPosition"/> is found with the requested Ticker.</returns>
        public IPosition GetPosition(string ticker)
        {
            return Positions.Where(p => p.Ticker == ticker).FirstOrDefault();
        }

        #endregion

        #region Private Methods
        
        private void AddToPosition(IShareTransaction transaction)
        {
            var ticker = transaction.Ticker;
            var position = GetPosition(ticker);
            if (position == null)
            {
                position = PositionFactory.CreatePosition(ticker);
                Positions.Add(position);
            }
            position.AddTransaction(transaction);
        }

        #endregion
    }
}