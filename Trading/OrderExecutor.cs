using System;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// Processes orders from a <see cref="TradingStrategy"/> and forwards them to a <see cref="SynchronousTradingAccount"/>.
    /// </summary>
    public abstract class OrderExecutor : IDisposable
    {
        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }

        ~OrderExecutor()
        {
            Dispose(true);
        }

        #endregion

        protected OrderExecutor(TradingStrategy strategy, SynchronousTradingAccount account)
        {
            Strategy = strategy;
            Account = account;

            Account.OrderFilled += OnOrderFilled;

            Strategy.CancelAllOrders += OnAllOrdersCancelled;
            Strategy.SubmitOrder += OnOrderSignaled;
        }

        /// <summary>
        /// Handles a signal to submit an order.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrderSignaled(object sender, OrderExecutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives notification that an order has been filled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrderFilled(object sender, OrderExecutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives notification that an order has expired.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOrderExpired(object sender, OrderExecutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles an order to cancel all unfilled orders.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllOrdersCancelled(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initiates automated trading.
        /// </summary>
        public void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Suspends automated trading.
        /// </summary>
        /// <remarks>This does not close any open positions.</remarks>
        public void Stop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The <see cref="TradingStrategy"/> used to generate order signals.
        /// </summary>
        public TradingStrategy Strategy { get; private set; }

        /// <summary>
        /// The trading account to use for automated trading.
        /// </summary>
        public SynchronousTradingAccount Account { get; private set; }
    }
}
