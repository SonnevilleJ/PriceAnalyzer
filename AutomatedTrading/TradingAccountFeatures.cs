namespace Sonneville.PriceTools.AutomatedTrading
{
    public class TradingAccountFeatures
    {
        internal TradingAccountFeatures(OrderType supportedOrderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            SupportedOrderTypes = supportedOrderTypes;
            CommissionSchedule = commissionSchedule;
            MarginSchedule = marginSchedule;
        }

        public OrderType SupportedOrderTypes { get; private set; }

        public ICommissionSchedule CommissionSchedule { get; private set; }

        public bool IsMarginAccount { get { return MarginSchedule.IsMarginAllowed; } }

        public IMarginSchedule MarginSchedule { get; private set; }

        public bool Supports(OrderType orderType)
        {
            return (SupportedOrderTypes & orderType) == orderType;
        }
    }
}
