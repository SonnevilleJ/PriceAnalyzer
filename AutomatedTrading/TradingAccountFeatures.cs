using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class TradingAccountFeatures
    {
        #region Constructors

        internal TradingAccountFeatures(OrderType supportedOrderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            SupportedOrderTypes = supportedOrderTypes;
            CommissionSchedule = commissionSchedule;
            MarginSchedule = marginSchedule;
        }

        #endregion

        /// <summary>
        /// Gets the <see cref="OrderType"/>s supported by the <see cref="TradingAccountImpl"/>.
        /// </summary>
        public OrderType SupportedOrderTypes { get; private set; }

        /// <summary>
        /// Gets the <see cref="ICommissionSchedule"/> used by the <see cref="TradingAccountImpl"/>.
        /// </summary>
        public ICommissionSchedule CommissionSchedule { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the TradingAccount supports margin trading.
        /// </summary>
        public bool IsMarginAccount { get { return (SupportedOrderTypes & OrderType.SellShort) == OrderType.SellShort; } }

        public IMarginSchedule MarginSchedule { get; private set; }

        /// <summary>
        /// Gets a value indicating if a particular <see cref="OrderType"/> is supported by the <see cref="TradingAccountImpl"/>.
        /// </summary>
        /// <param name="orderType">The desired <see cref="OrderType"/>.</param>
        /// <returns>A value indicating if the <see cref="OrderType"/> is supported.</returns>
        public bool Supports(OrderType orderType)
        {
            return (SupportedOrderTypes & orderType) == orderType;
        }
    }
}
