using System;
using System.Threading;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace TradingTest
{
    /// <summary>
    /// A trading account which simulates the execution of orders.
    /// </summary>
    public class SimulatedAccount : TradingAccount
    {
        /// <summary>
        /// $5.00 brokerage commission for all orders.
        /// </summary>
        private const decimal Commission = 5.00m;

        internal static TimeSpan MinProcessingTimeSpan
        {
            get { return new TimeSpan(0, 0, 0, 1); }
        }

        internal static TimeSpan MaxProcessingTimeSpan
        {
            get { return new TimeSpan(0, 0, 0, 5); }
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
                var transaction = TransactionFactory.CreateShareTransaction(executed, order.OrderType, order.Ticker, price, order.Shares, Commission);

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