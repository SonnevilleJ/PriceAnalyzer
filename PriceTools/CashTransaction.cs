using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for an <see cref="CashAccount"/>.
    /// </summary>
    [Serializable]
    public abstract class CashTransaction : Transaction, IEquatable<CashTransaction>
    {
        #region Constructors
        
        /// <summary>
        /// Constructs a CashTransaction with a given SettlemendDate and Amount.
        /// </summary>
        /// <param name="settlementDate"></param>
        /// <param name="amount"></param>
        protected internal CashTransaction(DateTime settlementDate, decimal amount)
        {
            SettlementDate = settlementDate;
            Amount = amount;
        }

        #endregion

        #region Implementation of CashTransaction

        /// <summary>
        ///   Gets the amount of cash in this CashTransaction.
        /// </summary>
        public decimal Amount { get; private set; }

        #endregion

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(CashTransaction other)
        {
            return base.Equals(other) &&
                Amount == other.Amount;
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
            return Equals(obj as CashTransaction);
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
            var result = Amount.GetHashCode();
            result = (result*397) ^ SettlementDate.GetHashCode();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(CashTransaction left, CashTransaction right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(CashTransaction left, CashTransaction right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
