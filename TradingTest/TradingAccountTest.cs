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
            TradingAccount target = new SimulatedAccount();

            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void OrderFilled()
        {
            TradingAccount target = new SimulatedAccount();

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
                
                var order = new Order(DateTime.Now, DateTime.Now.Add(SimulatedAccount.MaxProcessingTimeSpan), OrderType.Buy, ticker, 5, 100.00m);
                target.Submit(order);

                Thread.Sleep(SimulatedAccount.MaxProcessingTimeSpan);

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
            TradingAccount target = new SimulatedAccount();

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

                var order = new Order(DateTime.Now, DateTime.Now.AddMilliseconds(SimulatedAccount.MinProcessingTimeSpan.Milliseconds / 2.0), OrderType.Buy, "DE", 5, 100.00m);
                target.Submit(order);

                Thread.Sleep(SimulatedAccount.MaxProcessingTimeSpan);
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
            TradingAccount target = new SimulatedAccount();

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

                var order = new Order(DateTime.Now, DateTime.Now.Add(SimulatedAccount.MaxProcessingTimeSpan), OrderType.Buy, "DE", 5, 100.00m);
                target.Submit(order);
                target.TryCancelOrder(order);

                Thread.Sleep(SimulatedAccount.MaxProcessingTimeSpan);
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
            TradingAccount target = new SimulatedAccount();

            var order = new Order(DateTime.Now, DateTime.Now.Add(SimulatedAccount.MaxProcessingTimeSpan), OrderType.Buy, "DE", 5, 100.00m);
            target.Submit(order);
            var result = target.TryCancelOrder(order);

            Thread.Sleep(SimulatedAccount.MaxProcessingTimeSpan);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OrderCancelledReturnsFalse()
        {
            TradingAccount target = new SimulatedAccount();

            var order = new Order(DateTime.Now, DateTime.Now.Add(SimulatedAccount.MaxProcessingTimeSpan), OrderType.Buy, "DE", 5, 100.00m);
            target.Submit(order);

            Thread.Sleep(SimulatedAccount.MaxProcessingTimeSpan);
            var result = target.TryCancelOrder(order);

            Thread.Sleep(SimulatedAccount.MaxProcessingTimeSpan);
            Assert.IsFalse(result);
        }
    }
}
