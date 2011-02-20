using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for an <see cref="ICashAccount"/>.
    /// </summary>
    public abstract partial class CashTransaction : ICashTransaction
    {
        #region Constructors

        /// <summary>
        /// Constructs a CashTransaction.
        /// </summary>
        protected internal CashTransaction()
        {
        }

        #endregion

        #region Implementation of ITransaction

        /// <summary>
        ///   Gets the <see cref = "PriceTools.OrderType" /> of this CashTransaction.
        /// </summary>
        public OrderType OrderType
        {
            get { return (OrderType) TransactionType; }
            protected set { TransactionType = (Int32) value; }
        }

        #endregion

        /// <summary>
        /// Ensure Amount is positive for Deposit and negative for Withdrawal.
        /// </summary>
        partial void OnAmountChanged()
        {
            if ((OrderType == OrderType.Deposit && Amount < 0) || (OrderType == OrderType.Withdrawal && Amount > 0))
            {
                Amount = -Amount;
            }
        }

        #region Equality Checks

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator ==(CashTransaction left, CashTransaction right)
        {
            if (ReferenceEquals(null, left)) return false;
            if (ReferenceEquals(null, right)) return false;

            return (left.OrderType == right.OrderType &&
                    left.SettlementDate == right.SettlementDate &&
                    left.Amount == right.Amount);
        }

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator !=(CashTransaction left, CashTransaction right)
        {
            return !(left == right);
        }

        #endregion

        #region Implementation of IEquatable<ITransaction>

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
            if (obj as CashTransaction == null) return false;
            return this == (CashTransaction) obj;
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
                int result = SettlementDate.GetHashCode();
                result = (result * 397) ^ Amount.GetHashCode();
                result = (result * 397) ^ OrderType.GetHashCode();
                return result;
            }
        }

        #endregion
    }
}
