using System;
using System.Globalization;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction for cash.
    /// </summary>
    [Serializable]
    public abstract class CashTransaction : Transaction, IEquatable<CashTransaction>
    {
        #region Constructors

        /// <summary>
        /// Constructs a CashTransaction with a given SettlemendDate and Amount.
        /// </summary>
        /// <param name="factoryGuid"></param>
        /// <param name="settlementDate"></param>
        /// <param name="amount"></param>
        protected CashTransaction(Guid factoryGuid, DateTime settlementDate, decimal amount)
        {
            SettlementDate = settlementDate;
            Amount = amount;
            Id = GuidSeeder.SeedGuid(GetHashCode(), factoryGuid);
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
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
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
            var result = base.GetHashCode();
            result = (result*397) ^ Amount.GetHashCode();
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

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0} {1:c} on {2}", GetType().Name.ToUpperInvariant(), Amount, SettlementDate);
        }
    }
}
