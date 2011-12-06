using System;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// Processes orders from a <see cref="TradingStrategy"/> and forwards them to a <see cref="TradingAccount"/>.
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

        protected OrderExecutor(TradingStrategy strategy, TradingAccount account)
        {
            Strategy = strategy;
            Account = account;

            Account.OrderFilled += OrderFilled;

            Strategy.CancelAllOrders += CancelAllOrders;
            Strategy.Buy += Buy;
            Strategy.SellShort += SellShort;
            Strategy.Sell += Sell;
            Strategy.BuyToCover += BuyToCover;
        }

        /// <summary>
        /// Receives notification that an order has been filled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OrderFilled(object sender, OrderInfo e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles an order to cancel all unfilled orders.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CancelAllOrders(object sender, OrderInfo e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles an order to buy shares, thereby opening a long position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Buy(object sender, OrderInfo e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles an order to sell shares, thereby closing a long position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Sell(object sender, OrderInfo e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles an order to sell shares short, thereby opening a short position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SellShort(object sender, OrderInfo e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles an order to buy shares to cover a short position, thereby closing a short position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BuyToCover(object sender, OrderInfo e)
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
        public TradingAccount Account { get; private set; }
    }
}
