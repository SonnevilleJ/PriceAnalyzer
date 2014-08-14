using System;
using System.Globalization;

namespace Sonneville.PriceTools.Implementation
{
    public interface IShareTransaction : ITransaction, IEquatable<IShareTransaction>
    {
        /// <summary>
        ///   Gets the ticker symbol of the security traded in this ShareTransaction.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        ///   Gets the amount of securities traded in this ShareTransaction.
        /// </summary>
        decimal Shares { get; }

        /// <summary>
        ///   Gets the value of all securities traded in this ShareTransaction.
        /// </summary>
        decimal Price { get; }

        /// <summary>
        ///   Gets the commission charged for this ShareTransaction.
        /// </summary>
        decimal Commission { get; }

        /// <summary>
        ///   Gets the total value of this ShareTransaction, including commissions.
        /// </summary>
        decimal TotalValue { get; }

        /// <summary>
        ///    Gets the DateTime that the Transaction occurred.
        ///  </summary>
        DateTime SettlementDate { get; }

        /// <summary>
        ///     The unique identifier of this transaction.
        /// </summary>
        Guid Id { get; }
    }

    /// <summary>
    ///   Represents a transaction for a share of equity.
    /// </summary>
    [Serializable]
    public abstract class ShareTransaction : Transaction, IShareTransaction
    {
        /// <summary>
        /// Constructs a ShareTransaction object.
        /// </summary>
        /// <param name="factoryGuid"></param>
        /// <param name="ticker"></param>
        /// <param name="settlementDate"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <param name="commission"></param>
        protected ShareTransaction(Guid factoryGuid, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
        {
            SettlementDate = settlementDate;
            Ticker = ticker;

            if (shares < 0)
                throw new ArgumentOutOfRangeException("shares", shares, Strings.ShareTransaction_OnSharesChanging_Shares_must_be_greater_than_or_equal_to_0_);
            Shares = shares;

            Price = price;

            if (commission < 0)
                throw new ArgumentOutOfRangeException("commission", commission, Strings.ShareTransaction_Commission_Commission_must_be_greater_than_or_equal_to_0_);
            Commission = commission;

            Id = GuidSeeder.SeedGuid(GetHashCode(), factoryGuid);
        }

        /// <summary>
        ///   Gets the ticker symbol of the security traded in this ShareTransaction.
        /// </summary>
        public string Ticker { get; private set; }

        /// <summary>
        ///   Gets the amount of securities traded in this ShareTransaction.
        /// </summary>
        public decimal Shares { get; private set; }

        /// <summary>
        ///   Gets the value of all securities traded in this ShareTransaction.
        /// </summary>
        public decimal Price { get; private set; }

        /// <summary>
        ///   Gets the commission charged for this ShareTransaction.
        /// </summary>
        public decimal Commission { get; private set; }

        /// <summary>
        ///   Gets the total value of this ShareTransaction, including commissions.
        /// </summary>
        public virtual decimal TotalValue
        {
            get { return Math.Round(Price * Shares, 2) + Commission; }
        }

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IShareTransaction other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return base.Equals(other) &&
                   Ticker == other.Ticker &&
                   Shares == other.Shares &&
                   Price == other.Price &&
                   Commission == other.Commission;
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
            return Equals(obj as ShareTransaction);
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
                var result = base.GetHashCode();
                result = (result*397) ^ (Ticker != null ? Ticker.GetHashCode() : 0);
                result = (result*397) ^ Shares.GetHashCode();
                result = (result*397) ^ Price.GetHashCode();
                result = (result*397) ^ Commission.GetHashCode();
                result = (result*397) ^ SettlementDate.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ShareTransaction left, ShareTransaction right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ShareTransaction left, ShareTransaction right)
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
            return String.Format(CultureInfo.InvariantCulture, "{0} {1} shares of {2} on {3} for {4:c} with {5:c} commission", GetType().Name.ToUpperInvariant(), Shares, Ticker, SettlementDate, Price, Commission);
        }
    }
}