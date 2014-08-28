using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Extensions;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading.Implementation
{
    public interface IPortfolio : ISecurityBasket
    {
        IPriceSeries CashPriceSeries { get; }

        string CashTicker { get; }

        IEnumerable<IPosition> Positions { get; }

        decimal GetAvailableCash(DateTime settlementDate);

        void AddTransaction(ITransaction transaction);

        void Deposit(DividendReceipt dividendReceipt);

        void Deposit(DateTime settlementDate, decimal cashAmount);

        void Deposit(Deposit deposit);

        void Withdraw(DateTime settlementDate, decimal cashAmount);

        void Withdraw(Withdrawal withdrawal);

        bool TransactionIsValid(ITransaction transaction);

        IPosition GetPosition(string ticker);

        void AddToPosition(ShareTransaction transaction);
    }

    public class Portfolio : IPortfolio
    {
        private readonly ICashAccount _cashAccount;
        private readonly IList<IPosition> _positions;
        private readonly IPositionFactory _positionFactory;
        private readonly ISecurityBasketCalculator _securityBasketCalculator;

        internal Portfolio(string ticker)
        {
            _cashAccount = new CashAccountFactory().ConstructCashAccount();
            _positions = new List<IPosition>();
            CashTicker = ticker;
            _positionFactory = new PositionFactory();
            _securityBasketCalculator = new SecurityBasketCalculator();
        }

        public decimal this[DateTime dateTime]
        {
            get { return _securityBasketCalculator.CalculateGrossProfit(this, dateTime); }
        }

        public IPriceSeries CashPriceSeries
        {
            get
            {
                var cashPriceSeries = new PriceSeriesFactory().ConstructPriceSeries(CashTicker);
                for (var date = Head.PreviousPeriodOpen(Resolution.Days); date <= Tail; date = date.NextPeriodOpen(Resolution.Days))
                {
                    var heldShares = _securityBasketCalculator.GetHeldShares(_cashAccount.Transactions, date);
                    cashPriceSeries.AddPriceData(new PricePeriod(date, date.CurrentPeriodClose(Resolution.Days), heldShares*1m));
                }
                return cashPriceSeries;
            }
        }

        public DateTime Head
        {
            get
            {
                return Transactions.Any()
                    ? Transactions.Min(t => t.SettlementDate)
                    : DateTime.Now;
            }
        }

        public DateTime Tail
        {
            get
            {
                var transactions = Transactions;
                return transactions.Any()
                    ? transactions.Max(t => t.SettlementDate)
                    : DateTime.Now;
            }
        }

        public decimal GetAvailableCash(DateTime settlementDate)
        {
            return _cashAccount.GetCashBalance(settlementDate);
        }

        public string CashTicker { get; private set; }

        public IList<ITransaction> Transactions
        {
            get
            {
                return Positions.SelectMany(x => x.Transactions)
                    .Concat(_cashAccount.Transactions)
                    .OrderBy(x => x.SettlementDate)
                    .ToList();
            }
        }

        public void AddTransaction(ITransaction transaction)
        {
            transaction.ApplyToPortfolio(this);
        }

        public void Deposit(DividendReceipt dividendReceipt)
        {
            _cashAccount.Deposit(dividendReceipt);
        }

        public void Deposit(DateTime settlementDate, decimal cashAmount)
        {
            _cashAccount.Deposit(settlementDate, cashAmount);
        }

        public void Deposit(Deposit deposit)
        {
            _cashAccount.Deposit(deposit);
        }

        public void Withdraw(DateTime settlementDate, decimal cashAmount)
        {
            _cashAccount.Withdraw(settlementDate, cashAmount);
        }

        public void Withdraw(Withdrawal withdrawal)
        {
            _cashAccount.Withdraw(withdrawal);
        }

        public bool TransactionIsValid(ITransaction transaction)
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

        public IEnumerable<IPosition> Positions
        {
            get { return _positions; }
        }

        public IPosition GetPosition(string ticker)
        {
            return GetPosition(ticker, true);
        }

        private IPosition GetPosition(string ticker, bool nullAcceptable)
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