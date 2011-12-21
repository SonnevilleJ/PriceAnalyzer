using System;
using Sonneville.PriceTools.Trading;

namespace TradingTest
{
    public class ErroneousSimulatedAccount : BacktestSimulator
    {
        protected override void ProcessOrder(Order order)
        {
            throw new NotSupportedException();
        }
    }
}