namespace Sonneville.PriceTools.Trading
{
    public static class TradingAccountFeaturesFactory
    {
        private const OrderType CashAccount = OrderType.Deposit | OrderType.Withdrawal;
        private const OrderType Basic = CashAccount | OrderType.Buy | OrderType.Sell;
        private const OrderType Short = CashAccount | OrderType.SellShort | OrderType.BuyToCover;
        private const OrderType Full = Basic | Short;

        public static TradingAccountFeatures CreateBasicTradingAccountFeatures()
        {
            return new TradingAccountFeatures(Basic);
        }

        public static TradingAccountFeatures CreateShortTradingAccountFeatures()
        {
            return new TradingAccountFeatures(Short);
        }

        public static TradingAccountFeatures CreateFullTradingAccountFeatures()
        {
            return new TradingAccountFeatures(Full);
        }

        public static TradingAccountFeatures CreateCustomTradingAccountFeatures(OrderType orderTypes)
        {
            return new TradingAccountFeatures(orderTypes);
        }
    }
}