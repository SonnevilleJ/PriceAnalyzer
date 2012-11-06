using System;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.Fidelity
{
    public class FidelityBrokerage : IBrokerage
    {
        /// <summary>
        /// Collects credentials from the user and logs the user into the brokerage.
        /// </summary>
        public void LogIn()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs the user into the brokerage.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void LogIn(string username, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the <see cref="TradingAccountImpl"/> associated with the user's brokerage account.
        /// </summary>
        /// <returns>The <see cref="TradingAccountImpl"/> associated with the user's brokerage account.</returns>
        public TradingAccount GetTradingAccount()
        {
            var commissionSchedule = GetCommissionSchedule();
            var marginSchedule = GetMarginSchedule();
            var features = TradingAccountFeaturesFactory.ConstructTradingAccountFeatures(GetSupportedOrderTypes(), commissionSchedule, marginSchedule);
            var tradingAccount = new FidelityTradingAccount {Features = features};
            return tradingAccount;
        }

        private IMarginSchedule GetMarginSchedule()
        {
            throw new NotImplementedException();
        }

        private FidelityCommissionSchedule GetCommissionSchedule()
        {
            return new FidelityCommissionSchedule();
        }

        private OrderType GetSupportedOrderTypes()
        {
            const OrderType cash = OrderType.Deposit | OrderType.Withdrawal;
            const OrderType basic = cash | OrderType.Buy | OrderType.Sell;
            const OrderType full = basic | OrderType.SellShort | OrderType.BuyToCover;

            return basic;
        }
    }
}