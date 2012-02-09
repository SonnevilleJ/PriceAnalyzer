using System;
using System.Threading;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class SimulatedTradingAccount : BacktestingTradingAccount
    {
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