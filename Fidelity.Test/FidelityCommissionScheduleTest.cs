using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Fidelity;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools.Fidelity
{
    [TestClass]
    public class FidelityCommissionScheduleTest
    {
        private readonly IOrderFactory _orderFactory;

        public FidelityCommissionScheduleTest()
        {
            _orderFactory = new OrderFactory();
        }

        [TestMethod]
        public void DefaultCommissionBuy()
        {
            var target = new FidelityCommissionSchedule();
            const OrderType orderType = OrderType.Buy;
            var order = _orderFactory.ConstructOrder(DateTime.Now, DateTime.Today.AddDays(1), orderType, TickerManager.GetUniqueTicker(), 1, 100);

            const decimal expected = 7.95m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DefaultCommissionSell()
        {
            var target = new FidelityCommissionSchedule();
            const OrderType orderType = OrderType.Sell;
            var order = _orderFactory.ConstructOrder(DateTime.Now, DateTime.Today.AddDays(1), orderType, TickerManager.GetUniqueTicker(), 1, 100);

            const decimal expected = 7.95m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DefaultCommissionSellShort()
        {
            var target = new FidelityCommissionSchedule();
            const OrderType orderType = OrderType.SellShort;
            var order = _orderFactory.ConstructOrder(DateTime.Now, DateTime.Today.AddDays(1), orderType, TickerManager.GetUniqueTicker(), 1, 100);

            const decimal expected = 7.95m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DefaultCommissionBuyToCover()
        {
            var target = new FidelityCommissionSchedule();
            const OrderType orderType = OrderType.BuyToCover;
            var order = _orderFactory.ConstructOrder(DateTime.Now, DateTime.Today.AddDays(1), orderType, TickerManager.GetUniqueTicker(), 1, 100);

            const decimal expected = 7.95m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }
    }
}
