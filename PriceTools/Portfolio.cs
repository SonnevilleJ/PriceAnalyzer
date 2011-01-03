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

        private IList<ITransaction> _transactions;
        private decimal _availableCash;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a Portfolio with only an opening deposit.
        /// </summary>
        /// <param name="openingDeposit">The amount deposited when opening the Portfolio.</param>
        public Portfolio(decimal openingDeposit)
            : this(openingDeposit, new List<ITransaction>())
        {
        }

        /// <summary>
        /// Constructs a Portfolio with available cash and a list of previous transactions.
        /// </summary>
        /// <param name="availableCash"></param>
        /// <param name="transactions"></param>
        public Portfolio(decimal availableCash, IList<ITransaction> transactions)
        {
            if(availableCash < 0)
            {
                throw new ArgumentOutOfRangeException("availableCash", availableCash, "Cash must be greater than or equal to $0.00");
            }
            _availableCash = availableCash;
            _transactions = transactions;
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
        public decimal this[int index]
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

        #endregion

        #region Implementation of IPortfolio

        /// <summary>
        ///   Gets an <see cref = "IList{T}" /> of open positions held in this Portfolio.
        /// </summary>
        public IList<IPosition> OpenPositions
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
            return GetValue(DateTime.Today);
        }

        /// <summary>
        ///   Gets the total value of this Portfolio as of a given <see cref = "DateTime" />.
        /// </summary>
        /// <param name = "asOfDate">The <see cref = "DateTime" /> of which the value should be retrieved.</param>
        /// <returns>The total value of this Portfolio as of the given <see cref = "DateTime" />.</returns>
        public decimal GetValue(DateTime asOfDate)
        {
            throw new NotImplementedException(); // TODO: consider asOfDate
            return OpenPositions.Sum(position => position.TotalValue) + AvailableCash;
        }

        /// <summary>
        ///   Adds an <see cref="ITransaction"/> to this portfolio.
        /// </summary>
        /// <param name="transaction">The <see cref="ITransaction"/> to add to this portfolio.</param>
        public void AddTransaction(ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
