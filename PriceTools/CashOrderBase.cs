using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash order (<see cref="Deposit"/> or <see cref="Withdrawal"/>) for an <see cref="IPortfolio"/>.
    /// </summary>
    [Serializable]
    public abstract class CashOrderBase : ITransaction, IEquatable<CashOrderBase>
    {
        #region Private Members

        private readonly DateTime _date;
        private readonly string _ticker;
        private readonly decimal _amount;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a CashOrderBase.
        /// </summary>
        /// <param name="dateTime">The DateTime of the CashOrderBase.</param>
        /// <param name="amount">The amount of cash in this order.</param>
        protected internal CashOrderBase(DateTime dateTime, decimal amount)
            : this(dateTime, string.Empty, amount)
        {}

        /// <summary>
        /// Constructs a CashOrderBase.
        /// </summary>
        /// <param name="dateTime">The DateTime of the CashOrderBase.</param>
        /// <param name="ticker">The holding which cash is deposited to or withdrawn from.</param>
        /// <param name="amount">The amount of cash in this order.</param>
        protected internal CashOrderBase(DateTime dateTime, string ticker, decimal amount)
        {
            _date = dateTime;
            _ticker = ticker;
            _amount = amount;
        }

        #endregion

        #region Implementation of ITransaction

        /// <summary>
        ///   Gets the DateTime that the <see cref="ITransaction"/> occurred.
        /// </summary>
        public DateTime SettlementDate
        {
            get { return _date; }
        }

        /// <summary>
        ///   Gets the <see cref = "ITransaction.OrderType" /> of this <see cref="ITransaction"/>.
        /// </summary>
        public abstract OrderType OrderType { get; }

        /// <summary>
        ///   Gets the ticker symbol of the security traded in this <see cref="ITransaction"/>.
        /// </summary>
        public string Ticker
        {
            get { return _ticker; }
        }

        /// <summary>
        ///   Gets the amount of securities traded in this <see cref="ITransaction"/>.
        /// </summary>
        public double Shares
        {
            get { return double.Parse(_amount.ToString()); }
        }

        /// <summary>
        ///   Gets the commission charged for this <see cref="ITransaction"/>.
        /// </summary>
        public decimal Commission
        {
            get { return 0.0m; }
        }

        /// <summary>
        /// Gets the per-share price paid for this <see cref="ITransaction"/>.
        /// </summary>
        public abstract decimal Price { get; }

        #endregion

        #region Implementation of ISerializable

        protected CashOrderBase(SerializationInfo info, StreamingContext context)
        {
            _date = info.GetDateTime("Date");
            _ticker = info.GetString("Ticker");
            _amount = info.GetDecimal("Amount");
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Date", _date);
            info.AddValue("Ticker", _ticker);
            info.AddValue("Amount", _amount);
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
        public bool Equals(ITransaction other)
        {
            return Equals((object)other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(CashOrderBase other)
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
            if (obj.GetType() != typeof (CashOrderBase)) return false;
            return this == (CashOrderBase)obj;
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
                int result = _date.GetHashCode();
                result = (result*397) ^ _ticker.GetHashCode();
                result = (result*397) ^ _amount.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(CashOrderBase left, CashOrderBase right)
        {
            return (left._date == right._date &&
                    left._amount == right._amount &&
                    left._ticker == right._ticker);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(CashOrderBase left, CashOrderBase right)
        {
            return !(left == right);
        }

        #endregion
    }
}