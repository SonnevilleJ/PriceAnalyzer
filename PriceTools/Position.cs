﻿using System;
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
        private readonly IList<ITransaction> _additiveTransactions;
        private readonly IList<ITransaction> _subtractiveTransactions;

        #region Constructors

        /// <summary>
        ///   Constructs a Position from existing <see cref = "ITransaction" />s.
        /// </summary>
        /// <param name = "transactions">The <see cref = "ITransaction" />s in this Position.</param>
        public Position(params ITransaction[] transactions)
        {
            if (transactions == null || transactions.Length == 0)
            {
                throw new ArgumentNullException("transactions");
            }

            _additiveTransactions = new List<ITransaction>();
            _subtractiveTransactions = new List<ITransaction>();
            foreach (ITransaction transaction in transactions)
            {
                AddTransaction(transaction);
            }

            Validate();
        }

        #endregion

        private Position(SerializationInfo info, StreamingContext context)
        {
            _additiveTransactions =
                (IList<ITransaction>) info.GetValue("AdditiveTransactions", typeof (IList<ITransaction>));
            _subtractiveTransactions =
                (IList<ITransaction>) info.GetValue("SubtractiveTransactions", typeof (IList<ITransaction>));
            Validate();
        }

        #region IPosition Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Gets an enumeration of all <see cref = "ITransaction" />s in this IPosition.
        /// </summary>
        public IEnumerable<ITransaction> Transactions
        {
            get
            {
                int count = _additiveTransactions.Count + _subtractiveTransactions.Count;
                ITransaction[] list = new ITransaction[count];
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
            get { return GetOpenShares(DateTime.Now); }
        }

        /// <summary>
        ///   Gets the <see cref = "IPosition.PositionStatus" /> of this Position as of a given <see cref = "DateTime" />.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        public PositionStatus GetPositionStatus(DateTime date)
        {
            return GetOpenShares(date) == 0 ? PositionStatus.Open : PositionStatus.Closed;
        }

        /// <summary>
        ///   Gets the current <see cref = "IPosition.PositionStatus" /> of this IPosition.
        /// </summary>
        public PositionStatus PositionStatus
        {
            get { return GetPositionStatus(DateTime.Now); }
        }

        /// <summary>
        ///   Gets the total value of the Position, including commissions.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        public decimal this[DateTime date]
        {
            get { return GetValue(date); }
        }

        /// <summary>
        ///   Gets the span of the ITimeSeries.
        /// </summary>
        public int Span
        {
            get { throw new NotImplementedException(); }
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
        /// <param name = "date">The date to check.</param>
        /// <returns>A value indicating if the ITimeSeries has a valid value for the given date.</returns>
        public bool HasValue(DateTime date)
        {
            return date > Head && date < Tail;
        }

        /// <summary>
        ///   Adds an <see cref = "ITransaction" /> to this IPosition.
        ///   Note: An IPosition can only contain <see cref = "ITransaction" />s for a single ticker symbol.
        /// </summary>
        /// <param name = "transaction">The <see cref = "ITransaction" /> to add to the IPosition.</param>
        public void AddTransaction(ITransaction transaction)
        {
            switch (transaction.OrderType)
            {
                case OrderType.Buy:
                case OrderType.SellShort:
                    IncreasePosition(transaction);
                    break;
                case OrderType.Sell:
                case OrderType.BuyToCover:
                    DecreasePosition(transaction);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("transaction", transaction,
                                                          "Positions can only contain ITransaction types Buy, BuyToCover, Sell, and SellShort.");
            }
        }

        /// <summary>
        ///   Gets the value of the IPortfolio as of a given date.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <param name = "considerCommissions">A value indicating whether commissions should be included in the result.</param>
        /// <returns>The value of the IPortfolio as of the given date.</returns>
        public decimal GetValue(DateTime date, bool considerCommissions)
        {
            decimal proceeds = GetProceeds(date);
            decimal costs = GetCost(date);
            decimal commissions = (considerCommissions ? GetCommissions(date) : 0);
            return proceeds + costs + commissions;
        }

        /// <summary>
        ///   Gets the value of the IPortfolio as of a given date.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the IPortfolio as of the given date.</returns>
        public decimal GetValue(DateTime date)
        {
            return GetValue(date, true);
        }

        #endregion

        #region Private Methods

        private TimeSpan Duration
        {
            get { return Last.SettlementDate - First.SettlementDate; }
        }

        /// <summary>
        ///   Gets or sets a <see cref = "ITransaction" /> which added to this Position.
        ///   Typically <see cref = "OrderType.Buy" /> or <see cref = "OrderType.SellShort" /> <see cref = "ITransaction" />s.
        /// </summary>
        private IEnumerable<ITransaction> AdditiveTransactions
        {
            get { return _additiveTransactions; }
        }

        /// <summary>
        ///   Gets or sets a <see cref = "ITransaction" /> which subtracted from this Position.
        ///   Typically <see cref = "OrderType.Sell" /> or <see cref = "OrderType.BuyToCover" /> <see cref = "ITransaction" />s.
        /// </summary>
        private IEnumerable<ITransaction> SubtractiveTransactions
        {
            get { return _subtractiveTransactions; }
        }

        private ITransaction Last
        {
            get { return SubtractiveTransactions.Last(); }
        }

        private ITransaction First
        {
            get { return AdditiveTransactions.First(); }
        }

        /// <summary>
        ///   Gets the gross investment of this Position, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases as a negative number.</returns>
        public decimal GetCost(DateTime date)
        {
            return AdditiveTransactions.Where(transaction => transaction.SettlementDate <= date).Aggregate(0m,
                                                                                                           (current,
                                                                                                            transaction)
                                                                                                           =>
                                                                                                           current -
                                                                                                           (transaction.
                                                                                                                Price*
                                                                                                            (decimal)
                                                                                                            transaction.
                                                                                                                Shares));
        }

        /// <summary>
        ///   Gets the gross proceeds of this Position, ignoring all costs and commissions.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales as a positive number.</returns>
        public decimal GetProceeds(DateTime date)
        {
            return SubtractiveTransactions.Where(transaction => transaction.SettlementDate <= date).Aggregate(0m,
                                                                                                              (current,
                                                                                                               transaction)
                                                                                                              =>
                                                                                                              current +
                                                                                                              (transaction
                                                                                                                   .
                                                                                                                   Price*
                                                                                                               (decimal)
                                                                                                               transaction
                                                                                                                   .
                                                                                                                   Shares));
        }

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "ITransaction" />s as a negative number.</returns>
        public decimal GetCommissions(DateTime date)
        {
            return GetCommissions(date, _additiveTransactions) + GetCommissions(date, _subtractiveTransactions);
        }

        /// <summary>
        ///   Gets the raw rate of return for this Position, not accounting for commissions.
        /// </summary>
        public decimal GetRawReturn(DateTime date)
        {
            if (GetClosedShares(date) > 0)
            {
                return -(GetValue(date, false))/GetCost(date);
            }
            throw new InvalidOperationException("Cannot calculate raw return for an open position.");
        }

        /// <summary>
        ///   Gets the total rate of return for this Position, after commissions.
        /// </summary>
        public decimal GetTotalReturn(DateTime date)
        {
            if (GetClosedShares(date) > 0)
            {
                decimal proceeds = (GetProceeds(date) + GetCommissions(date, _additiveTransactions));
                decimal costs = (GetCost(date) + GetCommissions(date, _subtractiveTransactions));
                decimal profit = proceeds + costs;
                return -(profit/GetCost(date));
            }
            throw new InvalidOperationException("Cannot calculate return for an open position.");
        }

        /// <summary>
        ///   Validates the Position.
        /// </summary>
        public void Validate()
        {
            foreach (ITransaction aTransaction in AdditiveTransactions)
            {
                // Validate OrderType
                switch (aTransaction.OrderType)
                {
                    case OrderType.Buy:
                    case OrderType.SellShort:
                        break;
                    default:
                        // Not an opening transaction
                        throw new InvalidPositionException("Additive transactions must be of type Buy or SellShort.");
                }
            }
            foreach (ITransaction sTransaction in SubtractiveTransactions)
            {
                // Validate OrderType
                switch (sTransaction.OrderType)
                {
                    case OrderType.Sell:
                    case OrderType.BuyToCover:
                        break;
                    default:
                        // Not an opening transaction
                        throw new InvalidPositionException(
                            "Subtractive transactions must be of type Sell or BuyToCover.");
                }

                // Verify that sold shares does not exceed available shares at the time of the transaction.
                DateTime date = sTransaction.SettlementDate.Subtract(new TimeSpan(0, 0, 0, 1));
                if (GetClosedShares(date) > GetOpenedShares(date))
                {
                    throw new InvalidPositionException(
                        "Shares traded in subtractive transactions cannot exceed shares traded in additive transactions.");
                }
            }
        }

        /// <summary>
        ///   Gets the total rate of return on an annual basis for this Position.
        /// </summary>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public decimal GetTotalAnnualReturn(DateTime date)
        {
            if (SubtractiveTransactions.Count() == 0)
            {
                throw new InvalidOperationException("Cannot calculate return without any sale transactions.");
            }
            decimal totalReturn = GetTotalReturn(date);
            decimal time = (Duration.Days/365.0m);
            return totalReturn/time;
        }

        /// <summary>
        ///   Gets the net shares held at a given date.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        private double GetOpenShares(DateTime date)
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

        /// <summary>
        ///   Adds newly purchased shares to the IPosition.
        /// </summary>
        /// <param name = "transaction">An ITransaction that would open or add shares to the IPosition.</param>
        private void IncreasePosition(ITransaction transaction)
        {
            _additiveTransactions.Add(transaction);
        }

        /// <summary>
        ///   Adds newly sold shares to the IPosition.
        /// </summary>
        /// <param name = "transaction">An ITransaction that would close or subtract shares from the IPosition.</param>
        private void DecreasePosition(ITransaction transaction)
        {
            _subtractiveTransactions.Add(transaction);
        }

        private static decimal GetCommissions(DateTime date, IEnumerable<ITransaction> transactions)
        {
            return -1*transactions.Sum(transaction => transaction.Commission*(decimal) transaction.Shares);
        }

        #endregion
    }
}