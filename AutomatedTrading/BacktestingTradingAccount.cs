using System;
using System.Threading;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// A trading account which simulates the execution of orders.
    /// </summary>
    public class BacktestingTradingAccount : TradingAccountImpl
    {
        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        /// <param name="token"></param>
        protected override void ProcessOrder(Order order, CancellationToken token)
        {
            if (token.IsCancellationRequested) InvokeOrderCancelled(new OrderCancelledEventArgs(DateTime.Now, order));

            var now = order.Issued.Add(new TimeSpan(new Random().Next(1, 100)));
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
    }
}