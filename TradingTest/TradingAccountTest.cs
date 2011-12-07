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
        private EventHandler<OrderExecutedEventArgs> _filledHandler;
        private EventHandler<OrderExpiredEventArgs> _expiredHandler;
        private EventHandler<OrderCancelledEventArgs> _cancelledHandler;

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
            _expiredHandler = (sender, e) => expiredRaised = true;
            _cancelledHandler = (sender, e) => cancelRaised = true;
            _filledHandler = (sender, e) => filledRaised = e.Transaction.Ticker == ticker;
            try
            {
                target.OrderCancelled += _cancelledHandler;
                target.OrderFilled += _filledHandler;
                target.OrderExpired += _expiredHandler;
                
                var order = new Order(DateTime.Now, DateTime.Now.Add(SimulatedAccount.MaxProcessingTimeSpan), OrderType.Buy, ticker, 5, 100.00m);
                target.Submit(order);

                Thread.Sleep(SimulatedAccount.MaxProcessingTimeSpan);

                Assert.IsTrue(filledRaised);
                Assert.IsFalse(cancelRaised);
                Assert.IsFalse(expiredRaised);
            }
            finally
            {
                target.OrderCancelled -= _cancelledHandler;
                target.OrderFilled -= _filledHandler;
                target.OrderExpired -= _expiredHandler;
            }
        }

        [TestMethod]
        public void OrderExpired()
        {
            TradingAccount target = new SimulatedAccount();

            var expiredRaised = false;
            var cancelRaised = false;
            var filledRaised = false;
            _expiredHandler = (sender, e) => expiredRaised = (e.Expired < DateTime.Now) && (e.Order.Expiration == e.Expired);
            _cancelledHandler = (sender, e) => cancelRaised = true;
            _filledHandler = (sender, e) => filledRaised = true;
            try
            {
                target.OrderCancelled += _cancelledHandler;
                target.OrderFilled += _filledHandler;
                target.OrderExpired += _expiredHandler;

                var order = new Order(DateTime.Now, DateTime.Now.AddMilliseconds(SimulatedAccount.MinProcessingTimeSpan.Milliseconds / 2.0), OrderType.Buy, "DE", 5, 100.00m);
                target.Submit(order);

                Thread.Sleep(SimulatedAccount.MaxProcessingTimeSpan);
                Assert.IsTrue(expiredRaised);
                Assert.IsFalse(cancelRaised);
                Assert.IsFalse(filledRaised);
            }
            finally
            {
                target.OrderCancelled -= _cancelledHandler;
                target.OrderFilled -= _filledHandler;
                target.OrderExpired -= _expiredHandler;
            }
        }

        [TestMethod]
        public void OrderCancelled()
        {
            TradingAccount target = new SimulatedAccount();

            var cancelRaised = false;
            var expiredRaised = false;
            var filledRaised = false;
            _cancelledHandler = (sender, e) => cancelRaised = true;
            _filledHandler = (sender, e) => filledRaised = true;
            _expiredHandler = (sender, e) => expiredRaised = true;
            try
            {
                target.OrderCancelled += _cancelledHandler;
                target.OrderFilled += _filledHandler;
                target.OrderExpired += _expiredHandler;

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
                target.OrderCancelled -= _cancelledHandler;
                target.OrderFilled -= _filledHandler;
                target.OrderExpired -= _expiredHandler;
            }
        }
    }
}
