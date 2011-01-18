using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A trade made for a financial security. A Position is comprised of an opening transaction, and optionally, a closing transaction.
    /// </summary>
    [Serializable]
    public class Position : IPosition
    {
        /// <summary>
        /// The default commission charged for a transaction.
        /// </summary>
        public const decimal DefaultCommission = 0.00m;

        #region Private Members

        private readonly string _ticker;
        private readonly List<Transaction> _additiveTransactions;
        private readonly List<Transaction> _subtractiveTransactions;
        private decimal _averageCost;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker symbol that this portfolio will hold. All transactions will use this ticker symbol.</param>
        public Position(string ticker)
        {
            if(ticker == null)
            {
                throw new ArgumentNullException("ticker", "Ticker must not be null.");
            }
            _ticker = ticker;
            _additiveTransactions = new List<Transaction>();
            _subtractiveTransactions = new List<Transaction>();
            _averageCost = 0.00m;
        }

        /// <summary>
        /// Deserializes a Position.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected Position(SerializationInfo info, StreamingContext context)
        {
            _ticker = info.GetString("Ticker");
            _additiveTransactions =
                (List<Transaction>) info.GetValue("AdditiveTransactions", typeof (List<Transaction>));
            _subtractiveTransactions =
                (List<Transaction>) info.GetValue("SubtractiveTransactions", typeof (List<Transaction>));
            _averageCost = info.GetDecimal("AverageCost");
            //Validate(); // DO NOT perform validation during deserialization. Until all objects are deserialized, validation will fail!
        }

        #endregion

        #region IPosition Members

        /// <summary>
        /// Serializes a Position.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Ticker", _ticker);
            info.AddValue("AdditiveTransactions", _additiveTransactions);
            info.AddValue("SubtractiveTransactions", _subtractiveTransactions);
            info.AddValue("AverageCost", _averageCost);
        }

        /// <summary>
        ///   Gets an enumeration of all <see cref = "ITransaction" />s in this IPosition.
        /// </summary>
        public IList<ITransaction> Transactions
        {
            get
            {
                int count = _additiveTransactions.Count + _subtractiveTransactions.Count;
                Transaction[] list = new Transaction[count];
                _additiveTransactions.CopyTo(list, 0);
                _subtractiveTransactions.CopyTo(list, _additiveTransactions.Count);

                return list;
            }
        }

        /// <summary>
        ///   Gets the total number of currently held shares.
        /// </summary>
        public double OpenShares
        {
            get { return GetHeldShares(DateTime.Now); }
        }

        /// <summary>
        /// Gets the ticker symbol held by this Position.
        /// </summary>
        public string Ticker
        {
            get { return _ticker; }
        }

        /// <summary>
        /// Gets the average cost of all held shares in this Position as of a given asOfDate.
        /// </summary>
        /// <param name="asOfDate">The <see cref="DateTime"/> to use.</param>
        /// <returns>The average cost of all shares held at <paramref name="asOfDate"/>.</returns>
        public decimal GetAverageCost(DateTime asOfDate)
        {
            List<ITransaction> transactions = Transactions
                .Where(transaction => transaction.SettlementDate <= asOfDate)
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
                        totalCost += (transactions[i].Price * (decimal)transactions[i].Shares);
                        shares += transactions[i].Shares;
                        break;
                    case OrderType.Sell:
                    case OrderType.BuyToCover:
                        totalCost -= ((totalCost / (decimal)shares) * (decimal)transactions[i].Shares);
                        shares -= transactions[i].Shares;
                        break;
                }
            }

            return totalCost / (decimal)shares;
        }

        /// <summary>
        /// Buys shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name="asOfDate">The asOfDate of this transaction.</param>
        /// <param name="shares">The number of shares in this transaction.</param>
        /// <param name="price">The per-share price of this transaction.</param>
        /// <param name="commission">The commission paid for this transaction.</param>
        public void Buy(DateTime date, double shares, decimal price, decimal commission)
        {
            AddTransaction(shares, OrderType.Buy, date, price, commission);
        }

        /// <summary>
        /// Buys shares of the ticker held by this IPosition to cover a previous ShortSell.
        /// </summary>
        /// <param name="asOfDate">The asOfDate of this transaction.</param>
        /// <param name="shares">The number of shares in this transaction. Shares cannot exceed currently shorted shares.</param>
        /// <param name="price">The per-share price of this transaction.</param>
        /// <param name="commission">The commission paid for this transaction.</param>
        public void BuyToCover(DateTime date, double shares, decimal price, decimal commission)
        {
            AddTransaction(shares, OrderType.BuyToCover, date, price, commission);
        }

        /// <summary>
        /// Sells shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name="asOfDate">The asOfDate of this transaction.</param>
        /// <param name="shares">The number of shares in this transaction. Shares connot exceed currently held shares.</param>
        /// <param name="price">The per-share price of this transaction.</param>
        /// <param name="commission">The commission paid for this transaction.</param>
        public void Sell(DateTime date, double shares, decimal price, decimal commission)
        {
            AddTransaction(shares, OrderType.Sell, date, price, commission);
        }

        /// <summary>
        /// Sell short shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name="asOfDate">The asOfDate of this transaction.</param>
        /// <param name="shares">The number of shares in this transaction.</param>
        /// <param name="price">The per-share price of this transaction.</param>
        /// <param name="commission">The commission paid for this transaction.</param>
        public void SellShort(DateTime date, double shares, decimal price, decimal commission)
        {
            AddTransaction(shares, OrderType.SellShort, date, price, commission);
        }

        /// <summary>
        ///   Gets the total value of the Position, including commissions.
        /// </summary>
        /// <param name = "asOfDate">The <see cref = "DateTime" /> to use.</param>
        public decimal this[DateTime asOfDate]
        {
            get { return GetValue(asOfDate); }
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
        ///   Determines if the ITimeSeries has a valid value for a given asOfDate.
        /// </summary>
        /// <param name = "asOfDate">The asOfDate to check.</param>
        /// <returns>A value indicating if the ITimeSeries has a valid value for the given asOfDate.</returns>
        public bool HasValue(DateTime asOfDate)
        {
            DateTime end = Tail;
            if (GetValue(asOfDate) != 0)
            {
                end = asOfDate;
            }
            return asOfDate >= Head && asOfDate <= end;
        }

        /// <summary>
        ///   Gets the value of the IPortfolio as of a given asOfDate, excluding all commissions.
        /// </summary>
        /// <param name = "asOfDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the IPortfolio as of the given asOfDate.</returns>
        public decimal GetValue(DateTime asOfDate)
        {
            decimal proceeds = GetProceeds(asOfDate);   // positive proceeds = gain, negative proceeds = loss
            decimal totalCosts = GetCost(asOfDate);     // positive totalCosts = revenue, negative totalCosts = expense

            double heldShares = GetHeldShares(asOfDate);
            double totalShares = GetOpenedShares(asOfDate);
            double soldShares = totalShares - heldShares;

            decimal costOfUnsoldShares = 0.00m;
            if (totalShares != 0)
            {
                costOfUnsoldShares = totalCosts * (decimal)(heldShares / totalShares);
            }
            return proceeds + costOfUnsoldShares;
        }

        /// <summary>
        ///   Gets the gross investment of this Position, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name = "asOfDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases as a negative number.</returns>
        public decimal GetCost(DateTime asOfDate)
        {
            return AdditiveTransactions
                .Where(transaction => transaction.SettlementDate <= asOfDate)
                .Sum(transaction => transaction.Price * (decimal)transaction.Shares);
        }

        /// <summary>
        ///   Gets the gross proceeds of this Position, ignoring all totalCosts and commissions.
        /// </summary>
        /// <param name = "asOfDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales as a positive number.</returns>
        public decimal GetProceeds(DateTime asOfDate)
        {
            return -1 * SubtractiveTransactions
                .Where(transaction => transaction.SettlementDate <= asOfDate)
                .Aggregate(0m, (current, transaction) => current + (transaction.Price * (decimal)transaction.Shares));
        }

        /// <summary>
        ///   Gets the total commissions paid as of a given asOfDate.
        /// </summary>
        /// <param name = "asOfDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "ITransaction" />s as a negative number.</returns>
        public decimal GetCommissions(DateTime asOfDate)
        {
            return Transactions.Where(transaction => transaction.SettlementDate <= asOfDate).Sum(transaction => transaction.Commission);
        }

        /// <summary>
        ///   Gets the raw rate of return for this Position, not accounting for commissions.
        /// </summary>
        /// <param name = "asOfDate">The <see cref = "DateTime" /> to use.</param>
        public decimal GetRawReturn(DateTime asOfDate)
        {
            if (GetClosedShares(asOfDate) > 0)
            {
                return (GetValue(asOfDate) / GetCost(asOfDate)) - 1;
            }
            throw new InvalidOperationException("Cannot calculate raw return for an open position.");
        }

        /// <summary>
        ///   Gets the total rate of return for this Position, after commissions.
        /// </summary>
        /// <param name = "asOfDate">The <see cref = "DateTime" /> to use.</param>
        public decimal GetTotalReturn(DateTime asOfDate)
        {
            decimal proceeds = GetProceeds(asOfDate);
            decimal costs = GetCost(asOfDate);
            decimal commissions = GetCommissions(asOfDate);
            decimal profit = proceeds - costs - commissions;
            return (profit / costs);
        }

        /// <summary>
        ///   Gets the total rate of return on an annual basis for this Position.
        /// </summary>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public decimal GetTotalAnnualReturn(DateTime asOfDate)
        {
            decimal totalReturn = GetTotalReturn(asOfDate);
            decimal time = (Duration.Days / 365.0m);
            return totalReturn / time;
        }

        #endregion

        #region Private Methods

        private void AddTransaction(double shares, OrderType type, DateTime date, decimal price, decimal commission)
        {
            ITransaction transaction = TransactionFactory.CreateTransaction(date, type, _ticker, price, shares, commission);

            Validate(transaction); // verify transaction is apporpriate for this Position.
            switch (type)
            {
                case OrderType.Buy:
                case OrderType.SellShort:
                    _additiveTransactions.Add((Transaction)transaction);
                    break;
                case OrderType.Sell:
                case OrderType.BuyToCover:
                    _subtractiveTransactions.Add((Transaction)transaction);
                    break;
            }
        }

        private TimeSpan Duration
        {
            get { return Last.SettlementDate - First.SettlementDate; }
        }

        /// <summary>
        ///   Gets a list of <see cref = "ITransaction" />s which added to this Position.
        ///   Typically <see cref = "OrderType.Buy" /> or <see cref = "OrderType.SellShort" /> <see cref = "ITransaction" />s.
        /// </summary>
        private IEnumerable<ITransaction> AdditiveTransactions
        {
            get { return _additiveTransactions; }
        }

        /// <summary>
        ///   Gets a list of <see cref = "ITransaction" />s which subtracted from this Position.
        ///   Typically <see cref = "OrderType.Sell" /> or <see cref = "OrderType.BuyToCover" /> <see cref = "ITransaction" />s.
        /// </summary>
        private IEnumerable<ITransaction> SubtractiveTransactions
        {
            get { return _subtractiveTransactions; }
        }

        private ITransaction Last
        {
            get { return Transactions.Last(); }
        }

        private ITransaction First
        {
            get { return Transactions.First(); }
        }

        private void Validate(ITransaction transaction)
        {
            // Validate OrderType
            switch (transaction.OrderType)
            {
                case OrderType.Buy:
                case OrderType.SellShort:
                    // long transactions are OK
                    break;
                case OrderType.BuyToCover:
                case OrderType.Sell:
                    // Verify that sold shares does not exceed available shares at the time of the transaction.
                    DateTime date = transaction.SettlementDate.Subtract(new TimeSpan(0, 0, 0, 1));
                    double heldShares = GetHeldShares(date);
                    if (transaction.Shares > heldShares)
                    {
                        throw new InvalidOperationException(
                            String.Format(
                                "This transaction requires {0} shares, but only {1} shares are held by this Position as of {2}.",
                                transaction.Shares, heldShares, date));
                    }
                    break;
            }
        }


        /// <summary>
        ///   Gets the net shares held at a given asOfDate.
        /// </summary>
        /// <param name = "asOfDate">The <see cref = "DateTime" /> to use.</param>
        private double GetHeldShares(DateTime asOfDate)
        {
            return GetOpenedShares(asOfDate) - GetClosedShares(asOfDate);
        }

        /// <summary>
        ///   Gets the cumulative number of shares that have ever been owned before a given asOfDate.
        /// </summary>
        /// <param name = "asOfDate">The <see cref = "DateTime" /> to use.</param>
        private double GetOpenedShares(DateTime asOfDate)
        {
            return
                AdditiveTransactions.Where(transaction => transaction.SettlementDate <= asOfDate).Select(
                    transaction => transaction.Shares).Sum();
        }

        /// <summary>
        ///   Gets the total number of shares that were owned but are no longer owned.
        /// </summary>
        /// <param name = "asOfDate">The <see cref = "DateTime" /> to use.</param>
        private double GetClosedShares(DateTime asOfDate)
        {
            return
                SubtractiveTransactions.Where(transaction => transaction.SettlementDate <= asOfDate).Select(
                    transaction => transaction.Shares).Sum();
        }

        #endregion

        #region Equality Checks

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ITimeSeries other)
        {
            return Equals((object)other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Position)) return false;
            return this == (Position)obj;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = _ticker.GetHashCode();
                result = (result * 397) ^ _additiveTransactions.GetHashCode();
                result = (result * 397) ^ _subtractiveTransactions.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Position left, Position right)
        {
            if (left._ticker == right._ticker)
            {
                if (left.Transactions.Count == right.Transactions.Count)
                {
                    return left.Transactions.All(transaction => right.Transactions.Contains(transaction));
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        #endregion
    }
}