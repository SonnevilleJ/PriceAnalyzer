using System;
using System.Diagnostics;
using System.Threading;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace Sonneville.TradingTest
{
    /// <summary>
    /// A trading account which simulates the execution of orders.
    /// </summary>
    public class BacktestSimulator : TradingAccount
    {
        private static readonly TradingAccountFeatures TradingAccountFeatures = TradingAccountFeatures.Factory.CreateFullTradingAccountFeatures();

        public BacktestSimulator()
            : this(5.00m)
        {
        }

        public BacktestSimulator(decimal commission)
            : base(TradingAccountFeatures)
        {
            Commission = commission;
        }

        internal static TimeSpan MinProcessingTimeSpan
        {
            get { return new TimeSpan(0, 0, 0, 0, 100); }
        }

        internal static TimeSpan MaxProcessingTimeSpan
        {
            get { return new TimeSpan(0, 0, 0, 1, 0); }
        }

        /// <summary>
        /// Brokerage commission charged for all orders.
        /// </summary>
        public decimal Commission { get; set; }

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
                var transaction = TransactionFactory.Instance.CreateShareTransaction(now, order.OrderType, order.Ticker, price, order.Shares, Commission);

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
            Thread.Sleep(MinProcessingTimeSpan);
            Thread.Sleep(new Random().Next((MaxProcessingTimeSpan - MinProcessingTimeSpan).Milliseconds));
        }
    }
}