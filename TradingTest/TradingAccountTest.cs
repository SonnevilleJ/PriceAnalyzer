using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace TradingTest
{
    /// <summary>
    /// Summary description for TradingAccountTest
    /// </summary>
    [TestClass]
    public class TradingAccountTest
    {
        [TestMethod]
        public void EmptyPositionsByDefault()
        {
            TradingAccount target = new BacktestSimulator();

            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void OrderFilled()
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
        public void OrderExpired()
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
        public void OrderCancelled()
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
            var expiredRaised = false;
            var cancelRaised = false;
            EventHandler<OrderExpiredEventArgs> expiredHandler = (sender, e) => expiredRaised = true;
            EventHandler<OrderCancelledEventArgs> cancelledHandler = (sender, e) => cancelRaised = true;
            EventHandler<OrderExecutedEventArgs> filledHandler =
                (sender, e) =>
                    {
                        expected = TransactionFactory.Instance.CreateShareTransaction(e.Executed, e.Order, target.Commission);
                        actual = e.Transaction;
                        filledRaised = true;
                    };
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

                AssertSameTransaction(expected, actual);
            }
            finally
            {
                target.OrderCancelled -= cancelledHandler;
                target.OrderFilled -= filledHandler;
                target.OrderExpired -= expiredHandler;
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
        public void MultipleOrdersFilled()
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
                                                                             lock(padlock)
                                                                                 filledRaised++;
                                                                     };
            try
            {
                target.OrderCancelled += cancelledHandler;
                target.OrderFilled += filledHandler;
                target.OrderExpired += expiredHandler;

                const int count = 200;
                for (int i = 0; i < count; i++)
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
    }
}
