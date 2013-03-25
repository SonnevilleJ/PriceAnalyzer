using System;
using System.Threading;

namespace Sonneville.PriceTools.AutomatedTrading.Implementation
{
    internal class SimulatedTradingAccountImpl : BacktestingTradingAccountImpl
    {
        public SimulatedTradingAccountImpl(Guid brokerageGuid, string accountNumber) : base(brokerageGuid, accountNumber)
        {
        }

        protected override void ProcessOrder(IOrder order, CancellationToken token)
        {
            DelayProcessing();
            base.ProcessOrder(order, token);
        }

        private static void DelayProcessing()
        {
            var timeout = new TimeSpan(0, 0, 0, 0, 100);
            Thread.Sleep(timeout);
            Thread.Sleep(new Random().Next(timeout.Milliseconds));
        }
    }
}