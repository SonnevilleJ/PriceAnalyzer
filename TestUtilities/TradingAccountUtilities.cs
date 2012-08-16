using System;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace TestUtilities.Sonneville.PriceTools
{
    public static class TradingAccountUtilities{
        /// <summary>
        /// Creates a simulated <see cref="TradingAccount"/> which accepts all <see cref="OrderType"/>s, does not allow margin trading, imposes a flat commission of $5.00 per transaction, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <returns></returns>
        public static TradingAccount CreateSimulatedTradingAccount()
        {
            return CreateSimulatedTradingAccount(new MarginNotAllowed());
        }

        /// <summary>
        /// Creates a simulated <see cref="TradingAccount"/> which accepts all <see cref="OrderType"/>s, a flat commission of $5.00 per transaction, uses the given <see cref="IMarginSchedule"/>, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <returns></returns>
        public static TradingAccount CreateSimulatedTradingAccount(IMarginSchedule marginSchedule)
        {
            return CreateSimulatedTradingAccount(new FlatCommissionSchedule(5.00m), marginSchedule);
        }

        /// <summary>
        /// Creates a simulated <see cref="TradingAccount"/> which accepts all <see cref="OrderType"/>s, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <returns></returns>
        public static TradingAccount CreateSimulatedTradingAccount(ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            var orderTypes = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures().SupportedOrderTypes;
            return CreateSimulatedTradingAccount(orderTypes, commissionSchedule, marginSchedule);
        }

        /// <summary>
        /// Creates a simulated <see cref="TradingAccount"/> with an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="TradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <returns></returns>
        public static TradingAccount CreateSimulatedTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            // default deposit of $1,000,000
            var deposit = TransactionFactory.ConstructDeposit(new DateTime(1900, 1, 1), 1000000.00m);
            return CreateSimulatedTradingAccount(orderTypes, commissionSchedule, marginSchedule, deposit);
        }

        /// <summary>
        /// Creates a simulated <see cref="TradingAccount"/>.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="TradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <param name="openingDeposit">The opening deposit to place in the <see cref="Portfolio"/> used by the <see cref="TradingAccount"/>.</param>
        /// <returns></returns>
        public static TradingAccount CreateSimulatedTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule, Deposit openingDeposit)
        {
            var tradingAccountFeatures = TradingAccountFeaturesFactory.CreateTradingAccountFeatures(orderTypes, commissionSchedule, marginSchedule);
            var portfolio = PortfolioFactory.ConstructPortfolio();
            portfolio.Deposit(openingDeposit);
            return new SimulatedTradingAccountImpl {Features = tradingAccountFeatures, Portfolio = portfolio};
        }

        /// <summary>
        /// Creates an <see cref="ShareTransaction"/> which would result from the perfect execution of <paramref name="order"/>.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use as the SettlementDate for the resulting <see cref="ShareTransaction"/>.</param>
        /// <param name="order">The <see cref="Order"/> which should define the parameters for the resulting <see cref="ShareTransaction"/>.</param>
        /// <param name="commission">The commission that should be charged for the resulting <see cref="ShareTransaction"/>.</param>
        /// <returns></returns>
        public static ShareTransaction CreateShareTransaction(DateTime settlementDate, Order order, decimal commission)
        {
            return TransactionFactory.ConstructShareTransaction(order.OrderType, order.Ticker, settlementDate, order.Shares, order.Price, commission);
        }
    }
}