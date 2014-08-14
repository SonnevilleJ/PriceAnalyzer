using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading.Implementation
{
    public interface IPosition : ISecurityBasket
    {
        /// <summary>
        ///   Gets the ticker symbol held by this IPosition.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        /// Adds an ShareTransaction to the IPosition.
        /// </summary>
        /// <param name="shareTransaction"></param>
        void AddTransaction(ShareTransaction shareTransaction);

        /// <summary>
        /// Validates a transaction without adding it to the IPosition.
        /// </summary>
        /// <param name="shareTransaction"></param>
        bool TransactionIsValid(ShareTransaction shareTransaction);
    }

    /// <summary>
    ///   A trade made for a financial security. A Position is comprised of an opening shareTransaction, and optionally, a closing shareTransaction.
    /// </summary>
    public class Position : IPosition
    {
        private string _ticker;
        private readonly ICollection<ShareTransaction> _transactions = new List<ShareTransaction>();
        private readonly ISecurityBasketCalculator _securityBasketCalculator;

        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name = "ticker">The ticker symbol that this Position will hold. All transactions will use this ticker symbol.</param>
        internal Position(string ticker)
        {
            Ticker = ticker;
            _securityBasketCalculator = new SecurityBasketCalculator();
        }

        /// <summary>
        ///   Gets the ticker symbol held by this IPosition.
        /// </summary>
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

        /// <summary>
        ///   Gets the total value of the Position, including commissions.
        /// </summary>
        /// <param name = "dateTime">The <see cref = "DateTime" /> to use.</param>
        public decimal this[DateTime dateTime]
        {
            get { return _securityBasketCalculator.CalculateGrossProfit(this, dateTime); }
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
        ///   Gets an enumeration of all <see cref = "ShareTransaction" />s in this IPosition.
        /// </summary>
        public IList<ITransaction> Transactions
        {
            get { return new List<ITransaction>(_transactions); }
        }

        /// <summary>
        /// Adds an ShareTransaction to the IPosition.
        /// </summary>
        /// <param name="shareTransaction"></param>
        public void AddTransaction(ShareTransaction shareTransaction)
        {
            // verify shareTransaction is apporpriate for this IPosition.
            Validate(shareTransaction);

            _transactions.Add(shareTransaction);
        }

        /// <summary>
        /// Validates a transaction without adding it to the IPosition.
        /// </summary>
        /// <param name="shareTransaction"></param>
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