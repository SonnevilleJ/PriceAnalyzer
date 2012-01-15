namespace Sonneville.PriceTools.AutomatedTrading
{
    public static class TradingAccountFeaturesFactory
    {
        private const OrderType CashOnlyOrderTypes = OrderType.Deposit | OrderType.Withdrawal;
        private const OrderType BasicOrderTypes = CashOnlyOrderTypes | OrderType.Buy | OrderType.Sell;
        private const OrderType ShortOrderTypes = CashOnlyOrderTypes | OrderType.SellShort | OrderType.BuyToCover;
        private const OrderType FullOrderTypes = BasicOrderTypes | ShortOrderTypes;
        private const decimal DefaultPrice = 9.95m;

        public static TradingAccountFeatures CreateBasicTradingAccountFeatures()
        {
            return CreateTradingAccountFeatures(BasicOrderTypes);
        }

        public static TradingAccountFeatures CreateShortTradingAccountFeatures()
        {
            return CreateTradingAccountFeatures(ShortOrderTypes);
        }

        public static TradingAccountFeatures CreateFullTradingAccountFeatures()
        {
            return CreateTradingAccountFeatures(FullOrderTypes);
        }

        public static TradingAccountFeatures CreateTradingAccountFeatures(OrderType orderTypes)
        {
            return CreateTradingAccountFeatures(orderTypes, new FlatCommissionSchedule(DefaultPrice));
        }

        public static TradingAccountFeatures CreateTradingAccountFeatures(OrderType orderTypes, ICommissionSchedule commissionSchedule)
        {
            return CreateTradingAccountFeatures(orderTypes, commissionSchedule, new MarginNotAllowed());
        }

        public static TradingAccountFeatures CreateTradingAccountFeatures(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            return new TradingAccountFeatures(orderTypes, commissionSchedule, marginSchedule);
        }
    }
}