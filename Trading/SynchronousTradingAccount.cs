using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Sonneville.PriceTools.Trading
{
    public abstract class SynchronousTradingAccount : ITradingAccount
    {
        #region Private Members

        private readonly IPortfolio _portfolio = new Portfolio();

        #endregion

        #region Constructors

        protected SynchronousTradingAccount(TradingAccountFeatures tradingAccountFeatures)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of ITradingAccount

        /// <summary>
        /// A list of <see cref="IPosition"/>s currently held in this account.
        /// </summary>
        public ReadOnlyCollection<IPosition> Positions
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the list of features supported by this TradingAccount.
        /// </summary>
        public TradingAccountFeatures Features
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        public virtual void Submit(Order order)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attempts to cancel an <see cref="Order"/> before it is filled.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to attempt to cancel.</param>
        public virtual void TryCancelOrder(Order order)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Blocks the calling thread until all submitted orders are filled, cancelled, or expired.
        /// </summary>
        public virtual void WaitAll()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Events and Invokers

        /// <summary>
        /// Triggered when an order has been filled.
        /// </summary>
        public event EventHandler<OrderExecutedEventArgs> OrderFilled;

        /// <summary>
        /// Triggered when an order has expired.
        /// </summary>
        public event EventHandler<OrderExpiredEventArgs> OrderExpired;

        /// <summary>
        /// Triggered when an order has been cancelled.
        /// </summary>
        public event EventHandler<OrderCancelledEventArgs> OrderCancelled;

        protected void InvokeOrderFilled(OrderExecutedEventArgs e)
        {
            ProcessFill(e.Transaction);
            TriggerFilled(e);
        }

        protected void InvokeOrderExpired(OrderExpiredEventArgs e)
        {
            TriggerExpired(e);
        }

        protected void InvokeOrderCancelled(OrderCancelledEventArgs e)
        {
            TriggerCancelled(e);
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        /// <param name="token"></param>
        protected abstract void ProcessOrder(Order order, CancellationToken token);

        #endregion

        #region Private Methods

        protected bool ValidateOrder(Order order)
        {
            var commission = Features.CommissionSchedule.PriceCheck(order);
            var expectedTransaction = TransactionFactory.Instance.CreateShareTransaction(DateTime.Now, order.OrderType, order.Ticker, order.Price, order.Shares, commission);
            var position = GetPosition(order.Ticker);
            return position.TransactionIsValid(expectedTransaction);
        }

        private IPosition GetPosition(string ticker)
        {
            return Positions.Where(p => p.Ticker == ticker).FirstOrDefault() ?? PositionFactory.CreatePosition(ticker);
        }

        private void ProcessFill(IShareTransaction transaction)
        {
            _portfolio.AddTransaction(transaction);
        }

        private void TriggerFilled(OrderExecutedEventArgs e)
        {
            var handler = OrderFilled;
            if (handler != null) handler(this, e);
        }

        private void TriggerExpired(OrderExpiredEventArgs e)
        {
            var handler = OrderExpired;
            if (handler != null) handler(this, e);
        }

        private void TriggerCancelled(OrderCancelledEventArgs e)
        {
            var handler = OrderCancelled;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}