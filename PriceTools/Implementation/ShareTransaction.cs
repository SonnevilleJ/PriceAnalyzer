using System;
using System.Globalization;

namespace Sonneville.PriceTools.Implementation
{
    public interface IShareTransaction : ITransaction, IEquatable<IShareTransaction>
    {
        string Ticker { get; }

        decimal Shares { get; }

        decimal Price { get; }

        decimal Commission { get; }

        decimal TotalValue { get; }

        DateTime SettlementDate { get; }

        Guid Id { get; }
    }

    [Serializable]
    public abstract class ShareTransaction : Transaction, IShareTransaction
    {
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

        public string Ticker { get; private set; }

        public decimal Shares { get; private set; }

        public decimal Price { get; private set; }

        public decimal Commission { get; private set; }

        public virtual decimal TotalValue
        {
            get { return Math.Round(Price * Shares, 2) + Commission; }
        }

        #region Equality

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

        public override bool Equals(object obj)
        {
            return Equals(obj as ShareTransaction);
        }

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

        public static bool operator ==(ShareTransaction left, ShareTransaction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ShareTransaction left, ShareTransaction right)
        {
            return !Equals(left, right);
        }

        #endregion

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0} {1} shares of {2} on {3} for {4:c} with {5:c} commission", GetType().Name.ToUpperInvariant(), Shares, Ticker, SettlementDate, Price, Commission);
        }
    }
}