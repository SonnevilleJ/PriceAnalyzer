using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.AutomatedTrading.Implementation
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

        #region Implementation of IPortfolio

        /// <summary>
        /// Gets a value stored at a given DateTime index of the IPortfolio.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the IPortfolio as of the given DateTime.</returns>
        public decimal this[DateTime dateTime]
        {
            get { return this.CalculateGrossProfit(dateTime); }
        }

        /// <summary>
        /// Gets the first DateTime for which a value exists.
        /// </summary>
        public DateTime Head
        {
            get
            {
                if (Transactions.Any())
                {
                    return Transactions.Min(t => t.SettlementDate);
                }
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the last DateTime for which a value exists.
        /// </summary>
        public DateTime Tail
        {
            get
            {
                if (Transactions.Any())
                {
                    return Transactions.Max(t => t.SettlementDate);
                }
                return DateTime.Now;
            }
        }

        #endregion

        #region Implementation of Portfolio

        /// <summary>
        ///   Gets the amount of uninvested cash in this IPortfolio.
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
        ///   Gets an enumeration of all <see cref = "ShareTransaction" />s in this Position.
        /// </summary>
        public IEnumerable<Transaction> Transactions
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
        ///   Adds an <see cref="Transaction"/> to this IPortfolio.
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
        /// Deposits cash to this IPortfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the deposit.</param>
        /// <param name="cashAmount">The amount of cash deposited.</param>
        public void Deposit(DateTime settlementDate, decimal cashAmount)
        {
            _cashAccount.Deposit(settlementDate, cashAmount);
        }

        /// <summary>
        /// Deposits cash to this IPortfolio.
        /// </summary>
        /// <param name="deposit">The <see cref="PriceTools.Deposit"/> to deposit.</param>
        public void Deposit(Deposit deposit)
        {
            _cashAccount.Deposit(deposit);
        }

        /// <summary>
        /// Withdraws cash from this IPortfolio. AvailableCash must be greater than or equal to the withdrawn amount.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the withdrawal.</param>
        /// <param name="cashAmount">The amount of cash withdrawn.</param>
        public void Withdraw(DateTime settlementDate, decimal cashAmount)
        {
            _cashAccount.Withdraw(settlementDate, cashAmount);
        }

        /// <summary>
        /// Withdraws cash from this IPortfolio. Available cash must be greater than or equal to the withdrawn amount.
        /// </summary>
        /// <param name="withdrawal">The <see cref="Withdrawal"/> to withdraw.</param>
        public void Withdraw(Withdrawal withdrawal)
        {
            _cashAccount.Withdraw(withdrawal);
        }

        /// <summary>
        /// Adds historical transactions to the IPortfolio.
        /// </summary>
        /// <param name="transactions">The historical transactions to add.</param>
        public void AddTransactions(IEnumerable<Transaction> transactions)
        {
            if (transactions == null) throw new ArgumentNullException("transactions", Strings.PortfolioImpl_AddTransactionHistory_Parameter_transactionHistory_cannot_be_null_);

            foreach (var transaction in transactions)
            {
                AddTransaction(transaction);
            }
        }

        /// <summary>
        /// Validates an <see cref="Transaction"/> without adding it to the IPortfolio.
        /// </summary>
        /// <param name="transaction">The <see cref="ShareTransaction"/> to validate.</param>
        /// <returns></returns>
        public bool TransactionIsValid(Transaction transaction)
        {
            var cashTransaction = transaction as CashTransaction;
            if (cashTransaction != null)
            {
                return _cashAccount.TransactionIsValid(cashTransaction);
            }

            bool sufficientCash;
            if (transaction is OpeningTransaction && transaction is LongTransaction)
            {
                var buy = ((ShareTransaction) transaction);
                sufficientCash = GetAvailableCash(buy.SettlementDate) >= buy.TotalValue;
                return sufficientCash && ((PositionImpl) GetPosition(buy.Ticker, false)).TransactionIsValid(buy);
            }
            if (transaction is OpeningTransaction && transaction is ShortTransaction)
            {
                var sellShort = ((SellShort) transaction);
                return ((PositionImpl) GetPosition(sellShort.Ticker, false)).TransactionIsValid(sellShort);
            }
            if (transaction is ClosingTransaction && transaction is LongTransaction)
            {
                var sell = ((ShareTransaction) transaction);
                return ((PositionImpl) GetPosition(sell.Ticker, false)).TransactionIsValid(sell);
            }
            if (transaction is ClosingTransaction && transaction is ShortTransaction)
            {
                var buyToCover = ((ShareTransaction) transaction);
                sufficientCash = GetAvailableCash(buyToCover.SettlementDate) >= buyToCover.TotalValue;
                return sufficientCash &&
                       ((PositionImpl) GetPosition(buyToCover.Ticker, false)).TransactionIsValid(buyToCover);
            }

            // unknown order type
            return false;
        }

        /// <summary>
        ///   Gets an <see cref = "IList{T}" /> of positions held in this IPortfolio.
        /// </summary>
        public IEnumerable<Position> Positions
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
            return firstOrDefault == null && !nullAcceptable ? PositionFactory.ConstructPosition(ticker) : firstOrDefault;
        }

        private void AddToPosition(ShareTransaction transaction)
        {
            var ticker = transaction.Ticker;
            var position = GetPosition(ticker, true);
            if (position == null)
            {
                position = PositionFactory.ConstructPosition(ticker);
                _positions.Add(position);
            }
            ((PositionImpl) position).AddTransaction(transaction);
        }

        #endregion
    }
}