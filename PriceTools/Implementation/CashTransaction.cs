using System;
using System.Globalization;

namespace Sonneville.PriceTools.Implementation
{
    [Serializable]
    public abstract class CashTransaction : Transaction, IEquatable<CashTransaction>
    {
        protected CashTransaction(Guid factoryGuid, DateTime settlementDate, decimal amount)
        {
            SettlementDate = settlementDate;
            Amount = amount;
            Id = GuidSeeder.SeedGuid(GetHashCode(), factoryGuid);
        }

        public decimal Amount { get; private set; }

        #region Equality

        public bool Equals(CashTransaction other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return base.Equals(other) &&
                Amount == other.Amount;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CashTransaction);
        }

        public override int GetHashCode()
        {
            var result = base.GetHashCode();
            result = (result*397) ^ Amount.GetHashCode();
            return result;
        }

        public static bool operator ==(CashTransaction left, CashTransaction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CashTransaction left, CashTransaction right)
        {
            return !Equals(left, right);
        }

        #endregion

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0} {1:c} on {2}", GetType().Name.ToUpperInvariant(), Amount, SettlementDate);
        }
    }
}
