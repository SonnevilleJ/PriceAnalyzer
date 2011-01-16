using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    [Serializable]
    public class CashAccount : ICashAccount
    {
        #region Private Members

        private readonly string _ticker;
        private readonly List<ITransaction> _cashTransactions;

        #endregion

        #region Constructors

        public CashAccount()
            : this(String.Empty)
        {
        }

        public CashAccount(string ticker)
        {
            _ticker = ticker != null ? ticker : String.Empty;
        }

        /// <summary>
        /// Deconstructs a CashAccount
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CashAccount(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            throw new NotImplementedException();
        }

        #endregion

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        /// <summary>
        /// Deposits cash into the CashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposited into the CashAccount.</param>
        /// <param name="amount">The amount of cash deposited into the CashAccount.</param>
        public void Deposit(DateTime dateTime, decimal amount)
        {
            _cashTransactions.Add(new Deposit(dateTime, amount));
        }

        /// <summary>
        /// Withdraws cash from the ICashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is withdrawn from the CashAccount.</param>
        /// <param name="amount">The amount of cash withdrawn from the CashAccount.</param>
        public void Withdraw(DateTime dateTime, decimal amount)
        {
            _cashTransactions.Add(new Withdrawal(dateTime, amount));
        }

        /// <summary>
        /// Gets the ticker symbol this CashAccount is invested in.
        /// </summary>
        public string Ticker
        {
            get
            {
                return _ticker;
            }
        }

        #region Equality Checks

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ICashAccount other)
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
            if (obj.GetType() != typeof(CashAccount)) return false;
            return this == (CashAccount)obj;
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
                int result = _ticker.GetHashCode();
                result = (result * 397) ^ _cashTransactions.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(CashAccount left, CashAccount right)
        {
            bool tickersMatch = left._ticker == right._ticker;

            bool cashMatches = false;
            if (left._cashTransactions.Count == right._cashTransactions.Count)
            {
                cashMatches = left._cashTransactions.All(transaction => right._cashTransactions.Contains(transaction));
            }

            return tickersMatch && cashMatches;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(CashAccount left, CashAccount right)
        {
            return !(left == right);
        }

        #endregion
    }
}
