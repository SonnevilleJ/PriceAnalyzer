namespace Sonneville.PriceTools.Trading
{
    public class TradingAccountFeaturesFactory
    {
        private const OrderType CashAccount = OrderType.Deposit | OrderType.Withdrawal;
        private const OrderType Basic = CashAccount | OrderType.Buy | OrderType.Sell;
        private const OrderType Short = CashAccount | OrderType.SellShort | OrderType.BuyToCover;
        private const OrderType Full = Basic | Short;

        internal TradingAccountFeaturesFactory()
        {
        }

// ReSharper disable MemberCanBeMadeStatic.Global
        public TradingAccountFeatures CreateBasicTradingAccountFeatures()
        {
            return new TradingAccountFeatures(Basic);
        }

        public TradingAccountFeatures CreateShortTradingAccountFeatures()
        {
            return new TradingAccountFeatures(Short);
        }

        public TradingAccountFeatures CreateFullTradingAccountFeatures()
        {
            return new TradingAccountFeatures(Full);
        }

        public TradingAccountFeatures CreateCustomTradingAccountFeatures(OrderType orderTypes)
        {
            return new TradingAccountFeatures(orderTypes);
        }
// ReSharper restore MemberCanBeMadeStatic.Global
    }
}