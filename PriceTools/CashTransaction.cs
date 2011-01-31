using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for an <see cref="ICashAccount"/>.
    /// </summary>
    public abstract class CashTransaction : ICashTransaction, IEquatable<CashTransaction>
    {
        #region Private Members

        private readonly DateTime _settlementDate;
        private readonly OrderType _orderType;
        private readonly decimal _amount;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a CashTransaction.
        /// </summary>
        /// <param name="settlementDate">The DateTime the CashTransaction takes place.</param>
        /// <param name="orderType">The <see cref="OrderType"/> of the CashTransaction. Must be Deposit or Withdrawal.</param>
        /// <param name="amount">The amount of cash in this CashTransaction.</param>
        protected CashTransaction(DateTime settlementDate, OrderType orderType, decimal amount)
        {
            _settlementDate = settlementDate;
            _orderType = orderType;
            _amount = amount;
        }

        /// <summary>
        /// Deserializes a CashTransaction.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CashTransaction(SerializationInfo info, StreamingContext context)
        {
            if(info == null) throw new ArgumentNullException("info");

            _settlementDate = info.GetDateTime("SettlementDate");
            _orderType = (OrderType) info.GetValue("OrderType", typeof (OrderType));
            _amount = info.GetDecimal("Amount");
        }

        #endregion

        #region Implementation of ISerializable

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if(info == null) throw new ArgumentNullException("info");

            info.AddValue("SettlementDate", _settlementDate);
            info.AddValue("OrderType", _orderType);
            info.AddValue("Amount", _amount);
        }

        #endregion

        #region Implementation of IEquatable<ICashTransaction>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(CashTransaction left, CashTransaction right)
        {
            bool typesMatch = left.OrderType == right.OrderType;
            bool datesMatch = left._settlementDate == right._settlementDate;
            bool amountsMatch = left._amount == right._amount;

            return typesMatch && datesMatch && amountsMatch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(CashTransaction left, CashTransaction right)
        {
            return !(left == right);
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
            if (obj is CashTransaction) return this == (CashTransaction)obj;
            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(CashTransaction other)
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
        public bool Equals(ICashTransaction other)
        {
            return Equals((object)other);
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
                return (_settlementDate.GetHashCode()*397) ^ _amount.GetHashCode();
            }
        }

        #endregion

        #region Implementation of ICashTransaction

        /// <summary>
        ///   Gets the DateTime that the CashTransaction occurred.
        /// </summary>
        public DateTime SettlementDate
        {
            get { return _settlementDate; }
        }

        /// <summary>
        ///   Gets the <see cref = "PriceTools.OrderType" /> of this CashTransaction.
        /// </summary>
        public OrderType OrderType
        {
            get { return _orderType; }
        }

        /// <summary>
        ///   Gets the amount of cash in this CashTransaction.
        /// </summary>
        public decimal Amount
        {
            get { return _amount; }
        }

        #endregion
    }
}