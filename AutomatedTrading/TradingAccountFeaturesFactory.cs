namespace Sonneville.PriceTools.AutomatedTrading
{
    public static class TradingAccountFeaturesFactory
    {
        private const OrderType CashOnlyOrderTypes = OrderType.Deposit | OrderType.Withdrawal;
        private const OrderType BasicOrderTypes = CashOnlyOrderTypes | OrderType.Buy | OrderType.Sell;
        private const OrderType ShortOrderTypes = CashOnlyOrderTypes | OrderType.SellShort | OrderType.BuyToCover;
        private const OrderType FullOrderTypes = BasicOrderTypes | ShortOrderTypes;
        private const decimal DefaultPrice = 9.95m;

        public static TradingAccountFeatures ConstructBasicTradingAccountFeatures()
        {
            return ConstructTradingAccountFeatures(BasicOrderTypes);
        }

        public static TradingAccountFeatures ConstructShortTradingAccountFeatures()
        {
            return ConstructTradingAccountFeatures(ShortOrderTypes);
        }

        public static TradingAccountFeatures ConstructFullTradingAccountFeatures()
        {
            return ConstructTradingAccountFeatures(FullOrderTypes);
        }

        public static TradingAccountFeatures ConstructTradingAccountFeatures(OrderType orderTypes)
        {
            return ConstructTradingAccountFeatures(orderTypes, new FlatCommissionSchedule(DefaultPrice));
        }

        public static TradingAccountFeatures ConstructTradingAccountFeatures(OrderType orderTypes, ICommissionSchedule commissionSchedule)
        {
            return ConstructTradingAccountFeatures(orderTypes, commissionSchedule, new MarginNotAllowed());
        }

        public static TradingAccountFeatures ConstructTradingAccountFeatures(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            return new TradingAccountFeatures(orderTypes, commissionSchedule, marginSchedule);
        }
    }
}