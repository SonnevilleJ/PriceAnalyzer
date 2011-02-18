namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for an <see cref="ICashAccount"/>.
    /// </summary>
    public abstract partial class CashTransaction : ICashTransaction
    {
        #region Private Members

        private OrderType _type;

        #endregion

        #region Constructors

        protected internal CashTransaction()
        {
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
            return left.OrderType == right.OrderType &&
                   left.SettlementDate == right.SettlementDate &&
                   left.Amount == right.Amount;
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
                return (SettlementDate.GetHashCode() * 397) ^ Amount.GetHashCode();
            }
        }

        #endregion

        #region Implementation of ITransaction

        /// <summary>
        ///   Gets the <see cref = "PriceTools.OrderType" /> of this CashTransaction.
        /// </summary>
        public OrderType OrderType
        {
            get { return _type; }
            protected set { _type = value; }
        }

        #endregion

        partial void OnAmountChanged()
        {
            // ensure Amount is positive for Deposit and negative for Withdrawal
            if ((OrderType == OrderType.Deposit && Amount < 0) || (OrderType == OrderType.Withdrawal && Amount > 0))
            {
                Amount = -Amount;
            }
        }
    }
}
