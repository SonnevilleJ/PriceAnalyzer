using System;
using Sonneville.PriceTools.Trading;

namespace Sonneville.TradingTest
{
    public class ErroneousSimulatedAccount : BacktestSimulator
    {
        public ErroneousSimulatedAccount()
            : base(TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures())
        {
        }

        protected override void ProcessOrder(Order order)
        {
            throw new NotSupportedException();
        }
    }
}