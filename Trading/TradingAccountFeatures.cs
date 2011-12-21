namespace Sonneville.PriceTools.Trading
{
    public class TradingAccountFeatures
    {
        #region Factory and Constructors

        private static readonly TradingAccountFeaturesFactory Instance = new TradingAccountFeaturesFactory();
        public static TradingAccountFeaturesFactory Factory { get { return Instance; } }

        internal TradingAccountFeatures(OrderType supportedOrderTypes)
        {
            SupportedOrderTypes = supportedOrderTypes;
        }

        #endregion

        public OrderType SupportedOrderTypes { get; private set; }

        public bool Supports(OrderType orderType)
        {
            return (SupportedOrderTypes & orderType) == orderType;
        }
    }
}
