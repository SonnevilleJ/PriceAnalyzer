using System;
using Sonneville.PriceTools.Trading;

namespace TradingTest
{
    public class ErroneousSimulatedAccount : SimulatedAccount
    {
        protected override void ProcessOrder(Order order)
        {
            throw new NotSupportedException();
        }
    }
}