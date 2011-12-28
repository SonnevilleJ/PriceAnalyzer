using System;
using System.Diagnostics;
using System.Threading;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A trading account which simulates the execution of orders.
    /// </summary>
    public class SimulatedTradingAccount : SynchronousTradingAccount
    {
        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        protected override void ProcessOrder(Order order)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // simulate a delay in processing
            DelayProcessing();
            stopwatch.Stop();

            var now = order.Issued.Add(stopwatch.Elapsed);
            if (now <= order.Expiration)
            {

                // fill the order at 1% higher price
                var price = Math.Round(order.Price*1.01m, 2);
                var commission = Features.CommissionSchedule.PriceCheck(order);
                var transaction = TransactionFactory.CreateShareTransaction(now, order.OrderType, order.Ticker, price, order.Shares, commission);

                // signal the order has been filled
                InvokeOrderFilled(new OrderExecutedEventArgs(now, order, transaction));
            }
            else
            {
                InvokeOrderExpired(new OrderExpiredEventArgs(order));
            }
        }

        private static void DelayProcessing()
        {
            var timeout = new TimeSpan(0, 0, 0, 0, 100);
            Thread.Sleep(timeout);
            Thread.Sleep(new Random().Next(timeout.Milliseconds));
        }
    }
}