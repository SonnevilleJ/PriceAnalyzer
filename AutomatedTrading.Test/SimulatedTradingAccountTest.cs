using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Implementation;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools.AutomatedTrading
{
    [TestClass]
    public class SimulatedTradingAccountTest
    {
        private static readonly IOrderFactory OrderFactory;
        private static readonly ITradingAccountFactory TradingAccountFactory;
        private readonly ITradingAccountFeaturesFactory _tradingAccountFeaturesFactory;

        static SimulatedTradingAccountTest()
        {
            OrderFactory = new OrderFactory();
            TradingAccountFactory = new TradingAccountFactory();
        }

        public SimulatedTradingAccountTest()
        {
            _tradingAccountFeaturesFactory = new TradingAccountFeaturesFactory();
        }

        [TestMethod]
        public void TradingAccountFeaturesSupportedOrderTypesTest()
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();

            var expected = _tradingAccountFeaturesFactory.ConstructFullTradingAccountFeatures().SupportedOrderTypes;
            var actual = target.Features.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyPositionsByDefault()
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();

            Assert.AreEqual(0, target.Portfolio.Positions.Count());
        }

        [TestMethod]
        public void BuyOrderReturnsCorrectTransaction()
        {
            VerifyOrderFillsCorrectly(OrderType.Buy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellOrderThrowsValidationError()
        {
            VerifyOrderFillsCorrectly(OrderType.Sell);
        }

        [TestMethod]
        public void SellOrderReturnsCorrectTransaction()
        {
            VerifyOrderFillsCorrectly(OrderType.Buy, OrderType.Sell);
        }

        [TestMethod]
        public void SellShortOrderReturnsCorrectTransaction()
        {
            VerifyOrderFillsCorrectly(OrderType.SellShort);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BuyToCoverOrderThrowsValidationError()
        {
            VerifyOrderFillsCorrectly(OrderType.BuyToCover);
        }

        [TestMethod]
        public void BuyToCoverOrderReturnsCorrectTransaction()
        {
            VerifyOrderFillsCorrectly(OrderType.SellShort, OrderType.BuyToCover);
        }

        private static void VerifyOrderFillsCorrectly(params OrderType[] orderTypes)
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();

            var ticker = TickerManager.GetUniqueTicker();
            foreach (var orderType in orderTypes)
            {
                var issued = DateTime.Now;
                var expiration = issued.AddDays(1);
                const int shares = 5;
                const decimal price = 100.00m;
                
                var order = OrderFactory.ConstructOrder(issued, expiration, orderType, ticker, shares, price);
                
                VerifyOrderFillsCorrectly(target, order);
            }
        }

        private static void VerifyOrderFillsCorrectly(ITradingAccount target, Order order)
        {
            IShareTransaction expected = null;
            IShareTransaction actual = null;
            var syncroot = new object();

            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) =>
                                                                     {
                                                                         lock (syncroot)
                                                                         {
                                                                             var commission = target.Features.CommissionSchedule.PriceCheck(e.Order);
                                                                             expected = TradingAccountUtilities.CreateShareTransaction(e.Executed, e.Order, commission);
                                                                             actual = e.Transaction;
                                                                             Monitor.Pulse(syncroot);
                                                                         }
                                                                     };
            try
            {
                target.OrderFilled += filledHandler;

                lock (syncroot)
                {
                    target.Submit(order);
                    Monitor.Wait(syncroot);
                }
                AssertSameTransaction(expected, actual);
            }
            finally
            {
                target.OrderFilled -= filledHandler;
            }
        }

        private static void AssertSameTransaction(IShareTransaction expected, IShareTransaction actual)
        {
            var expectedList = new List<IShareTransaction> {expected};
            var actualList = new List<IShareTransaction> {actual};
            AssertSameTransactions(expectedList, actualList);
        }

        private static void AssertSameTransactions(IEnumerable<IShareTransaction> expected, IEnumerable<IShareTransaction> actual)
        {
            var expectedArray = expected.ToArray();
            var actualArray = actual.ToArray();

            foreach (var transaction in expectedArray)
            {
                var local = transaction;
                Func<IShareTransaction, bool> predicate = t => t.SettlementDate == local.SettlementDate &&
                                                               t.Ticker == local.Ticker &&
                                                               // we may simulate price fluctuations, so ignore price
                                                               //t.Price == local.Price &&
                                                               t.Shares == local.Shares &&
                                                               t.Commission == local.Commission;
                var actualCount = actualArray.Where(predicate).Count();
                var expectedCount = expectedArray.Where(predicate).Count();

                Assert.AreEqual(expectedCount, actualCount);
            }
        }

        [TestMethod]
        public void HistoricalOrderFilledReturnsCorrectTransaction()
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();

            IShareTransaction expected = null;
            IShareTransaction actual = null;
            var syncroot = new object();

            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) =>
                                                                     {
                                                                         lock (syncroot)
                                                                         {
                                                                             var commission = target.Features.CommissionSchedule.PriceCheck(e.Order);
                                                                             expected = TradingAccountUtilities.CreateShareTransaction(e.Executed, e.Order, commission);
                                                                             actual = e.Transaction;
                                                                             Monitor.Pulse(syncroot);
                                                                         }
                                                                     };
            try
            {
                target.OrderFilled += filledHandler;

                var issued = new DateTime(2010, 12, 20, 12, 0, 0);
                var expiration = issued.AddDays(1);
                var order = OrderFactory.ConstructOrder(issued, expiration, OrderType.Buy, TickerManager.GetUniqueTicker(), 5, 100.00m);

                lock (syncroot)
                {
                    target.Submit(order);
                    Monitor.Wait(syncroot);

                    AssertSameTransaction(expected, actual);
                }
            }
            finally
            {
                target.OrderFilled -= filledHandler;
            }
        }

        [TestMethod]
        public void FilledAddsToPortfolioCorrectly()
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();

            IShareTransaction expected = null;
            var syncroot = new object();

            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) =>
                                                                     {
                                                                         lock (syncroot)
                                                                         {
                                                                             var commission = target.Features.CommissionSchedule.PriceCheck(e.Order);
                                                                             expected = TradingAccountUtilities.CreateShareTransaction(e.Executed, e.Order, commission);
                                                                             Monitor.Pulse(syncroot);
                                                                         }
                                                                     };
            try
            {
                target.OrderFilled += filledHandler;

                var issued = new DateTime(2010, 12, 20, 12, 0, 0);
                var expiration = issued.AddDays(1);
                var order = OrderFactory.ConstructOrder(issued, expiration, OrderType.Buy, TickerManager.GetUniqueTicker(), 5, 100.00m);

                lock (syncroot)
                {
                    target.Submit(order);
                    Monitor.Wait(syncroot);
                }

                var containsTransaction = TargetContainsTransaction(target, expected);
                Assert.IsTrue(containsTransaction);
            }
            finally
            {
                target.OrderFilled -= filledHandler;
            }
        }

        private static bool TargetContainsTransaction(ITradingAccount target, IShareTransaction transaction)
        {
            return target.Portfolio.Transactions.Where(t=>t is IShareTransaction).Cast<IShareTransaction>().Select(
                trans => (
                             trans.GetType() == transaction.GetType() &&
                             trans.Commission == transaction.Commission &&
                             trans.SettlementDate == transaction.SettlementDate &&
                             // price may fluctuate
                             //trans.Price == transaction.Price &&
                             trans.Shares == transaction.Shares &&
                             trans.Ticker == transaction.Ticker)
                ).FirstOrDefault();
        }
    }
}
