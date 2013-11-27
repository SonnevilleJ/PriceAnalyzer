using System;
using System.Threading;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading.Implementation
{
    internal class SimulatedTradingAccount : BacktestingTradingAccount
    {
        public SimulatedTradingAccount(Guid brokerageGuid, string accountNumber) : base(brokerageGuid, accountNumber)
        {
        }

        protected override void ProcessOrder(Order order, CancellationToken token)
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