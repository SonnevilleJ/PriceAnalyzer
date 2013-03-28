using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sonneville.PriceTools.AutomatedTrading.Implementation
{
    /// <summary>
    ///   A trade made for a financial security. A Position is comprised of an opening shareTransaction, and optionally, a closing shareTransaction.
    /// </summary>
    internal class PositionImpl : IPosition
    {
        #region Private Members

        private string _ticker;
        private readonly ICollection<IShareTransaction> _transactions = new List<IShareTransaction>();

        #endregion

        #region Constructors

        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name = "ticker">The ticker symbol that this Position will hold. All transactions will use this ticker symbol.</param>
        internal PositionImpl(string ticker)
        {
            Ticker = ticker;
        }

        #endregion

        #region Position Members

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
        ///   Gets an enumeration of all <see cref = "IShareTransaction" />s in this IPosition.
        /// </summary>
        public IEnumerable<ITransaction> Transactions
        {
            get { return new List<ITransaction>(_transactions); }
        }

        /// <summary>
        /// Adds an ShareTransaction to the IPosition.
        /// </summary>
        /// <param name="shareTransaction"></param>
        public void AddTransaction(IShareTransaction shareTransaction)
        {
            // verify shareTransaction is apporpriate for this IPosition.
            Validate(shareTransaction);

            _transactions.Add(shareTransaction);
        }

        /// <summary>
        /// Validates a transaction without adding it to the IPosition.
        /// </summary>
        /// <param name="shareTransaction"></param>
        public bool TransactionIsValid(IShareTransaction shareTransaction)
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

        #endregion

        #region Helper Methods

        private void Validate(IShareTransaction shareTransaction)
        {
            // Validate OrderType
            if (shareTransaction is IOpeningTransaction)
            {
                    // new holdings are OK
            }
            else if (shareTransaction is IClosingTransaction)
            {
                    var date = shareTransaction.SettlementDate;
                    var heldShares = _transactions.GetHeldShares(date);
                    if (shareTransaction.Shares > heldShares)
                    {
                        throw new InvalidOperationException(
                            String.Format(CultureInfo.CurrentCulture,
                                          Strings.PositionImpl_Validate_This_Transaction_requires__0__shares__but_only__1__shares_are_held_by_this_Position_as_of__2__,
                                          shareTransaction.Shares, heldShares, date));
                    }
            }
        }

        #endregion
    }
}