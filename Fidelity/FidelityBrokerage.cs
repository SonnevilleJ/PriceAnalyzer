using System;
using System.Security.Authentication;
using Sonneville.PriceTools.AutomatedTrading;

namespace Sonneville.PriceTools.Fidelity
{
    public class FidelityBrokerage : IBrokerage
    {
        private readonly ITradingAccountFeaturesFactory _tradingAccountFeaturesFactory;

        public FidelityBrokerage()
        {
            _tradingAccountFeaturesFactory = new TradingAccountFeaturesFactory();
        }

        /// <summary>
        /// Logs the user into the brokerage.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <exception cref="AuthenticationException">Thrown when the supplied credentials are incorrect.</exception>
        public ITradingAccount LogIn(string username, string password)
        {
            var commissionSchedule = GetCommissionSchedule();
            var marginSchedule = GetMarginSchedule();
            var features = _tradingAccountFeaturesFactory.ConstructTradingAccountFeatures(GetSupportedOrderTypes(), commissionSchedule, marginSchedule);
            var tradingAccount = new FidelityTradingAccount { Features = features };
            return tradingAccount;
        }

        #region Private Methods

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

        #endregion
    }
}