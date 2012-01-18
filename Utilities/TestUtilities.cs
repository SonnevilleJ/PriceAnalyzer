using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.Utilities
{
    public static class TestUtilities
    {
        #region Price Period tools

        public static IPricePeriod CreatePeriod1()
        {
            var head = new DateTime(2011, 3, 14);
            var tail = head.GetFollowingClose();
            const decimal open = 100.00m;
            const decimal high = 110.00m;
            const decimal low = 90.00m;
            const decimal close = 100.00m;
            const long volume = 20000;

            return PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        public static IPricePeriod CreatePeriod2()
        {
            var head = new DateTime(2011, 3, 15);
            var tail = head.GetFollowingClose();
            const decimal open = 100.00m;
            const decimal high = 120.00m;
            const decimal low = 100.00m;
            const decimal close = 110.00m;

            return PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close);
        }

        public static IPricePeriod CreatePeriod3()
        {
            var head = new DateTime(2011, 3, 16);
            var tail = head.GetFollowingClose();
            const decimal open = 110.00m;
            const decimal high = 110.00m;
            const decimal low = 80.00m;
            const decimal close = 90.00m;
            const long volume = 10000;

            return PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        #endregion

        #region Price Quote tools

        public static PriceQuote CreateQuote1()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("2/28/2011 9:30 AM"),
                Price = 10,
                Volume = 50
            };
        }

        public static PriceQuote CreateQuote2()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("3/1/2011 10:00 AM"),
                Price = 9,
                Volume = 60
            };
        }

        public static PriceQuote CreateQuote3()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("3/2/2011 2:00 PM"),
                Price = 14,
                Volume = 50
            };
        }

        public static PriceQuote CreateQuote4()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("3/2/2011 4:00 PM"),
                Price = 11,
                Volume = 30
            };
        }

        #endregion

        #region Trading Account tools

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, does not allow margin trading, imposes a flat commission of $5.00 per transaction, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <returns></returns>
        public static ITradingAccount CreateSimulatedTradingAccount()
        {
            return CreateSimulatedTradingAccount(new MarginNotAllowed());
        }

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, a flat commission of $5.00 per transaction, uses the given <see cref="IMarginSchedule"/>, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public static ITradingAccount CreateSimulatedTradingAccount(IMarginSchedule marginSchedule)
        {
            return CreateSimulatedTradingAccount(new FlatCommissionSchedule(5.00m), marginSchedule);
        }

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public static ITradingAccount CreateSimulatedTradingAccount(ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            var orderTypes = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures().SupportedOrderTypes;
            return CreateSimulatedTradingAccount(orderTypes, commissionSchedule, marginSchedule);
        }

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> with an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="ITradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public static ITradingAccount CreateSimulatedTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            // default deposit of $1,000,000
            var deposit = TransactionFactory.ConstructDeposit(new DateTime(1900, 1, 1), 1000000.00m);
            return CreateSimulatedTradingAccount(orderTypes, commissionSchedule, marginSchedule, deposit);
        }

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/>.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="ITradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="openingDeposit">The opening deposit to place in the <see cref="IPortfolio"/> used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public static ITradingAccount CreateSimulatedTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule, Deposit openingDeposit)
        {
            var tradingAccountFeatures = TradingAccountFeaturesFactory.CreateTradingAccountFeatures(orderTypes, commissionSchedule, marginSchedule);
            var portfolio = new Portfolio();
            portfolio.Deposit(openingDeposit);
            return new SimulatedTradingAccount {Features = tradingAccountFeatures, Portfolio = portfolio};
        }

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> which operates asynchronously.
        /// </summary>
        /// <returns></returns>
        public static ITradingAccount GetAsynchronousSimulatedTradingAccount()
        {
            Assert.Inconclusive("No asynchronous TradingAccount is available.");
            return null;
        }

        /// <summary>
        /// Creates an <see cref="IShareTransaction"/> which would result from the perfect execution of <paramref name="order"/>.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use as the SettlementDate for the resulting <see cref="IShareTransaction"/>.</param>
        /// <param name="order">The <see cref="Order"/> which should define the parameters for the resulting <see cref="IShareTransaction"/>.</param>
        /// <param name="commission">The commission that should be charged for the resulting <see cref="IShareTransaction"/>.</param>
        /// <returns></returns>
        public static IShareTransaction CreateShareTransaction(DateTime settlementDate, Order order, decimal commission)
        {
            return TransactionFactory.ConstructShareTransaction(settlementDate, order.OrderType, order.Ticker, order.Price, order.Shares, commission);
        }

        #endregion

        /// <summary>
        /// Asserts that each property is identical for the two instances of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AssertSameState<T>(T expected, T actual)
        {
            var properties = expected.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.GetIndexParameters().Length != 0) continue;

                Assert.AreEqual(propertyInfo.GetValue(expected, null), propertyInfo.GetValue(actual, null));
            }
        }
    }
}
