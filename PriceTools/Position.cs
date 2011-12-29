using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A trade made for a financial security. A Position is comprised of an opening shareTransaction, and optionally, a closing shareTransaction.
    /// </summary>
    public class Position : IPosition
    {
        #region Private Members

        private IPriceSeries _priceSeries;
        private string _ticker;
        private readonly ICollection<IShareTransaction> _transactions = new List<IShareTransaction>();

        #endregion

        #region Constructors

        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name = "ticker">The ticker symbol that this Position will hold. All transactions will use this ticker symbol.</param>
        internal Position(string ticker)
        {
            Ticker = ticker;
            _priceSeries = PriceSeriesFactory.CreatePriceSeries(Ticker);
        }

        #endregion

        #region IPosition Members

        /// <summary>
        ///   Gets the average cost of all held shares in this Position as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The average cost of all shares held at <paramref name = "settlementDate" />.</returns>
        public decimal CalculateAverageCost(DateTime settlementDate)
        {
            var transactions = _transactions
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .OrderBy(transaction => transaction.SettlementDate).ToList();
            var count = transactions.Count();

            var totalCost = 0.00m;
            var shares = 0.0;

            for (var i = 0; i < count; i++)
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
            get { return CalculateValue(index); }
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
        /// Gets the <see cref="ITimeSeries.Resolution"/> of price data stored within the ITimeSeries.
        /// </summary>
        public Resolution Resolution
        {
            get { return PriceSeries.Resolution; }
        }

        /// <summary>
        /// Gets the values stored within the ITimeSeries.
        /// </summary>
        public IDictionary<DateTime, decimal> Values
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        ///   Determines if the ITimeSeries has a valid value for a given date.
        /// </summary>
        /// <param name = "settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimeSeries has a valid value for the given date.</returns>
        public bool HasValueInRange(DateTime settlementDate)
        {
            var end = Tail;
            if (CalculateValue(settlementDate) != 0)
            {
                end = settlementDate;
            }
            return settlementDate >= Head && settlementDate <= end;
        }

        /// <summary>
        ///   Gets the value of any shares held the Position as of a given date.
        /// </summary>
        /// <param name="provider">The <see cref="PriceHistoryCsvFileProvider"/> to use when requesting price data.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the shares held in the Position as of the given date.</returns>
        public decimal CalculateInvestedValue(PriceHistoryCsvFileProvider provider, DateTime settlementDate)
        {
            var heldShares = (decimal) GetHeldShares(settlementDate);
            if (heldShares == 0) return 0;

            if (!PriceSeries.HasValueInRange(settlementDate)) (PriceSeries as PriceSeries).DownloadPriceData(provider, settlementDate);
            var price = PriceSeries[settlementDate];
            return heldShares*price;
        }

        /// <summary>
        ///   Gets an enumeration of all <see cref = "ShareTransaction" />s in this Position.
        /// </summary>
        public IList<ITransaction> Transactions
        {
            get { return new List<ITransaction>(_transactions); }
        }

        /// <summary>
        ///   Gets the value of the Position, excluding all commissions, as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the Position as of the given date.</returns>
        public decimal CalculateValue(DateTime settlementDate)
        {
            var proceeds = CalculateProceeds(settlementDate); // positive proceeds = gain, negative proceeds = loss
            var totalCosts = CalculateCost(settlementDate);
                // positive totalCosts = revenue, negative totalCosts = expense

            var heldShares = GetHeldShares(settlementDate);
            var totalShares = GetOpenedShares(settlementDate);

            var costOfUnsoldShares = 0.00m;
            if (totalShares != 0)
            {
                costOfUnsoldShares = totalCosts*(decimal) (heldShares/totalShares);
            }
            return proceeds + costOfUnsoldShares;
        }

        /// <summary>
        ///   Gets the total value of the Position, including any commissions, as of a given date.
        /// </summary>
        /// <param name="provider">The <see cref="PriceHistoryCsvFileProvider"/> to use when requesting price data.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total value of the Position as of the given date.</returns>
        public decimal CalculateTotalValue(PriceHistoryCsvFileProvider provider, DateTime settlementDate)
        {
            return CalculateValue(settlementDate) - CalculateCommissions(settlementDate);
        }

        /// <summary>
        ///   Gets the gross investment of this Position, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases as a negative number.</returns>
        public decimal CalculateCost(DateTime settlementDate)
        {
            return AdditiveTransactions.AsParallel()
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .Sum(transaction => transaction.Price*(decimal) transaction.Shares);
        }

        /// <summary>
        ///   Gets the gross proceeds of this Position, ignoring all totalCosts and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales as a positive number.</returns>
        public decimal CalculateProceeds(DateTime settlementDate)
        {
            return -1*SubtractiveTransactions.AsParallel()
                          .Where(transaction => transaction.SettlementDate <= settlementDate)
                          .Sum(transaction => transaction.Price*(decimal) transaction.Shares);
        }

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "IShareTransaction" />s as a negative number.</returns>
        public decimal CalculateCommissions(DateTime settlementDate)
        {
            return
                _transactions.AsParallel().Where(transaction => transaction.SettlementDate <= settlementDate).Sum(
                    transaction => transaction.Commission);
        }

        /// <summary>
        /// Adds an IShareTransaction to the Position.
        /// </summary>
        /// <param name="shareTransaction"></param>
        public void AddTransaction(IShareTransaction shareTransaction)
        {
            // verify shareTransaction is apporpriate for this Position.
            Validate(shareTransaction);

            _transactions.Add(shareTransaction);
        }

        /// <summary>
        /// Gets an <see cref="IList{IHolding}"/> from the transactions in the Position.
        /// </summary>
        /// <param name="settlementDate">The latest date used to include a transaction in the calculation.</param>
        /// <returns>An <see cref="IList{IHolding}"/> of the transactions in the Position.</returns>
        public IList<IHolding> CalculateHoldings(DateTime settlementDate)
        {
            var holdings = new List<IHolding>();
            var buys = AdditiveTransactions.Where(t => t.SettlementDate < settlementDate).OrderByDescending(t => t.SettlementDate);
            var buysUsed = 0;
            var unusedSharesInCurrentBuy = 0.0;
            IShareTransaction buy = null;

            foreach (var sell in SubtractiveTransactions.Where(t => t.SettlementDate <= settlementDate).OrderByDescending(t => t.SettlementDate))
            {
                // collect shares from most recent buy
                var sharesToMatch = sell.Shares;
                while (sharesToMatch > 0)
                {
                    // find a matching purchase and record a new holding
                    // must keep track of remaining shares in corresponding purchase
                    if (unusedSharesInCurrentBuy == 0)
                    {
                        buy = buys.Skip(buysUsed).First();
                    }

                    var availableShares = unusedSharesInCurrentBuy > 0 ? unusedSharesInCurrentBuy : buy.Shares;
                    var neededShares = sharesToMatch;
                    double shares;
                    if (availableShares >= neededShares)
                    {
                        shares = neededShares;
                        unusedSharesInCurrentBuy = availableShares - shares;
                        if (unusedSharesInCurrentBuy == 0)
                        {
                            buysUsed++;
                        }
                    }
                    else
                    {
                        shares = availableShares;
                        buysUsed++;
                    }
                    var holding = new Holding
                                      {
                                          Ticker = Ticker,
                                          Head = buy.SettlementDate,
                                          Tail = sell.SettlementDate,
                                          Shares = shares,
                                          OpenPrice = buy.Price*(decimal) shares,
                                          ClosePrice = -1*sell.Price*(decimal) shares
                                      };
                    holdings.Add(holding);

                    sharesToMatch -= shares;
                }
            }
            return holdings;
        }

        /// <summary>
        /// Validates a transaction without adding it to the Position.
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

        #region Helper Properties

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
            get { return _transactions.Where(t => Additive.Contains(t.OrderType)); }
        }

        /// <summary>
        ///   Gets a list of <see cref = "IShareTransaction" />s which subtracted from this Position.
        ///   Typically <see cref = "OrderType.Sell" /> or <see cref = "OrderType.BuyToCover" /> <see cref = "IShareTransaction" />s.
        /// </summary>
        private IEnumerable<IShareTransaction> SubtractiveTransactions
        {
            get { return _transactions.Where(t => Subtractive.Contains(t.OrderType)); }
        }

        private IShareTransaction Last
        {
            get { return _transactions.OrderBy(t => t.SettlementDate).Last(); }
        }

        private IShareTransaction First
        {
            get { return _transactions.OrderBy(t => t.SettlementDate).First(); }
        }

        private IPriceSeries PriceSeries
        {
            get { return _priceSeries ?? (_priceSeries = PriceSeriesFactory.CreatePriceSeries(Ticker)); }
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

        #endregion

        #region Helper Methods

        private void AddTransaction(double shares, OrderType type, DateTime settlementDate, decimal price, decimal commission)
        {
            var shareTransaction = TransactionFactory.CreateShareTransaction(settlementDate, type, Ticker, price, shares, commission);
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
                    var date = shareTransaction.SettlementDate;
                    var heldShares = GetHeldShares(date);
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
    }
}