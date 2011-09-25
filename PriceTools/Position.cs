using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A trade made for a financial security. A Position is comprised of an opening shareTransaction, and optionally, a closing shareTransaction.
    /// </summary>
    public partial class Position : IPosition
    {
        #region Private Members

        private IPriceSeries _priceSeries;

        #endregion

        #region Constructors

        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        private Position()
        {
        }

        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name = "ticker">The ticker symbol that this portfolio will hold. All transactions will use this ticker symbol.</param>
        internal Position(string ticker)
        {
            Ticker = ticker;
        }

        #endregion

        #region IPosition Members

        /// <summary>
        ///   Gets the average cost of all held shares in this Position as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The average cost of all shares held at <paramref name = "settlementDate" />.</returns>
        public decimal GetAverageCost(DateTime settlementDate)
        {
            List<ShareTransaction> transactions = EFTransactions
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .OrderBy(transaction => transaction.SettlementDate).ToList();
            int count = transactions.Count();

            decimal totalCost = 0.00m;
            double shares = 0.0;

            for (int i = 0; i < count; i++)
            {
                switch (transactions[i].OrderType)
                {
                    case OrderType.Buy:
                    case OrderType.SellShort:
                    case OrderType.DividendReinvestment:
                        totalCost += (transactions[i].Price*(decimal) transactions[i].Shares);
                        shares += transactions[i].Shares;
                        break;
                    case OrderType.Sell:
                    case OrderType.BuyToCover:
                        totalCost -= ((totalCost/(decimal) shares)*(decimal) transactions[i].Shares);
                        shares -= transactions[i].Shares;
                        break;
                }
            }

            return totalCost/(decimal) shares;
        }

        /// <summary>
        ///   Buys shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        public void Buy(DateTime settlementDate, double shares, decimal price, decimal commission)
        {
            AddTransaction(shares, OrderType.Buy, settlementDate, price, commission);
        }

        /// <summary>
        ///   Buys shares of the ticker held by this IPosition to cover a previous ShortSell.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction. Shares cannot exceed currently shorted shares.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        public void BuyToCover(DateTime settlementDate, double shares, decimal price, decimal commission)
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
        public void Sell(DateTime settlementDate, double shares, decimal price, decimal commission)
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
        public void SellShort(DateTime settlementDate, double shares, decimal price, decimal commission)
        {
            AddTransaction(shares, OrderType.SellShort, settlementDate, price, commission);
        }

        /// <summary>
        ///   Gets the total value of the Position, including commissions.
        /// </summary>
        /// <param name = "index">The <see cref = "DateTime" /> to use.</param>
        public decimal this[DateTime index]
        {
            get { return GetValue(index); }
        }

        /// <summary>
        ///   Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public DateTime Head
        {
            get { return First.SettlementDate; }
        }

        /// <summary>
        ///   Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public DateTime Tail
        {
            get { return Last.SettlementDate; }
        }

        /// <summary>
        ///   Determines if the ITimeSeries has a valid value for a given date.
        /// </summary>
        /// <param name = "settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimeSeries has a valid value for the given date.</returns>
        public bool HasValueInRange(DateTime settlementDate)
        {
            DateTime end = Tail;
            if (GetValue(settlementDate) != 0)
            {
                end = settlementDate;
            }
            return settlementDate >= Head && settlementDate <= end;
        }

        /// <summary>
        ///   Gets the value of any shares held the Portfolio as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the shares held in the Portfolio as of the given date.</returns>
        public decimal GetInvestedValue(DateTime settlementDate)
        {
            var heldShares = (decimal) GetHeldShares(settlementDate);
            return heldShares > 0
                       ? heldShares*(PriceSeries[settlementDate])
                       : 0;
        }

        /// <summary>
        ///   Gets an enumeration of all <see cref = "ShareTransaction" />s in this IMeasurableSecurityBasket.
        /// </summary>
        public IList<ITransaction> Transactions
        {
            get { return new List<ITransaction>(EFTransactions); }
        }

        /// <summary>
        ///   Gets the value of the IPortfolio as of a given date, excluding all commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the IPortfolio as of the given date.</returns>
        public decimal GetValue(DateTime settlementDate)
        {
            decimal proceeds = GetProceeds(settlementDate); // positive proceeds = gain, negative proceeds = loss
            decimal totalCosts = GetCost(settlementDate);
                // positive totalCosts = revenue, negative totalCosts = expense

            double heldShares = GetHeldShares(settlementDate);
            double totalShares = GetOpenedShares(settlementDate);

            decimal costOfUnsoldShares = 0.00m;
            if (totalShares != 0)
            {
                costOfUnsoldShares = totalCosts*(decimal) (heldShares/totalShares);
            }
            return proceeds + costOfUnsoldShares;
        }

        /// <summary>
        ///   Gets the gross investment of this Position, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases as a negative number.</returns>
        public decimal GetCost(DateTime settlementDate)
        {
            return AdditiveTransactions
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .Sum(transaction => transaction.Price*(decimal) transaction.Shares);
        }

        /// <summary>
        ///   Gets the gross proceeds of this Position, ignoring all totalCosts and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales as a positive number.</returns>
        public decimal GetProceeds(DateTime settlementDate)
        {
            return -1*SubtractiveTransactions
                          .Where(transaction => transaction.SettlementDate <= settlementDate)
                          .Sum(transaction => transaction.Price*(decimal) transaction.Shares);
        }

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "IShareTransaction" />s as a negative number.</returns>
        public decimal GetCommissions(DateTime settlementDate)
        {
            return
                EFTransactions.Where(transaction => transaction.SettlementDate <= settlementDate).Sum(
                    transaction => transaction.Commission);
        }

        /// <summary>
        ///   Gets the raw rate of return for this Position, not accounting for commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        public decimal? GetRawReturn(DateTime settlementDate)
        {
            return GetClosedShares(settlementDate) > 0
                       ? (decimal?) ((GetValue(settlementDate)/GetCost(settlementDate)) - 1)
                       : null;
        }

        /// <summary>
        ///   Gets the total rate of return for this Position, after commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
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
        ///   Gets the total rate of return on an annual basis for this Position.
        /// </summary>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public decimal? GetAverageAnnualReturn(DateTime settlementDate)
        {
            decimal? totalReturn = GetTotalReturn(settlementDate);
            decimal time = (Duration.Days/365.0m);
            return totalReturn != null
                       ? totalReturn/time
                       : null;
        }

        /// <summary>
        /// Adds an IShareTransaction to the Position.
        /// </summary>
        /// <param name="shareTransaction"></param>
        public void AddTransaction(IShareTransaction shareTransaction)
        {
            // verify shareTransaction is apporpriate for this Position.
            Validate(shareTransaction);

            EFTransactions.Add((ShareTransaction)shareTransaction);
        }

        #endregion

        #region Helper Properties

        private TimeSpan Duration
        {
            get { return Last.SettlementDate - First.SettlementDate; }
        }

        private static IEnumerable<OrderType> Additive
        {
            get { return new[] { OrderType.Buy, OrderType.SellShort, OrderType.DividendReinvestment }; }
        }

        private static IEnumerable<OrderType> Subtractive
        {
            get { return new[] { OrderType.Sell, OrderType.BuyToCover }; }
        }

        /// <summary>
        ///   Gets a list of <see cref = "IShareTransaction" />s which added to this Position.
        ///   Typically <see cref = "OrderType.Buy" /> or <see cref = "OrderType.SellShort" /> <see cref = "IShareTransaction" />s.
        /// </summary>
        private IEnumerable<IShareTransaction> AdditiveTransactions
        {
            get { return EFTransactions.Where(t => Additive.Contains(t.OrderType)); }
        }

        /// <summary>
        ///   Gets a list of <see cref = "IShareTransaction" />s which subtracted from this Position.
        ///   Typically <see cref = "OrderType.Sell" /> or <see cref = "OrderType.BuyToCover" /> <see cref = "IShareTransaction" />s.
        /// </summary>
        private IEnumerable<IShareTransaction> SubtractiveTransactions
        {
            get { return EFTransactions.Where(t => Subtractive.Contains(t.OrderType)); }
        }

        private IShareTransaction Last
        {
            get { return EFTransactions.OrderBy(t => t.SettlementDate).Last(); }
        }

        private IShareTransaction First
        {
            get { return EFTransactions.OrderBy(t => t.SettlementDate).First(); }
        }

        private IPriceSeries PriceSeries
        {
            get { return _priceSeries ?? (_priceSeries = PriceSeriesFactory.CreatePriceSeries(Ticker)); }
        }

        #endregion

        #region Helper Methods

        private void AddTransaction(double shares, OrderType type, DateTime settlementDate, decimal price, decimal commission)
        {
            ShareTransaction shareTransaction = TransactionFactory.CreateShareTransaction(settlementDate, type, Ticker, price,
                                                                                     shares, commission);
            AddTransaction(shareTransaction);
        }

        private void Validate(IShareTransaction shareTransaction)
        {
            // Validate OrderType
            switch (shareTransaction.OrderType)
            {
                case OrderType.Buy:
                case OrderType.SellShort:
                    // new holdings are OK
                    break;
                case OrderType.BuyToCover:
                case OrderType.Sell:
                    // Verify that closed holdings do not exceed available shares at the time of the shareTransaction.
                    DateTime date = shareTransaction.SettlementDate.Subtract(new TimeSpan(0, 0, 0, 1));
                    double heldShares = GetHeldShares(date);
                    if (shareTransaction.Shares > heldShares)
                    {
                        throw new InvalidOperationException(
                            String.Format(CultureInfo.CurrentCulture,
                                          "This Transaction requires {0} shares, but only {1} shares are held by this Position as of {2}.",
                                          shareTransaction.Shares, heldShares, date));
                    }
                    break;
            }
        }

        /// <summary>
        ///   Gets the net shares held at a given date.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        private double GetHeldShares(DateTime date)
        {
            return GetOpenedShares(date) - GetClosedShares(date);
        }

        /// <summary>
        ///   Gets the cumulative number of shares that have ever been owned before a given date.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        private double GetOpenedShares(DateTime date)
        {
            return
                AdditiveTransactions.Where(transaction => transaction.SettlementDate <= date).Select(
                    transaction => transaction.Shares).Sum();
        }

        /// <summary>
        ///   Gets the total number of shares that were owned but are no longer owned.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        private double GetClosedShares(DateTime date)
        {
            return
                SubtractiveTransactions.Where(transaction => transaction.SettlementDate <= date).Select(
                    transaction => transaction.Shares).Sum();
        }

        #endregion

        partial void OnTickerChanging(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("value", "Ticker must not be null, empty, or whitespace.");
            }
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<decimal> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}