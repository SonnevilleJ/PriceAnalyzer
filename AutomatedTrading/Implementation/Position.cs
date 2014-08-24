using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading.Implementation
{
    public interface IPosition : ISecurityBasket
    {
        string Ticker { get; }

        void AddTransaction(ShareTransaction shareTransaction);

        bool TransactionIsValid(ShareTransaction shareTransaction);
    }

    public class Position : IPosition
    {
        private string _ticker;
        private readonly ICollection<ShareTransaction> _transactions = new List<ShareTransaction>();
        private readonly ISecurityBasketCalculator _securityBasketCalculator;

        internal Position(string ticker)
        {
            Ticker = ticker;
            _securityBasketCalculator = new SecurityBasketCalculator();
        }

        public string Ticker
        {
            get
            {
                return _ticker;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("value", Strings.Position_OnTickerChanging_Ticker_must_not_be_null__empty__or_whitespace_);
                }
                _ticker = value;
            }
        }

        public decimal this[DateTime dateTime]
        {
            get { return _securityBasketCalculator.CalculateGrossProfit(this, dateTime); }
        }

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

        public IList<ITransaction> Transactions
        {
            get { return new List<ITransaction>(_transactions); }
        }

        public void AddTransaction(ShareTransaction shareTransaction)
        {
            // verify shareTransaction is apporpriate for this IPosition.
            Validate(shareTransaction);

            _transactions.Add(shareTransaction);
        }

        public bool TransactionIsValid(ShareTransaction shareTransaction)
        {
            try
            {
                Validate(shareTransaction);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        private void Validate(ShareTransaction shareTransaction)
        {
            // Validate OrderType
            if (shareTransaction.IsOpeningTransaction())
            {
                    // new holdings are OK
            }
            else if (shareTransaction.IsClosingTransaction())
            {
                    var date = shareTransaction.SettlementDate;
                    var heldShares = _securityBasketCalculator.GetHeldShares(_transactions, date);
                    if (shareTransaction.Shares > heldShares)
                    {
                        throw new InvalidOperationException(
                            String.Format(CultureInfo.CurrentCulture,
                                          Strings.PositionImpl_Validate_This_Transaction_requires__0__shares__but_only__1__shares_are_held_by_this_Position_as_of__2__,
                                          shareTransaction.Shares, heldShares, date));
                    }
            }
        }
    }
}