namespace Sonneville.PriceTools.AutomatedTrading
{
    public class TradingAccountFeaturesFactory : ITradingAccountFeaturesFactory
    {
        private const OrderType CashOnlyOrderTypes = OrderType.Deposit | OrderType.Withdrawal;
        private const OrderType BasicOrderTypes = CashOnlyOrderTypes | OrderType.Buy | OrderType.Sell;
        private const OrderType ShortOrderTypes = CashOnlyOrderTypes | OrderType.SellShort | OrderType.BuyToCover;
        private const OrderType FullOrderTypes = BasicOrderTypes | ShortOrderTypes;
        private const decimal DefaultPrice = 9.95m;

        public TradingAccountFeatures ConstructBasicTradingAccountFeatures()
        {
            return ConstructTradingAccountFeatures(BasicOrderTypes);
        }

        public TradingAccountFeatures ConstructShortTradingAccountFeatures()
        {
            return ConstructTradingAccountFeatures(ShortOrderTypes);
        }

        public TradingAccountFeatures ConstructFullTradingAccountFeatures()
        {
            return ConstructTradingAccountFeatures(FullOrderTypes);
        }

        public TradingAccountFeatures ConstructTradingAccountFeatures(OrderType orderTypes)
        {
            return ConstructTradingAccountFeatures(orderTypes, new FlatCommissionSchedule(DefaultPrice));
        }

        public TradingAccountFeatures ConstructTradingAccountFeatures(OrderType orderTypes, ICommissionSchedule commissionSchedule)
        {
            return ConstructTradingAccountFeatures(orderTypes, commissionSchedule, new MarginNotAllowedSchedule());
        }

        public TradingAccountFeatures ConstructTradingAccountFeatures(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            return new TradingAccountFeatures(orderTypes, commissionSchedule, marginSchedule);
        }
    }
}