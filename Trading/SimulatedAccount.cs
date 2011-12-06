using System;
using System.Threading;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A trading account which simulates the execution of orders.
    /// </summary>
    public class SimulatedAccount : TradingAccount
    {
        /// <summary>
        /// $5.00 brokerage commission.
        /// </summary>
        private const decimal Commission = 5.00m;

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        protected override void SubmitOrderImpl(Order order)
        {
            // simulate a delay in processing - up to 10 seconds
            Thread.Sleep(new Random().Next(10000));

            var executed = DateTime.Now;
            if (executed <= order.Expiration)
            {
                // fill the order at 1% higher price
                var price = Math.Round(order.Price*1.01m, 2);
                var transaction = TransactionFactory.CreateShareTransaction(executed, order.OrderType, order.Ticker, price, order.Shares, Commission);

                // signal the order has been filled
                InvokeOrderFilled(new OrderExecutedInfo(executed, order, transaction));
            }
            else
            {
                // todo: InvokeOrderExpired()
            }
        }
    }
}