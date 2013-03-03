using System;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// Contains event data for an Order excution.
    /// </summary>
    public sealed class OrderExecutedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new OrderExecutedEventArgs object.
        /// </summary>
        /// <param name="executed">The DateTime at which the Order was executed.</param>
        /// <param name="order">The Order which was executed.</param>
        /// <param name="transaction">The resulting ShareTransaction from the execution of the order.</param>
        public OrderExecutedEventArgs(DateTime executed, IOrder order, IShareTransaction transaction)
        {
            Executed = executed;
            Order = order;
            Transaction = transaction;
            Validate();
        }

        /// <summary>
        /// The DateTime at which the order was executed.
        /// </summary>
        public DateTime Executed { get; private set; }

        /// <summary>
        /// The Order which was executed.
        /// </summary>
        public IOrder Order { get; private set; }

        /// <summary>
        /// The resulting ShareTransaction from the execution of the order.
        /// </summary>
        public IShareTransaction Transaction { get; private set; }

        private void Validate()
        {
            if (Executed > Order.Expiration)
            {
                throw new InvalidOperationException("Execution date must be earlier than or equal to the Order expiration date.");
            }
        }
    }
}