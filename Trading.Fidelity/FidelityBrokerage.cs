using System;

namespace Sonneville.PriceTools.Trading.Fidelity
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
        /// Gets the <see cref="TradingAccount"/> associated with the user's brokerage account.
        /// </summary>
        /// <returns>The <see cref="TradingAccount"/> associated with the user's brokerage account.</returns>
        public TradingAccount GetTradingAccount()
        {
            var commissionSchedule = GetCommissionSchedule();
            var marginSchedule = GetMarginSchedule();
            var features = TradingAccountFeaturesFactory.CreateCustomTradingAccountFeatures(GetSupportedOrderTypes(), marginSchedule);
            var tradingAccount = new FidelityTradingAccount(features);
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