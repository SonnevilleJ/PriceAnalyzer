using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a financial transaction.
    /// </summary>
    [Serializable]
    internal abstract class TransactionImpl : ITransaction
    {
        /// <summary>
        ///    Gets the DateTime that the Transaction occurred.
        ///  </summary>
        public DateTime SettlementDate { get; protected set; }

        /// <summary>
        ///     The unique identifier of this transaction.
        /// </summary>
        public Guid Id { get; protected set; }

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public virtual bool Equals(ITransaction other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return SettlementDate == other.SettlementDate;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="obj"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="obj">An object to compare with this object.</param>
        public override bool Equals(object obj)
        {
            return Equals(obj as ITransaction);
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
            return SettlementDate.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(TransactionImpl left, TransactionImpl right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TransactionImpl left, TransactionImpl right)
        {
            return !Equals(left, right);
        }

        #endregion

        protected Guid CalculateTransactionID(Guid factoryGuid)
        {
            var hashCode = GetHashCode();
            unchecked
            {
                hashCode = (hashCode*397) ^ factoryGuid.GetHashCode();
            }
            var random = new Random(hashCode);
            var guid = new byte[16];
            random.NextBytes(guid);

            return new Guid(guid);
        }
    }
}
