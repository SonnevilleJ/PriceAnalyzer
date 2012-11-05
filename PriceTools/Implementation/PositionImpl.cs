using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    ///   A trade made for a financial security. A Position is comprised of an opening shareTransaction, and optionally, a closing shareTransaction.
    /// </summary>
    internal class PositionImpl : Position
    {
        #region Private Members

        private string _ticker;
        private readonly ICollection<ShareTransaction> _transactions = new List<ShareTransaction>();

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
        ///   Buys shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        public void Buy(DateTime settlementDate, decimal shares, decimal price, decimal commission)
        {
            AddTransaction(shares, OrderType.Buy, settlementDate, price, commission);
        }

        /// <summary>
        ///   Buys shares of the ticker held by this Position to cover a previous ShortSell.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction. Shares cannot exceed currently shorted shares.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        public void BuyToCover(DateTime settlementDate, decimal shares, decimal price, decimal commission)
        {
            AddTransaction(shares, OrderType.BuyToCover, settlementDate, price, commission);
        }

        /// <summary>
        ///   Sells shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction. Shares connot exceed currently held shares.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        public void Sell(DateTime settlementDate, decimal shares, decimal price, decimal commission)
        {
            AddTransaction(shares, OrderType.Sell, settlementDate, price, commission);
        }

        /// <summary>
        ///   Sell short shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        public void SellShort(DateTime settlementDate, decimal shares, decimal price, decimal commission)
        {
            AddTransaction(shares, OrderType.SellShort, settlementDate, price, commission);
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
        ///   Gets an enumeration of all <see cref = "ShareTransaction" />s in this IPosition.
        /// </summary>
        public IEnumerable<Transaction> Transactions
        {
            get { return new List<Transaction>(_transactions); }
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

        #endregion

        #region Helper Methods

        private void AddTransaction(decimal shares, OrderType type, DateTime settlementDate, decimal price, decimal commission)
        {
            var shareTransaction = TransactionFactory.ConstructShareTransaction(type, Ticker, settlementDate, shares, price, commission);
            AddTransaction(shareTransaction);
        }

        private void Validate(ShareTransaction shareTransaction)
        {
            // Validate OrderType
            if (shareTransaction is OpeningTransaction)
            {
                    // new holdings are OK
            }
            else if (shareTransaction is ClosingTransaction)
            {
                    var date = shareTransaction.SettlementDate;
                    var heldShares = _transactions.GetHeldShares(date);
                    if (shareTransaction.Shares > heldShares)
                    {
                        throw new InvalidOperationException(
                            String.Format(CultureInfo.CurrentCulture,
                                          "This Transaction requires {0} shares, but only {1} shares are held by this Position as of {2}.",
                                          shareTransaction.Shares, heldShares, date));
                    }
            }
        }

        #endregion
    }
}