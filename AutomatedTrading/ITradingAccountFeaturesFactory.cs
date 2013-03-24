namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface ITradingAccountFeaturesFactory
    {
        TradingAccountFeatures ConstructBasicTradingAccountFeatures();
        TradingAccountFeatures ConstructShortTradingAccountFeatures();
        TradingAccountFeatures ConstructFullTradingAccountFeatures();
        TradingAccountFeatures ConstructTradingAccountFeatures(OrderType orderTypes);
        TradingAccountFeatures ConstructTradingAccountFeatures(OrderType orderTypes, ICommissionSchedule commissionSchedule);
        TradingAccountFeatures ConstructTradingAccountFeatures(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule);
    }
}