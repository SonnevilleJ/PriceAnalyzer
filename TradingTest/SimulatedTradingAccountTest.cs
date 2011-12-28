﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace Sonneville.TradingTest
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
            var target = GetSimulator();

            var expected = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures().SupportedOrderTypes;
            var actual = target.Features.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyPositionsByDefault()
        {
            var target = GetSimulator();

            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void EventsTestFilled()
        {
            var target = GetSimulator();

            const string ticker = "DE";
            var filledRaised = false;
            var expiredRaised = false;
            var cancelRaised = false;
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) => expiredRaised = true;
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) => cancelRaised = true;
            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) => filledRaised = true;
            try
            {
                target.OrderCancelled += cancelledHandler;
                target.OrderFilled += filledHandler;
                target.OrderExpired += expiredHandler;

                var issued = DateTime.Now;
                var order = new Order(issued, issued.AddDays(1), OrderType.Buy, ticker, 5, 100.00m);
                target.Submit(order);

                target.WaitAll();

                Assert.IsTrue(filledRaised);
                Assert.IsFalse(cancelRaised);
                Assert.IsFalse(expiredRaised);
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
            var target = GetSimulator();

            var expiredRaised = false;
            var cancelRaised = false;
            var filledRaised = false;
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) => expiredRaised = true;
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) => cancelRaised = true;
            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) => filledRaised = true;
            try
            {
                target.OrderCancelled += cancelledHandler;
                target.OrderFilled += filledHandler;
                target.OrderExpired += expiredHandler;

                var issued = DateTime.Now;
                var order = new Order(issued, issued.AddMilliseconds(1), OrderType.Buy, "DE", 5, 100.00m);
                target.Submit(order);

                target.WaitAll();

                Assert.IsTrue(expiredRaised);
                Assert.IsFalse(cancelRaised);
                Assert.IsFalse(filledRaised);
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
            var target = GetSimulator();

            var cancelRaised = false;
            var expiredRaised = false;
            var filledRaised = false;
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) => cancelRaised = true;
            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) => filledRaised = true;
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) => expiredRaised = true;
            try
            {
                target.OrderCancelled += cancelledHandler;
                target.OrderFilled += filledHandler;
                target.OrderExpired += expiredHandler;

                var issued = DateTime.Now;
                var order = new Order(issued, issued.AddDays(1), OrderType.Buy, "DE", 5, 100.00m);
                target.Submit(order);
                target.TryCancelOrder(order);

                target.WaitAll();

                Assert.IsTrue(cancelRaised);
                Assert.IsFalse(filledRaised);
                Assert.IsFalse(expiredRaised);
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
            var target = GetSimulator();

            var issued = DateTime.Now;
            var expiration = issued.AddDays(1);
            const string ticker = "DE";
            const int shares = 5;
            const decimal price = 100.00m;

            foreach (var order in orderTypes.Select(orderType => new Order(issued, expiration, orderType, ticker, shares, price)))
            {
                VerifyOrderFillsCorrectly(target, order);
            }
        }

        private static void VerifyOrderFillsCorrectly(ITradingAccount target, Order order)
        {
            IShareTransaction expected = null;
            IShareTransaction actual = null;
            
            bool filledRaised;
            EventHandler<OrderExecutedEventArgs> filledHandler =
                (sender, e) =>
                    {
                        var commission = target.Features.CommissionSchedule.PriceCheck(e.Order);
                        expected = TransactionFactory.Instance.CreateShareTransaction(e.Executed, e.Order, commission);
                        actual = e.Transaction;
                        filledRaised = true;
                    };
            try
            {
                filledRaised = false;
                target.OrderFilled += filledHandler;

                target.Submit(order);

                target.WaitAll();

                Assert.IsTrue(filledRaised);

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
            foreach (var transaction in expected)
            {
                Func<IShareTransaction, bool> predicate = t => t.SettlementDate == transaction.SettlementDate &&
                                                               t.Ticker == transaction.Ticker &&
                                                               // we may simulate price fluctuations, so ignore price
                                                               //t.Price == transaction.Price &&
                                                               t.Shares == transaction.Shares &&
                                                               t.Commission == transaction.Commission;
                var actualCount = actual.Where(predicate).Count();
                var expectedCount = expected.Where(predicate).Count();

                Assert.AreEqual(expectedCount, actualCount);
            }
        }

        [TestMethod]
        public void MultipleEventsTestFilled()
        {
            var target = GetSimulator();

            const string ticker = "DE";
            var fillCount = 0;
            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) => Interlocked.Increment(ref fillCount);
            try
            {
                target.OrderFilled += filledHandler;

                const int count = 200;
                for (var i = 0; i < count; i++)
                {
                    var issued = DateTime.Now;
                    var order = new Order(issued, issued.AddDays(1), OrderType.Buy, ticker, 5, 100.00m);
                    target.Submit(order);
                    Thread.Sleep(1);
                }

                target.WaitAll();

                Assert.AreEqual(count, fillCount);
            }
            finally
            {
                target.OrderFilled -= filledHandler;
            }
        }

        [TestMethod]
        public void MultipleEventsTestCancelled()
        {
            var target = GetSimulator();

            const string ticker = "DE";
            var cancelCount = 0;
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) => Interlocked.Increment(ref cancelCount);
            try
            {
                target.OrderCancelled += cancelledHandler;

                const int count = 200;
                for (var i = 0; i < count; i++)
                {
                    var issued = DateTime.Now;
                    var order = new Order(issued, issued.AddDays(1), OrderType.Buy, ticker, 5, 100.00m);
                    target.Submit(order);
                    target.TryCancelOrder(order);
                    Thread.Sleep(1);
                }

                target.WaitAll();

                Assert.AreEqual(count, cancelCount);
            }
            finally
            {
                target.OrderCancelled -= cancelledHandler;
            }
        }

        [TestMethod]
        public void MultipleEventsTestExpired()
        {
            var target = GetSimulator();

            const string ticker = "DE";
            var expiredCount = 0;
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) => Interlocked.Increment(ref expiredCount);
            try
            {
                target.OrderExpired += expiredHandler;

                const int count = 200;
                for (var i = 0; i < count; i++)
                {
                    var issued = DateTime.Now;
                    var order = new Order(issued, issued.AddMilliseconds(1), OrderType.Buy, ticker, 5, 100.00m);
                    target.Submit(order);
                    Thread.Sleep(1);
                }

                target.WaitAll();

                Assert.AreEqual(count, expiredCount);
            }
            finally
            {
                target.OrderExpired -= expiredHandler;
            }
        }

        [TestMethod]
        public void HistoricalOrderFilledReturnsCorrectTransaction()
        {
            var target = GetSimulator();

            const string ticker = "DE";
            IShareTransaction expected = null;
            IShareTransaction actual = null;

            var filledRaised = false;
            EventHandler<OrderExecutedEventArgs> filledHandler =
                (sender, e) =>
                {
                    var commission = target.Features.CommissionSchedule.PriceCheck(e.Order);
                    expected = TransactionFactory.Instance.CreateShareTransaction(e.Executed, e.Order, commission);
                    actual = e.Transaction;
                    filledRaised = true;
                };
            try
            {
                target.OrderFilled += filledHandler;

                var issued = new DateTime(2010, 12, 20, 12, 0, 0);
                var expiration = issued.AddDays(1);
                var order = new Order(issued, expiration, OrderType.Buy, ticker, 5, 100.00m);
                target.Submit(order);

                target.WaitAll();

                Assert.IsTrue(filledRaised);

                AssertSameTransaction(expected, actual);
            }
            finally
            {
                target.OrderFilled -= filledHandler;
            }
        }

        [TestMethod]
        public void FilledAddsToPositionsCount()
        {
            var target = GetSimulator();

            const string ticker = "DE";
            var issued = new DateTime(2010, 12, 20, 12, 0, 0);
            var expiration = issued.AddDays(1);
            var order = new Order(issued, expiration, OrderType.Buy, ticker, 5, 100.00m);

            target.Submit(order);

            target.WaitAll();
            
            Assert.AreEqual(1, target.Positions.Count);
        }

        [TestMethod]
        public void FilledAddsToPositionsCorrectly()
        {
            var target = GetSimulator();

            const string ticker = "DE";
            IShareTransaction expected = null;

            EventHandler<OrderExecutedEventArgs> filledHandler =
                (sender, e) =>
                {
                    var commission = target.Features.CommissionSchedule.PriceCheck(e.Order);
                    expected = TransactionFactory.Instance.CreateShareTransaction(e.Executed, e.Order, commission);
                };
            try
            {
                target.OrderFilled += filledHandler;

                var issued = new DateTime(2010, 12, 20, 12, 0, 0);
                var expiration = issued.AddDays(1);
                var order = new Order(issued, expiration, OrderType.Buy, ticker, 5, 100.00m);
                target.Submit(order);

                target.WaitAll();

                var containsTransaction = TargetContainsTransaction(target, expected);
                Assert.IsTrue(containsTransaction);
            }
            finally
            {
                target.OrderFilled -= filledHandler;
            }
        }

        private static ITradingAccount GetSimulator()
        {
            return GetSimulator(new MarginNotAllowed());
        }

        private static ITradingAccount GetSimulator(IMarginSchedule marginSchedule)
        {
            return GetSimulator(new FlatCommissionSchedule(5.00m), marginSchedule);
        }

        private static ITradingAccount GetSimulator(ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            var orderTypes = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures().SupportedOrderTypes;
            return GetSimulator(orderTypes, commissionSchedule, marginSchedule);
        }

        private static ITradingAccount GetSimulator(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            return new SimulatedTradingAccount(TradingAccountFeaturesFactory.CreateTradingAccountFeatures(orderTypes, commissionSchedule, marginSchedule));
        }

        private static bool TargetContainsTransaction(ITradingAccount target, IShareTransaction transaction)
        {
            var positions = target.Positions;
            var position = positions.First(p => p.Ticker == transaction.Ticker);
            var transactions = position.Transactions.Cast<IShareTransaction>();

            return transactions.Select(
                trans => (
                             trans.OrderType == transaction.OrderType &&
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