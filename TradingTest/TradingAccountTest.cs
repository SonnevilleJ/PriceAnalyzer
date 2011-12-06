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
        private EventHandler<OrderExecutedEventArgs> _eventHandler;

        [TestMethod]
        public void EmptyPositionsByDefault()
        {
            TradingAccount target = new SimulatedAccount();

            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void BuyFillsOrder()
        {
            TradingAccount target = new SimulatedAccount();

            const string ticker = "DE";
            var eventRaised = false;
            _eventHandler = (sender, e) => eventRaised = e.Transaction.Ticker == ticker;
            try
            {
                target.OrderFilled += _eventHandler;

                var order = new Order(DateTime.Now, DateTime.Now.AddDays(1), OrderType.Buy, ticker, 5, 100.00m);
                target.Submit(order);

                Thread.Sleep(SimulatedAccount.MaxTimeout);
                Assert.IsTrue(eventRaised);
            }
            finally
            {
                target.OrderFilled -= _eventHandler;
            }
        }
    }
}
