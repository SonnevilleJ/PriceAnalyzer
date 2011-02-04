using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash withdrawal from an <see cref="IPortfolio"/>.
    /// </summary>
    public sealed partial class Withdrawal : CashTransaction
    {
        #region Constructors

        private Withdrawal()
        {}

        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Withdrawal.</param>
        /// <param name="amount">The amount of cash withdrawn.</param>
        public Withdrawal(DateTime dateTime, decimal amount)
            : base(dateTime, PriceTools.OrderType.Withdrawal, amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, "Amount of withdrawal must be greater than zero.");
            }
        }

        #endregion

        #region ISerializable Implementation

        private Withdrawal(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        /// <summary>
        ///   Gets the <see cref = "PriceTools.OrderType" /> of this CashTransaction.
        /// </summary>
        public override OrderType OrderType
        {
            get { return OrderType.Withdrawal; }
        }

        #endregion
    }
}