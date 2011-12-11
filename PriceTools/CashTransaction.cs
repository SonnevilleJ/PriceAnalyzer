using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for an <see cref="ICashAccount"/>.
    /// </summary>
    public abstract class CashTransaction : ICashTransaction
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
        ///   Gets the DateTime that the ITransaction occurred.
        /// </summary>
        public DateTime SettlementDate { get; set; }

        /// <summary>
        ///   Gets the <see cref = "PriceTools.OrderType" /> of this CashTransaction.
        /// </summary>
        public OrderType OrderType { get; protected set; }

        #endregion

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
            return Equals(other as object);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ICashTransaction other)
        {
            return Equals(other as object);
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
            return this == obj as CashTransaction;
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
                // The following code identifies two "equal" objects as the same.
                // However, this causes a glitch where Entity Framework 4.0
                // will not add a duplicate CashTransaction to a CashAccount.
                // Therefore, the fowllowing code has been replaced with the active code.

                //int result = SettlementDate.GetHashCode();
                //result = (result * 397) ^ Amount.GetHashCode();
                //return (result * 397) ^ (int)OrderType;

                return base.GetHashCode();
            }
        }

        #endregion

        #region Implementation of ICashTransaction

        private decimal _amount;

        /// <summary>
        ///   Gets the amount of cash in this ICashTransaction.
        /// </summary>
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                // ensure Amount is negative for Withdrawal
                var amount = Math.Abs(value);
                switch (OrderType)
                {
                    case OrderType.Withdrawal:
                        _amount = -amount;
                        break;
                    default:
                        _amount = amount;
                        break;
                }
            }
        }

        #endregion
    }
}
