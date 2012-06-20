using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a transaction (or order) for a financial security.
    /// </summary>
    [Serializable]
    public abstract class ShareTransaction : Transaction, IEquatable<ShareTransaction>
    {
        #region Constructors

        protected internal ShareTransaction(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
        {
            Ticker = ticker;
            SettlementDate = settlementDate;

            if (shares < 0)
                throw new ArgumentOutOfRangeException("shares", shares, Strings.ShareTransaction_OnSharesChanging_Shares_must_be_greater_than_or_equal_to_0_);
            Shares = shares;

            Price = price;

            if (commission < 0)
                throw new ArgumentOutOfRangeException("commission", commission, Strings.ShareTransaction_Commission_Commission_must_be_greater_than_or_equal_to_0_);
            Commission = commission;
        }


        #endregion
        
        #region Accessors

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

        #endregion

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ShareTransaction other)
        {
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
                var result = (Ticker != null ? Ticker.GetHashCode() : 0);
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
    }
}