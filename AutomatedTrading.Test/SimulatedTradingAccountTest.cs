using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// Summary description for SimulatedTradingAccountTest
    /// </summary>
    [TestClass]
    public class SimulatedTradingAccountTest
    {
        [TestMethod]
        public void TradingAccountFeaturesSupportedOrderTypesTest()
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();

            var expected = TradingAccountFeaturesFactory.ConstructFullTradingAccountFeatures().SupportedOrderTypes;
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
        public void EventsTestFilled()
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();
            var syncroot = new object();

            var expiredRaised = false;
            var cancelRaised = false;
            var filledRaised = false;
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) =>
                                                                     {
                                                                         lock (syncroot)
                                                                         {
                                                                             expiredRaised = true;
                                                                             Monitor.Pulse(syncroot);
                                                                         }
                                                                     };
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) =>
                                                                         {
                                                                             lock (syncroot)
                                                                             {
                                                                                 cancelRaised = true;
                                                                                 Monitor.Pulse(syncroot);
                                                                             }
                                                                         };
            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) =>
                                                                     {
                                                                         lock (syncroot)
                                                                         {
                                                                             filledRaised = true;
                                                                             Monitor.Pulse(syncroot);
                                                                         }
                                                                     };
            try
            {
                target.OrderCancelled += cancelledHandler;
                target.OrderFilled += filledHandler;
                target.OrderExpired += expiredHandler;

                var issued = DateTime.Now;
                var order = OrderFactory.ConstructOrder(issued, issued.AddDays(1), OrderType.Buy, TickerManager.GetUniqueTicker(), 5, 100.00m);

                lock (syncroot)
                {
                    target.Submit(order);

                    Monitor.Wait(syncroot);
                    Assert.IsTrue(filledRaised);
                    Assert.IsFalse(cancelRaised);
                    Assert.IsFalse(expiredRaised);
                }
            }
            finally
            {
                target.OrderCancelled -= cancelledHandler;
                target.OrderFilled -= filledHandler;
                target.OrderExpired -= expiredHandler;
            }
        }

        [TestMethod]
        public void EventsTestExpired()
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();
            var syncroot = new object();

            var expiredRaised = false;
            var cancelRaised = false;
            var filledRaised = false;
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) =>
                                                                     {
                                                                         lock (syncroot)
                                                                         {
                                                                             expiredRaised = true;
                                                                             Monitor.Pulse(syncroot);
                                                                         }
                                                                     };
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) =>
                                                                         {
                                                                             lock (syncroot)
                                                                             {
                                                                                 cancelRaised = true;
                                                                                 Monitor.Pulse(syncroot);
                                                                             }
                                                                         };
            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) =>
                                                                     {
                                                                         lock (syncroot)
                                                                         {
                                                                             filledRaised = true;
                                                                             Monitor.Pulse(syncroot);
                                                                         }
                                                                     };
            try
            {
                target.OrderCancelled += cancelledHandler;
                target.OrderFilled += filledHandler;
                target.OrderExpired += expiredHandler;

                var expired = DateTime.Now;
                var order = OrderFactory.ConstructOrder(expired.AddTicks(-1), expired, OrderType.Buy, TickerManager.GetUniqueTicker(), 5, 100.00m);
                lock (syncroot)
                {
                    target.Submit(order);

                    Monitor.Wait(syncroot);
                    Assert.IsTrue(expiredRaised);
                    Assert.IsFalse(cancelRaised);
                    Assert.IsFalse(filledRaised);
                }
            }
            finally
            {
                target.OrderCancelled -= cancelledHandler;
                target.OrderFilled -= filledHandler;
                target.OrderExpired -= expiredHandler;
            }
        }

        [TestMethod]
        public void EventsTestCancelled()
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();
            var syncroot = new object();

            var expiredRaised = false;
            var cancelRaised = false;
            var filledRaised = false;
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) =>
                                                                     {
                                                                         lock (syncroot)
                                                                         {
                                                                             expiredRaised = true;
                                                                             Monitor.Pulse(syncroot);
                                                                         }
                                                                     };
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) =>
                                                                         {
                                                                             lock (syncroot)
                                                                             {
                                                                                 cancelRaised = true;
                                                                                 Monitor.Pulse(syncroot);
                                                                             }
                                                                         };
            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) =>
                                                                     {
                                                                         lock (syncroot)
                                                                         {
                                                                             filledRaised = true;
                                                                             Monitor.Pulse(syncroot);
                                                                         }
                                                                     };
            try
            {
                target.OrderCancelled += cancelledHandler;
                target.OrderFilled += filledHandler;
                target.OrderExpired += expiredHandler;

                var issued = DateTime.Now;
                var order = OrderFactory.ConstructOrder(issued, issued.AddDays(1), OrderType.Buy, TickerManager.GetUniqueTicker(), 5, 100.00m);

                lock (syncroot)
                {
                    target.Submit(order);
                    target.TryCancelOrder(order);

                    Monitor.Wait(syncroot);
                    Assert.IsTrue(cancelRaised);
                    Assert.IsFalse(filledRaised);
                    Assert.IsFalse(expiredRaised);
                }
            }
            finally
            {
                target.OrderCancelled -= cancelledHandler;
                target.OrderFilled -= filledHandler;
                target.OrderExpired -= expiredHandler;
            }
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

        private static void VerifyOrderFillsCorrectly(TradingAccount target, Order order)
        {
            ShareTransaction expected = null;
            ShareTransaction actual = null;
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

        private static void AssertSameTransaction(ShareTransaction expected, ShareTransaction actual)
        {
            var expectedList = new List<ShareTransaction> {expected};
            var actualList = new List<ShareTransaction> {actual};
            AssertSameTransactions(expectedList, actualList);
        }

        private static void AssertSameTransactions(IEnumerable<ShareTransaction> expected, IEnumerable<ShareTransaction> actual)
        {
            foreach (var transaction in expected)
            {
                var local = transaction;
                Func<ShareTransaction, bool> predicate = t => t.SettlementDate == local.SettlementDate &&
                                                               t.Ticker == local.Ticker &&
                                                               // we may simulate price fluctuations, so ignore price
                                                               //t.Price == local.Price &&
                                                               t.Shares == local.Shares &&
                                                               t.Commission == local.Commission;
                var actualCount = actual.Where(predicate).Count();
                var expectedCount = expected.Where(predicate).Count();

                Assert.AreEqual(expectedCount, actualCount);
            }
        }

        [TestMethod]
        public void MultipleEventsTestFilled()
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();

            const int count = 5;
            var syncroot = new object();
            var filled = new bool[count];
            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) =>
                                                                     {
                                                                         lock (syncroot)
                                                                         {
                                                                             filled[(int) e.Order.Shares] = true;
                                                                         }
                                                                     };
            try
            {
                target.OrderFilled += filledHandler;

                for (var i = 0; i < count; i++)
                {
                    var issued = DateTime.Now;
                    var order = OrderFactory.ConstructOrder(issued, issued.AddDays(1), OrderType.Buy, TickerManager.GetUniqueTicker(), i, 100.00m);
                    target.Submit(order);
                    Thread.Sleep(50);
                }

                Thread.Sleep(500);
                lock (syncroot) Assert.AreEqual(count, filled.Count(b => b));
            }
            finally
            {
                target.OrderFilled -= filledHandler;
            }
        }

        [TestMethod]
        public void MultipleEventsTestCancelled()
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();

            const int count = 5;
            var syncroot = new object();
            var cancelled = new bool[count];
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) =>
                                                                         {
                                                                             lock (syncroot)
                                                                             {
                                                                                 cancelled[(int) e.Order.Shares] = true;
                                                                             }
                                                                         };
            try
            {
                target.OrderCancelled += cancelledHandler;

                for (var i = 0; i < count; i++)
                {
                    var issued = DateTime.Now;
                    var order = OrderFactory.ConstructOrder(issued, issued.AddDays(1), OrderType.Buy, TickerManager.GetUniqueTicker(), i, 100.00m);
                    target.Submit(order);
                    Thread.Sleep(50);
                    target.TryCancelOrder(order);
                }

                Thread.Sleep(500);
                lock (syncroot) Assert.AreEqual(count, cancelled.Count(b => b));
            }
            finally
            {
                target.OrderCancelled -= cancelledHandler;
            }
        }

        [TestMethod]
        public void MultipleEventsTestExpired()
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();

            const int count = 5;
            var syncroot = new object();
            var expired = new bool[count];
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) =>
                                                                     {
                                                                         lock (syncroot)
                                                                         {
                                                                             expired[(int) e.Order.Shares] = true;
                                                                         }
                                                                     };
            try
            {
                target.OrderExpired += expiredHandler;

                for (var i = 0; i < count; i++)
                {
                    var issued = DateTime.Now;
                    var order = OrderFactory.ConstructOrder(issued, issued.AddTicks(1), OrderType.Buy, TickerManager.GetUniqueTicker(), i, 100.00m);
                    Thread.Sleep(50);
                    target.Submit(order);
                }

                Thread.Sleep(500);
                lock (syncroot) Assert.AreEqual(count, expired.Count(b => b));
            }
            finally
            {
                target.OrderExpired -= expiredHandler;
            }
        }

        [TestMethod]
        public void HistoricalOrderFilledReturnsCorrectTransaction()
        {
            var target = TradingAccountFactory.ConstructSimulatedTradingAccount();

            ShareTransaction expected = null;
            ShareTransaction actual = null;
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

            ShareTransaction expected = null;
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

        private static bool TargetContainsTransaction(TradingAccount target, ShareTransaction transaction)
        {
            return target.Portfolio.Transactions.Where(t=>t is ShareTransaction).Cast<ShareTransaction>().Select(
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
