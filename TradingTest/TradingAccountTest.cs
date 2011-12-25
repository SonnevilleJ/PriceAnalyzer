using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace Sonneville.TradingTest
{
    /// <summary>
    /// Summary description for TradingAccountTest
    /// </summary>
    [TestClass]
    public class TradingAccountTest
    {
        [TestMethod]
        public void TradingAccountFeaturesSupportedOrderTypesTest()
        {
            TradingAccount target = GetBacktestSimulator();

            var expected = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures().SupportedOrderTypes;
            var actual = target.Features.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyPositionsByDefault()
        {
            TradingAccount target = GetBacktestSimulator();

            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void EventsTestFilled()
        {
            TradingAccount target = GetBacktestSimulator();

            const string ticker = "DE";
            var filledRaised = false;
            var expiredRaised = false;
            var cancelRaised = false;
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) => expiredRaised = true;
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) => cancelRaised = true;
            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) => filledRaised = e.Transaction.Ticker == ticker;
            try
            {
                target.OrderCancelled += cancelledHandler;
                target.OrderFilled += filledHandler;
                target.OrderExpired += expiredHandler;

                var order = new Order(DateTime.Now, DateTime.Now.Add(BacktestSimulator.MaxProcessingTimeSpan), OrderType.Buy, ticker, 5, 100.00m);
                target.Submit(order);

                Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);

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
            TradingAccount target = GetBacktestSimulator();

            var expiredRaised = false;
            var cancelRaised = false;
            var filledRaised = false;
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) => expiredRaised = (e.Expired < DateTime.Now) && (e.Order.Expiration == e.Expired);
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) => cancelRaised = true;
            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) => filledRaised = true;
            try
            {
                target.OrderCancelled += cancelledHandler;
                target.OrderFilled += filledHandler;
                target.OrderExpired += expiredHandler;

                var order = new Order(DateTime.Now, DateTime.Now.AddMilliseconds(BacktestSimulator.MinProcessingTimeSpan.Milliseconds / 2.0), OrderType.Buy, "DE", 5, 100.00m);
                target.Submit(order);

                Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);
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
            TradingAccount target = GetBacktestSimulator();

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

                var order = new Order(DateTime.Now, DateTime.Now.Add(BacktestSimulator.MaxProcessingTimeSpan), OrderType.Buy, "DE", 5, 100.00m);
                target.Submit(order);
                target.TryCancelOrder(order);

                Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);
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
        public void TryCancelOrderReturnsTrue()
        {
            TradingAccount target = GetBacktestSimulator();

            var order = new Order(DateTime.Now, DateTime.Now.Add(BacktestSimulator.MaxProcessingTimeSpan), OrderType.Buy, "DE", 5, 100.00m);
            target.Submit(order);
            var result = target.TryCancelOrder(order);

            Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TryCancelOrderReturnsFalse()
        {
            TradingAccount target = GetBacktestSimulator();

            var order = new Order(DateTime.Now, DateTime.Now.Add(BacktestSimulator.MaxProcessingTimeSpan), OrderType.Buy, "DE", 5, 100.00m);
            target.Submit(order);

            Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);
            var result = target.TryCancelOrder(order);

            Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);
            Assert.IsFalse(result);
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
            var target = GetBacktestSimulator();

            var issued = DateTime.Now;
            var expiration = issued.AddDays(1);

            foreach (var orderType in orderTypes)
            {
                const string ticker = "DE";
                const int shares = 5;
                const decimal price = 100.00m;
                var order = new Order(issued, expiration, orderType, ticker, shares, price);

                VerifyOrderFillsCorrectly(target, order);
            }
        }

        private static void VerifyOrderFillsCorrectly(TradingAccount target, Order order)
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

                Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);

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
            TradingAccount target = GetBacktestSimulator();
            var padlock = new object();

            const string ticker = "DE";
            var filledRaised = 0;
            var expiredRaised = false;
            var cancelRaised = false;
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) => expiredRaised = true;
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) => cancelRaised = true;
            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) =>
                                                                     {
                                                                         if (e.Transaction.Ticker == ticker)
                                                                             lock(padlock) { filledRaised++; }
                                                                     };
            try
            {
                target.OrderCancelled += cancelledHandler;
                target.OrderFilled += filledHandler;
                target.OrderExpired += expiredHandler;

                const int count = 200;
                for (var i = 0; i < count; i++)
                {
                    var order = new Order(DateTime.Now, DateTime.Now.Add(BacktestSimulator.MaxProcessingTimeSpan), OrderType.Buy, ticker, 5, 100.00m);
                    target.Submit(order);
                    Thread.Sleep(1);
                }

                Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);

                Assert.AreEqual(count, filledRaised);
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
        public void MultipleEventsTestCancelled()
        {
            TradingAccount target = GetBacktestSimulator();
            var padlock = new object();

            const string ticker = "DE";
            var filledRaised = false;
            var expiredRaised = false;
            var cancelRaised = 0;
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) => expiredRaised = true;
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) => { lock (padlock) cancelRaised++; };
            EventHandler<OrderExecutedEventArgs> filledHandler = (sender, e) => filledRaised = true;
            try
            {
                target.OrderCancelled += cancelledHandler;
                target.OrderFilled += filledHandler;
                target.OrderExpired += expiredHandler;

                const int count = 200;
                for (var i = 0; i < count; i++)
                {
                    var order = new Order(DateTime.Now, DateTime.Now.Add(BacktestSimulator.MaxProcessingTimeSpan), OrderType.Buy, ticker, 5, 100.00m);
                    target.Submit(order);
                    target.TryCancelOrder(order);
                    Thread.Sleep(1);
                }

                Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);

                Assert.IsFalse(filledRaised);
                Assert.AreEqual(count, cancelRaised);
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
        public void HistoricalOrderFilledReturnsCorrectTransaction()
        {
            var target = GetBacktestSimulator();

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

                Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);

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
            var target = GetBacktestSimulator();

            const string ticker = "DE";
            var issued = new DateTime(2010, 12, 20, 12, 0, 0);
            var expiration = issued.AddDays(1);
            var order = new Order(issued, expiration, OrderType.Buy, ticker, 5, 100.00m);

            target.Submit(order);
            Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);
            
            Assert.AreEqual(1, target.Positions.Count);
        }

        [TestMethod]
        public void FilledAddsToPositionsCorrectly()
        {
            var target = GetBacktestSimulator();

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

                Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);

                var containsTransaction = TargetContainsTransaction(target, expected);
                Assert.IsTrue(containsTransaction);
            }
            finally
            {
                target.OrderFilled -= filledHandler;
            }
        }

        private static BacktestSimulator GetBacktestSimulator()
        {
            return GetBacktestSimulator(new MarginNotAllowed());
        }

        private static BacktestSimulator GetBacktestSimulator(IMarginSchedule marginSchedule)
        {
            return GetBacktestSimulator(new FlatCommissionSchedule(5.00m), marginSchedule);
        }

        private static BacktestSimulator GetBacktestSimulator(ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            var orderTypes = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures().SupportedOrderTypes;
            return GetBacktestSimulator(orderTypes, commissionSchedule, marginSchedule);
        }

        private static BacktestSimulator GetBacktestSimulator(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            return new BacktestSimulator(TradingAccountFeaturesFactory.CreateTradingAccountFeatures(orderTypes, commissionSchedule, marginSchedule));
        }

        private static bool TargetContainsTransaction(TradingAccount target, IShareTransaction transaction)
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
