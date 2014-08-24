using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface ITradingAccount
    {
        IBrokerage Brokerage { get; }

        IPortfolio Portfolio { get; }

        string AccountNumber { get; }

        TradingAccountFeatures Features { get; }

        void Submit(Order order);

        void TryCancelOrder(Order order);
    }
}