﻿using System;
using System.Diagnostics;
using System.Threading;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// A trading account which simulates the execution of orders.
    /// </summary>
    public class SimulatedTradingAccount : AsynchronousTradingAccount
    {
        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        /// <param name="token"></param>
        protected override void ProcessOrder(Order order, CancellationToken token)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // simulate a delay in processing
            DelayProcessing();
            stopwatch.Stop();

            if (token.IsCancellationRequested) InvokeOrderCancelled(new OrderCancelledEventArgs(DateTime.Now, order));

            var now = order.Issued.Add(stopwatch.Elapsed);
            if (now <= order.Expiration)
            {

                // fill the order at 1% higher price
                var price = Math.Round(order.Price*1.01m, 2);
                var commission = Features.CommissionSchedule.PriceCheck(order);
                var transaction = TransactionFactory.ConstructShareTransaction(order.OrderType, order.Ticker, now, order.Shares, price, commission);

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