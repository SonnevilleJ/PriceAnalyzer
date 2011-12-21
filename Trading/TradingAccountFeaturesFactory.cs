namespace Sonneville.PriceTools.Trading
{
    public class TradingAccountFeaturesFactory
    {
        private const OrderType CashAccount = OrderType.Deposit | OrderType.Withdrawal;
        private const OrderType Basic = CashAccount | OrderType.Buy | OrderType.Sell;
        private const OrderType Short = Basic | OrderType.SellShort | OrderType.BuyToCover;

        internal TradingAccountFeaturesFactory()
        {
        }

        public TradingAccountFeatures CreateBasicTradingAccountFeatures()
        {
            return new TradingAccountFeatures(Basic);
        }

        public TradingAccountFeatures CreateShortTradingAccountFeatures()
        {
            return new TradingAccountFeatures(Short);
        }

        public TradingAccountFeatures CreateCustomTradingAccountFeatures(OrderType orderTypes)
        {
            return new TradingAccountFeatures(orderTypes);
        }
    }
}