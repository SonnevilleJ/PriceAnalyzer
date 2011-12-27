using System;
using System.Threading;
using Sonneville.PriceTools.Trading;

namespace Sonneville.TradingTest
{
    public class ErroneousSimulatedAccount : TradingAccount
    {
        public ErroneousSimulatedAccount()
            : base(TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures())
        {
        }

        protected override void ProcessOrder(Order order, CancellationToken token)
        {
            throw new NotSupportedException();
        }
    }
}