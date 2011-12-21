using System;
using System.Threading;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace TradingTest
{
    /// <summary>
    /// A trading account which simulates the execution of orders.
    /// </summary>
    public class BacktestSimulator : TradingAccount
    {
        private decimal _commission = 5.00m;

        internal static TimeSpan MinProcessingTimeSpan
        {
            get { return new TimeSpan(0, 0, 0, 0, 100); }
        }

        internal static TimeSpan MaxProcessingTimeSpan
        {
            get { return new TimeSpan(0, 0, 0, 0, 500); }
        }

        /// <summary>
        /// Brokerage commission charged for all orders.
        /// </summary>
        public decimal Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        protected override void ProcessOrder(Order order)
        {
            // simulate a delay in processing
            DelayProcessing();

            var executed = DateTime.Now;
            if (executed <= order.Expiration)
            {
                // fill the order at 1% higher price
                var price = Math.Round(order.Price*1.01m, 2);
                var transaction = TransactionFactory.Instance.CreateShareTransaction(executed, order.OrderType, order.Ticker, price, order.Shares, Commission);

                // signal the order has been filled
                InvokeOrderFilled(new OrderExecutedEventArgs(executed, order, transaction));
            }
            else
            {
                InvokeOrderExpired(new OrderExpiredEventArgs(order));
            }
        }

        private static void DelayProcessing()
        {
            Thread.Sleep(MinProcessingTimeSpan);
            Thread.Sleep(new Random().Next((MaxProcessingTimeSpan - MinProcessingTimeSpan).Milliseconds));
        }
    }
}