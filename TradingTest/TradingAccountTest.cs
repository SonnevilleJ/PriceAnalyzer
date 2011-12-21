using System;
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
            TradingAccount target = new BacktestSimulator();

            var expected = TradingAccountFeatures.Factory.CreateFullTradingAccountFeatures().SupportedOrderTypes;
            var actual = target.Features.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyPositionsByDefault()
        {
            TradingAccount target = new BacktestSimulator();

            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void EventsTestFilled()
        {
            TradingAccount target = new BacktestSimulator();

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
            TradingAccount target = new BacktestSimulator();

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
            TradingAccount target = new BacktestSimulator();

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
        public void OrderCancelledReturnsTrue()
        {
            TradingAccount target = new BacktestSimulator();

            var order = new Order(DateTime.Now, DateTime.Now.Add(BacktestSimulator.MaxProcessingTimeSpan), OrderType.Buy, "DE", 5, 100.00m);
            target.Submit(order);
            var result = target.TryCancelOrder(order);

            Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OrderCancelledReturnsFalse()
        {
            TradingAccount target = new BacktestSimulator();

            var order = new Order(DateTime.Now, DateTime.Now.Add(BacktestSimulator.MaxProcessingTimeSpan), OrderType.Buy, "DE", 5, 100.00m);
            target.Submit(order);

            Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);
            var result = target.TryCancelOrder(order);

            Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OrderFilledReturnsCorrectTransaction()
        {
            var target = new BacktestSimulator();

            const string ticker = "DE";
            IShareTransaction expected = null;
            IShareTransaction actual = null;
            
            var filledRaised = false;
            EventHandler<OrderExecutedEventArgs> filledHandler =
                (sender, e) =>
                    {
                        expected = TransactionFactory.Instance.CreateShareTransaction(e.Executed, e.Order, target.Commission);
                        actual = e.Transaction;
                        filledRaised = true;
                    };
            try
            {
                target.OrderFilled += filledHandler;

                var order = new Order(DateTime.Now, DateTime.Now.Add(BacktestSimulator.MaxProcessingTimeSpan), OrderType.Buy, ticker, 5, 100.00m);
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
        public void OrderFilledReturnsCorrectTransactionWhenCommissionCustomized()
        {
            const decimal commission = 7.95m;
            var target = new BacktestSimulator(commission);

            Assert.AreEqual(commission, target.Commission);

            const string ticker = "DE";
            IShareTransaction expected = null;
            IShareTransaction actual = null;

            var filledRaised = false;
            EventHandler<OrderExecutedEventArgs> filledHandler =
                (sender, e) =>
                {
                    expected = TransactionFactory.Instance.CreateShareTransaction(e.Executed, e.Order, target.Commission);
                    actual = e.Transaction;
                    filledRaised = true;
                };
            try
            {
                target.OrderFilled += filledHandler;

                var order = new Order(DateTime.Now, DateTime.Now.Add(BacktestSimulator.MaxProcessingTimeSpan), OrderType.Buy, ticker, 5, 100.00m);
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
            Assert.AreEqual(expected.SettlementDate, actual.SettlementDate);
            Assert.AreEqual(expected.Ticker, actual.Ticker);
            // we may simulate price fluctuations, so ignore price
            //Assert.AreEqual(expected.Price, actual.Price);
            Assert.AreEqual(expected.Shares, actual.Shares);
            Assert.AreEqual(expected.Commission, actual.Commission);
        }

        [TestMethod]
        public void MultipleEventsTestFilled()
        {
            TradingAccount target = new BacktestSimulator();
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
            TradingAccount target = new BacktestSimulator();
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
            var target = new BacktestSimulator();

            const string ticker = "DE";
            IShareTransaction expected = null;
            IShareTransaction actual = null;

            var filledRaised = false;
            EventHandler<OrderExecutedEventArgs> filledHandler =
                (sender, e) =>
                {
                    expected = TransactionFactory.Instance.CreateShareTransaction(e.Executed, e.Order, target.Commission);
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
            var target = new BacktestSimulator();

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
            var target = new BacktestSimulator();

            const string ticker = "DE";
            IShareTransaction expected = null;

            EventHandler<OrderExecutedEventArgs> processedHandler =
                (sender, e) =>
                {
                    expected = TransactionFactory.Instance.CreateShareTransaction(e.Executed, e.Order, target.Commission);
                };
            try
            {
                target.TransactionProcessed += processedHandler;

                var issued = new DateTime(2010, 12, 20, 12, 0, 0);
                var expiration = issued.AddDays(1);
                var order = new Order(issued, expiration, OrderType.Buy, ticker, 5, 100.00m);
                target.Submit(order);

                Thread.Sleep(BacktestSimulator.MaxProcessingTimeSpan);

                Assert.IsTrue(TargetContainsTransaction(target, expected));
            }
            finally
            {
                target.TransactionProcessed -= processedHandler;
            }
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
