using System;

namespace Sonneville.PriceTools.Trading
{
    public class ErroneousSimulatedAccount : SimulatedAccount
    {
        protected override void SubmitOrderImpl(Order order)
        {
            throw new NotSupportedException();
        }
    }
}