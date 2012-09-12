using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a portfolio of investments.
    /// </summary>
    internal class PortfolioImpl : Portfolio
    {
        #region Private Members

        private readonly CashAccount _cashAccount = CashAccountFactory.ConstructCashAccount();
        private readonly IList<Position> _positions = new List<Position>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a Portfolio and assigns a ticker symbol to use as the Portfolio's <see cref="CashAccount"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="CashAccount"/>.</param>
        internal PortfolioImpl(string ticker)
        {
            CashTicker = ticker;
        }

        #endregion

        #region Implementation of ITimePeriod

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimePeriod.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimePeriod as of the given DateTime.</returns>
        public decimal this[DateTime dateTime]
        {
            get { return this.CalculateGrossProfit(dateTime); }
        }

        /// <summary>
        /// Gets the first DateTime in the ITimePeriod.
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
                //    ShareTransaction first = Positions.OrderBy(position => position.Head).First().Transactions.OrderBy(trans => trans.SettlementDate).First();

                //    earliest = first.SettlementDate;
                //}

                return earliest;
            }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimePeriod.
        /// </summary>
        public DateTime Tail
        {
            get
            {
                DateTime? latest = null;
                if (Positions.Count > 0)
                    latest = Positions.OrderBy(position => position.Tail).Last().Transactions.OrderBy(trans => trans.SettlementDate).Last().SettlementDate;
                var cashTransactions = _cashAccount.Transactions.Where(transaction => transaction.SettlementDate > (latest ?? DateTime.MinValue)).ToList();
                if (cashTransactions.Any())
                    latest = cashTransactions.Max(t=>t.SettlementDate);

                return latest ?? DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the <see cref="ITimePeriod.Resolution"/> of price data stored within the ITimePeriod.
        /// </summary>
        public Resolution Resolution
        {
            get { return Positions.Min(p => p.Resolution); }
        }

        /// <summary>
        /// Determines if the ITimePeriod has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimePeriod has a valid value for the given date.</returns>
        public bool HasValueInRange(DateTime settlementDate)
        {
            return settlementDate >= Head;
        }

        #endregion

        #region Implementation of Portfolio

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
        public string CashTicker { get; private set; }

        /// <summary>
        ///   Gets an enumeration of all <see cref = "ShareTransaction" />s in this Position.
        /// </summary>
        public IList<Transaction> Transactions
        {
            get
            {
                var list = new List<Transaction>();
                foreach (var p in Positions)
                {
                    list.AddRange(p.Transactions);
                }
                list.AddRange(_cashAccount.Transactions);

                return list.OrderBy(t => t.SettlementDate).ToList();
            }
        }

        /// <summary>
        ///   Adds an <see cref="Transaction"/> to this Portfolio.
        /// </summary>
        public void AddTransaction(Transaction transaction)
        {
            if (transaction is DividendReceipt)
            {
                    _cashAccount.Deposit((DividendReceipt)transaction);
            }
            else if (transaction is Deposit)
            {
                    Deposit((Deposit)transaction);
            }
            else if (transaction is Withdrawal)
            {
                    Withdraw((Withdrawal)transaction);
            }
            else if (transaction is DividendReinvestment)
            {
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
            }
            else if (transaction is Buy)
            {
                    var buy = ((Buy)transaction);
                    Withdraw(buy.SettlementDate, buy.TotalValue);
                    AddToPosition(buy);
            }
            else if (transaction is SellShort)
            {
                    var sellShort = ((SellShort)transaction);
                    Withdraw(sellShort.SettlementDate, sellShort.TotalValue);
                    AddToPosition(sellShort);
            }
            else if (transaction is Sell)
            {
                    var sell = ((Sell)transaction);
                    AddToPosition(sell);
                    Deposit(sell.SettlementDate, sell.TotalValue);
            }
            else if (transaction is BuyToCover)
            {
                    var buyToCover = ((BuyToCover)transaction);
                    AddToPosition(buyToCover);
                    Deposit(buyToCover.SettlementDate, buyToCover.TotalValue);
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
        /// <param name="withdrawal">The <see cref="Withdrawal"/> to withdraw.</param>
        public void Withdraw(Withdrawal withdrawal)
        {
            _cashAccount.Withdraw(withdrawal);
        }

        /// <summary>
        /// Adds historical transactions to the Portfolio.
        /// </summary>
        /// <param name="transactionHistory">The historical transactions to add.</param>
        public void AddTransactionHistory(TransactionHistory transactionHistory)
        {
            if (transactionHistory == null) throw new ArgumentNullException("transactionHistory", Strings.PortfolioImpl_AddTransactionHistory_Parameter_transactionHistory_cannot_be_null_);

            foreach (var transaction in transactionHistory.Transactions)
            {
                AddTransaction(transaction);
            }
        }

        /// <summary>
        /// Validates an <see cref="Transaction"/> without adding it to the Portfolio.
        /// </summary>
        /// <param name="transaction">The <see cref="ShareTransaction"/> to validate.</param>
        /// <returns></returns>
        public bool TransactionIsValid(Transaction transaction)
        {
            bool sufficientCash;
            if (transaction is CashTransaction)
            {
                    var cashTransaction = (CashTransaction) transaction;
                    return _cashAccount.TransactionIsValid(cashTransaction);
            }
            if (transaction is OpeningTransaction && transaction is LongTransaction)
            {
                    var buy = ((ShareTransaction)transaction);
                    sufficientCash = GetAvailableCash(buy.SettlementDate) >= buy.TotalValue;
                    return sufficientCash && GetPosition(buy.Ticker, false).TransactionIsValid(buy);
            }
            if (transaction is SellShort)
            {
                    var sellShort = ((SellShort)transaction);
                    return GetPosition(sellShort.Ticker, false).TransactionIsValid(sellShort);
            }
            if (transaction is Sell)
            {
                    var sell = ((ShareTransaction)transaction);
                    return GetPosition(sell.Ticker, false).TransactionIsValid(sell);
            }
            if (transaction is BuyToCover)
            {
                    var buyToCover = ((ShareTransaction)transaction);
                    sufficientCash = GetAvailableCash(buyToCover.SettlementDate) >= buyToCover.TotalValue;
                    return sufficientCash && GetPosition(buyToCover.Ticker, false).TransactionIsValid(buyToCover);
            }
                    // unknown order type
                    return false;
            }

        /// <summary>
        ///   Gets an <see cref = "IList{T}" /> of positions held in this Portfolio.
        /// </summary>
        public IList<Position> Positions
        {
            get { return _positions; }
        }

        /// <summary>
        ///   Retrieves the <see cref="Position"/> with Ticker <paramref name="ticker"/>.
        /// </summary>
        /// <param name="ticker">The Ticker symbol of the position to retrieve.</param>
        /// <returns>The <see cref="Position"/> with the requested Ticker. Returns null if no <see cref="Position"/> is found with the requested Ticker.</returns>
        public Position GetPosition(string ticker)
        {
            return GetPosition(ticker, true);
        }

        #endregion

        #region Private Methods

        private Position GetPosition(string ticker, bool nullAcceptable)
        {
            var firstOrDefault = Positions.FirstOrDefault(p => p.Ticker == ticker);
            return firstOrDefault == null && !nullAcceptable ? PositionFactory.CreatePosition(ticker) : firstOrDefault;
        }

        private void AddToPosition(ShareTransaction transaction)
        {
            var ticker = transaction.Ticker;
            var position = GetPosition(ticker, true);
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