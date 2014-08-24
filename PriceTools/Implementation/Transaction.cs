using System;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Implementation
{
    public interface ITransaction
    {
        DateTime SettlementDate { get; }

        Guid Id { get; }

        bool Equals(ITransaction other);

        bool Equals(object obj);

        int GetHashCode();
    }

    [Serializable]
    public abstract class Transaction : IEquatable<ITransaction>, ITransaction
    {
        protected Transaction()
        {
            GuidSeeder = new GuidSeeder();
        }

        public DateTime SettlementDate { get; protected set; }

        public Guid Id { get; protected set; }

        protected GuidSeeder GuidSeeder { get; private set; }

        #region Equality

        public virtual bool Equals(ITransaction other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return SettlementDate == other.SettlementDate;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ITransaction);
        }

        public override int GetHashCode()
        {
            var result = GetType().GetHashCode();
            result = (result * 397) ^ SettlementDate.GetHashCode();
            return result;
        }

        public static bool operator ==(Transaction left, Transaction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Transaction left, Transaction right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
