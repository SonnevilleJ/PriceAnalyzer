using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a portfolio of investments.
    /// </summary>
    public partial class Portfolio : IPortfolio
    {
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
        public decimal? this[DateTime index]
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
                DateTime latest = DateTime.Now;
                if (Positions.Count > 0)
                {
                    latest = Positions.OrderBy(position => position.Head).Last().Transactions.OrderBy(trans => trans.SettlementDate).Last().SettlementDate;
                }
                if (CashAccount.Transactions.Count > 0)
                {
                    latest = ((ITransaction) CashAccount.Transactions.OrderBy(transaction => transaction.SettlementDate).Last()).SettlementDate;
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
        ///   Gets the raw rate of return for this Portfolio, not accounting for commissions.
        /// </summary>
        public decimal? GetRawReturn(DateTime settlementDate)
        {
            decimal proceeds = GetProceeds(settlementDate);
            decimal costs = GetCost(settlementDate);
            decimal profit = proceeds - costs;
            return proceeds != 0
                ? (profit / costs)
                : (decimal?) null;
        }

        /// <summary>
        ///   Gets the total rate of return for this Portfolio, after commissions.
        /// </summary>
        public decimal? GetTotalReturn(DateTime settlementDate)
        {
            decimal proceeds = GetProceeds(settlementDate);
            decimal costs = GetCost(settlementDate);
            decimal commissions = GetCommissions(settlementDate);
            decimal profit = proceeds - costs - commissions;
            return proceeds != 0
                       ? (profit/costs)
                       : (decimal?) null;
        }

        /// <summary>
        ///   Gets the total rate of return on an annual basis for this Portfolio.
        /// </summary>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public decimal? GetAverageAnnualReturn(DateTime settlementDate)
        {
            decimal time = ((Tail - Head).Days / 365.0m);
            decimal? totalReturn = GetTotalReturn(settlementDate);

            return totalReturn/(time);
        }

        /// <summary>
        ///   Gets the gross investment of this Portfolio, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases.</returns>
        public decimal GetCost(DateTime settlementDate)
        {
            return Positions.Sum(p => p.GetCost(settlementDate));
        }

        /// <summary>
        ///   Gets the gross proceeds of this Portfolio, ignoring all totalCosts and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales.</returns>
        public decimal GetProceeds(DateTime settlementDate)
        {
            return Positions.Sum(p => p.GetProceeds(settlementDate));
        }

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "IShareTransaction" />s.</returns>
        public decimal GetCommissions(DateTime settlementDate)
        {
            return Positions.Sum(p => p.GetCommissions(settlementDate));
        }

        /// <summary>
        ///   Gets an enumeration of all <see cref = "IShareTransaction" />s in this IPosition.
        /// </summary>
        public IList<ITransaction> Transactions
        {
            get
            {
                List<ITransaction> list = new List<ITransaction>();
                foreach (Position p in Positions)
                {
                    list.AddRange(p.Transactions);
                }
                list.AddRange(CashAccount.Transactions);

                list.OrderBy(t => t.SettlementDate);
                return list;
            }
        }

        /// <summary>
        ///   Gets the value of all shares held the IPortfolio as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the shares held in the IPortfolio as of the given date.</returns>
        public decimal GetInvestedValue(DateTime settlementDate)
        {
            return Positions.Sum(p => p.GetInvestedValue(settlementDate));
        }

        /// <summary>
        ///   Adds an <see cref="ITransaction"/> to this Portfolio.
        /// </summary>
        public void AddTransaction(ITransaction transaction)
        {
            switch (transaction.OrderType)
            {
                case OrderType.DividendReceipt:
                    Deposit((DividendReceipt) transaction);
                    break;
                case OrderType.Deposit:
                    Deposit((Deposit) transaction);
                    break;
                case OrderType.Withdrawal:
                    Withdraw((Withdrawal) transaction);
                    break;
                case OrderType.DividendReinvestment:
                    DividendReinvestment dr = ((DividendReinvestment) transaction);
                    if (dr.Ticker == CashTicker)
                    {
                        // DividendReceipt already deposited into cash account,
                        // so no need to "buy" the CashTicker. Do nothing.
                    }
                    else
                    {
                        Withdraw(dr.SettlementDate, dr.TotalValue);
                        GetPosition(dr.Ticker).AddTransaction(dr);
                    }
                    break;
                case OrderType.Buy:
                    Buy buy = ((Buy) transaction);
                    Withdraw(buy.SettlementDate, buy.TotalValue);
                    GetPosition(buy.Ticker).AddTransaction(buy);
                    break;
                case OrderType.SellShort:
                    SellShort sellShort = ((SellShort) transaction);
                    Withdraw(sellShort.SettlementDate, sellShort.TotalValue);
                    GetPosition(sellShort.Ticker).AddTransaction(sellShort);
                    break;
                case OrderType.Sell:
                    Sell sell = ((Sell) transaction);
                    GetPosition(sell.Ticker).AddTransaction(sell);
                    Deposit(sell.SettlementDate, sell.TotalValue);
                    break;
                case OrderType.BuyToCover:
                    BuyToCover buyToCover = ((BuyToCover) transaction);
                    GetPosition(buyToCover.Ticker).AddTransaction(buyToCover);
                    Deposit(buyToCover.SettlementDate, buyToCover.TotalValue);
                    break;
            }
        }

        private void Deposit(DividendReceipt dividendReceipt)
        {
            CashAccount.Deposit(dividendReceipt);
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
        /// Deposits cash to this Portfolio.
        /// </summary>
        public void Deposit(Deposit deposit)
        {
            CashAccount.Deposit(deposit);
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
        /// Withdraws cash from this Portfolio. Available cash must be greater than or equal to the withdrawn amount.
        /// </summary>
        public void Withdraw(Withdrawal withdrawal)
        {
            CashAccount.Withdraw(withdrawal);
        }

        /// <summary>
        /// Adds transaction history from a CSV file to the Portfolio.
        /// </summary>
        /// <param name="csvFile">The CSV file containing the transactions to add.</param>
        public void AddTransactionHistory(TransactionHistoryCsvFile csvFile)
        {
            foreach (ITransaction transaction in csvFile.Transactions)
            {
                AddTransaction(transaction);
            }
        }

        #endregion

        public decimal Value { get { return GetValue(DateTime.Now); } }

        #region Private Methods

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
    }
}