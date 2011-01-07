using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a portfolio of investments.
    /// </summary>
    public class Portfolio : IPortfolio
    {
        #region Private Members

        private readonly IList<ITransaction> _transactions = new List<ITransaction>();
        private decimal _availableCash;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an empty Portfolio.
        /// </summary>
        public Portfolio()
            : this(0.00m)
        {
        }

        /// <summary>
        /// Constructs a Portfolio with only an opening deposit.
        /// </summary>
        /// <param name="openingDeposit">The amount deposited when opening the Portfolio.</param>
        public Portfolio(decimal openingDeposit)
        {
            if(openingDeposit < 0)
            {
                throw new ArgumentOutOfRangeException("openingDeposit", openingDeposit, "Cash must be greater than or equal to $0.00");
            }
            _availableCash = openingDeposit;
        }

        /// <summary>
        /// Constructs a Portfolio with available cash and a list of previous transactions.
        /// </summary>
        /// <param name="availableCash"></param>
        /// <param name="transactions"></param>
        public Portfolio(decimal availableCash, params ITransaction[] transactions)
            : this(availableCash)
        {
            foreach (ITransaction transaction in transactions)
            {
                AddTransaction(transaction);
            }
        }

        #endregion

        #region Implementation of ISerializable

        protected Portfolio(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of ITimeSeries

        /// <summary>
        /// Gets a value stored at a given index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The index of the desired value.</param>
        /// <returns>The value stored at the given index.</returns>
        public decimal this[DateTime index]
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the span of the ITimeSeries.
        /// </summary>
        public int Span
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public DateTime Head
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public DateTime Tail
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasValue(DateTime date)
        {
            return (date >= Head && date <= Tail);
        }

        #endregion

        #region Implementation of IPortfolio

        /// <summary>
        ///   Gets an <see cref = "IList{T}" /> of open positions held in this Portfolio.
        /// </summary>
        public IEnumerable<IPosition> OpenPositions
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///   Gets the amount of uninvested cash in this Portfolio.
        /// </summary>
        public decimal AvailableCash
        {
            get
            {
                return _availableCash;
            }
            private set
            {
                if(value < 0.0m)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Cash amount must be greater than or equal to $0.00.");
                }
                _availableCash = value;
            }
        }

        /// <summary>
        ///   Gets the current total value of this Portfolio.
        /// </summary>
        public decimal GetValue()
        {
            return this[DateTime.Today];
        }

        /// <summary>
        ///   Adds an <see cref="ITransaction"/> to this portfolio.
        /// </summary>
        /// <param name="transaction">The <see cref="ITransaction"/> to add to this portfolio.</param>
        public void AddTransaction(ITransaction transaction)
        {
            switch (transaction.OrderType)
            {
                case OrderType.Buy:
                    AddBuy(transaction);
                    break;
                case OrderType.BuyToCover:
                    AddBuyToCover(transaction);
                    break;
                case OrderType.Deposit:
                    AddDeposit(transaction);
                    break;
                case OrderType.Dividend:
                    AddDividend(transaction);
                    break;
                case OrderType.Sell:
                    AddSell(transaction);
                    break;
                case OrderType.SellShort:
                    AddSellShort(transaction);
                    break;
                case OrderType.Withdrwawal:
                    AddWithDrawal(transaction);
                    break;
            }
        }

        #endregion

        private void AddBuy(ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        private void AddBuyToCover(ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        private void AddDeposit(ITransaction transaction)
        {
            _transactions.Add((Deposit) transaction);
        }

        private void AddDividend(ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        private void AddSell(ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        private void AddSellShort(ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        private void AddWithDrawal(ITransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
