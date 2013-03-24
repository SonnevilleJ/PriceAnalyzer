﻿using System;
using System.Threading;

namespace Sonneville.PriceTools.AutomatedTrading.Implementation
{
    /// <summary>
    /// A trading account which simulates the execution of orders.
    /// </summary>
    internal class BacktestingTradingAccountImpl : TradingAccountImpl
    {
        private readonly ITransactionFactory _transactionFactory;

        internal BacktestingTradingAccountImpl()
        {
            MaximumSlippage = 0.01m;
            _transactionFactory = new TransactionFactory();
        }

        /// <summary>
        /// Specifies the maximum amount of price slippage when processing orders.
        /// </summary>
        /// <remarks>Slippage of 0 means trades execute at the exact price specified by the order.
        /// Slippage of 1 means trades can execute 100% more or less than the price specified by the order.</remarks>
        public decimal MaximumSlippage { get; set; }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="IOrder"/> to execute.</param>
        /// <param name="token"></param>
        protected override void ProcessOrder(IOrder order, CancellationToken token)
        {
            if (token.IsCancellationRequested) InvokeOrderCancelled(new OrderCancelledEventArgs(DateTime.Now, order));

            var now = order.Issued.Add(new TimeSpan(new Random().Next(1, 100)));
            if (now <= order.Expiration)
            {
                // fill the order with slippage
                var price = Math.Round(order.Price*(1 + CalculateSlippage()), 2);
                var commission = Features.CommissionSchedule.PriceCheck(order);
                var transaction = _transactionFactory.ConstructShareTransaction(order.OrderType, order.Ticker, now, order.Shares, price, commission);

                // signal the order has been filled
                InvokeOrderFilled(new OrderExecutedEventArgs(now, order, transaction));
            }
            else
            {
                InvokeOrderExpired(new OrderExpiredEventArgs(order));
            }
        }

        private decimal CalculateSlippage()
        {
            var randomInt = new Random().Next(int.MinValue, int.MaxValue);
            const decimal maxValue = (decimal) int.MaxValue;
            var randomPercentage = (randomInt/maxValue);

            return randomPercentage*MaximumSlippage;
        }
    }
}