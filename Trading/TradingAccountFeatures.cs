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

        /// <summary>
        /// Gets the <see cref="OrderType"/>s supported by the <see cref="TradingAccount"/>.
        /// </summary>
        public OrderType SupportedOrderTypes { get; private set; }

        /// <summary>
        /// Gets the <see cref="ICommissionSchedule"/> used by the <see cref="TradingAccount"/>.
        /// </summary>
        public ICommissionSchedule CommissionSchedule { get; private set; }

        /// <summary>
        /// Gets a value indicating if a particular <see cref="OrderType"/> is supported by the <see cref="TradingAccount"/>.
        /// </summary>
        /// <param name="orderType">The desired <see cref="OrderType"/>.</param>
        /// <returns>A value indicating if the <see cref="OrderType"/> is supported.</returns>
        public bool Supports(OrderType orderType)
        {
            return (SupportedOrderTypes & orderType) == orderType;
        }
    }
}
