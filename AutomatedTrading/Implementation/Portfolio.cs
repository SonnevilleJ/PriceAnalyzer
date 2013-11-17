﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Extensions;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading.Implementation
{
    /// <summary>
    /// Represents a portfolio of investments.
    /// </summary>
    public class Portfolio : ISecurityBasket
    {
        private readonly ICashAccountFactory _cashAccountFactory;
        private readonly ICashAccount _cashAccount;
        private readonly IList<Position> _positions;
        private readonly IPositionFactory _positionFactory;

        /// <summary>
        /// Constructs a Portfolio and assigns a ticker symbol to use as the Portfolio's <see cref="ICashAccount"/>.
        /// </summary>
        /// <param name="ticker">The ticker symbol which is used as the <see cref="ICashAccount"/>.</param>
        internal Portfolio(string ticker)
        {
            _cashAccountFactory = new CashAccountFactory();
            _cashAccount = _cashAccountFactory.ConstructCashAccount();
            _positions = new List<Position>();
            CashTicker = ticker;
            _positionFactory = new PositionFactory();
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the Portfolio.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the Portfolio as of the given DateTime.</returns>
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
        ///   Adds an <see cref="Transaction"/> to this Portfolio.
        /// </summary>
        public void AddTransaction(Transaction transaction)
        {
            transaction.ApplyToPortfolio(this);
        }

        /// <summary>
        /// Deposits dividends to this Portfolio.
        /// </summary>
        /// <param name="dividendReceipt">The <see cref="DividendReceipt"/> to deposit.</param>
        public void Deposit(DividendReceipt dividendReceipt)
        {
            _cashAccount.Deposit(dividendReceipt);
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
        /// <param name="deposit">The <see cref="PriceTools.Implementation.Deposit"/> to deposit.</param>
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
        /// Validates an <see cref="Transaction"/> without adding it to the Portfolio.
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
            var buy = transaction as Buy;
            var sellShort = transaction as SellShort;
            var sell = transaction as Sell;
            var buyToCover = transaction as BuyToCover;
            if (buy != null)
            {
                sufficientCash = GetAvailableCash(buy.SettlementDate) >= buy.TotalValue;
                return sufficientCash && GetPosition(buy.Ticker, false).TransactionIsValid(buy);
            }
            if (sellShort != null)
            {
                return GetPosition(sellShort.Ticker, false).TransactionIsValid(sellShort);
            }
            if (sell != null)
            {
                return GetPosition(sell.Ticker, false).TransactionIsValid(sell);
            }
            if (buyToCover != null)
            {
                sufficientCash = GetAvailableCash(buyToCover.SettlementDate) >= buyToCover.TotalValue;
                return sufficientCash &&
                       GetPosition(buyToCover.Ticker, false).TransactionIsValid(buyToCover);
            }

            // unknown order type
            return false;
        }

        /// <summary>
        ///   Gets an <see cref = "IList{T}" /> of positions held in this Portfolio.
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

        private Position GetPosition(string ticker, bool nullAcceptable)
        {
            var firstOrDefault = Positions.FirstOrDefault(p => p.Ticker == ticker);
            return firstOrDefault == null && !nullAcceptable ? _positionFactory.ConstructPosition(ticker) : firstOrDefault;
        }

        public void AddToPosition(ShareTransaction transaction)
        {
            var ticker = transaction.Ticker;
            var position = GetPosition(ticker, true);
            if (position == null)
            {
                position = _positionFactory.ConstructPosition(ticker);
                _positions.Add(position);
            }
            position.AddTransaction(transaction);
        }
    }
}