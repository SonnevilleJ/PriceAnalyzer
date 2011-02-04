using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash deposit to an <see cref="IPortfolio"/>.
    /// </summary>
    public sealed partial class Deposit : CashTransaction
    {
        #region Constructors

        private Deposit()
        {}

        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        /// <param name="dateTime">The DateTime of the Deposit.</param>
        /// <param name="amount">The amount of cash deposited.</param>
        public Deposit(DateTime dateTime, decimal amount)
            : base(dateTime, PriceTools.OrderType.Deposit, amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, "Amount of deposit must be greater than zero.");
            }
        }

        #endregion

        #region ISerializable Implementation

        private Deposit(SerializationInfo info, StreamingContext context)
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
            get { return OrderType.Deposit; }
        }

        #endregion
    }
}