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
            const OrderType orderTypes = Basic;
            return CreateTradingAccountFeatures(orderTypes, new MarginNotAllowed());
        }

        public static TradingAccountFeatures CreateShortTradingAccountFeatures(MarginNotAllowed marginSchedule)
        {
            const OrderType orderTypes = Short;
            return CreateTradingAccountFeatures(orderTypes, marginSchedule);
        }

        public static TradingAccountFeatures CreateFullTradingAccountFeatures(MarginNotAllowed marginSchedule)
        {
            const OrderType orderTypes = Full;
            return CreateTradingAccountFeatures(orderTypes, marginSchedule);
        }

        public static TradingAccountFeatures CreateCustomTradingAccountFeatures(OrderType orderTypes, IMarginSchedule marginSchedule)
        {
            return CreateTradingAccountFeatures(orderTypes, marginSchedule);
        }

        private static TradingAccountFeatures CreateTradingAccountFeatures(OrderType orderTypes, IMarginSchedule marginSchedule)
        {
            return new TradingAccountFeatures(orderTypes, marginSchedule);
        }
    }
}