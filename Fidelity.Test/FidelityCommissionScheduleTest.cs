using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.Fidelity.Test
{
    [TestFixture]
    public class FidelityCommissionScheduleTest
    {
        private readonly IOrderFactory _orderFactory;

        public FidelityCommissionScheduleTest()
        {
            _orderFactory = new OrderFactory();
        }

        [Test]
        public void DefaultCommissionBuy()
        {
            var target = new FidelityCommissionSchedule();
            const OrderType orderType = OrderType.Buy;
            var order = _orderFactory.ConstructOrder(DateTime.Now, DateTime.Today.AddDays(1), orderType, "DE", 1, 100);

            const decimal expected = 7.95m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DefaultCommissionSell()
        {
            var target = new FidelityCommissionSchedule();
            const OrderType orderType = OrderType.Sell;
            var order = _orderFactory.ConstructOrder(DateTime.Now, DateTime.Today.AddDays(1), orderType, "DE", 1, 100);

            const decimal expected = 7.95m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DefaultCommissionSellShort()
        {
            var target = new FidelityCommissionSchedule();
            const OrderType orderType = OrderType.SellShort;
            var order = _orderFactory.ConstructOrder(DateTime.Now, DateTime.Today.AddDays(1), orderType, "DE", 1, 100);

            const decimal expected = 7.95m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DefaultCommissionBuyToCover()
        {
            var target = new FidelityCommissionSchedule();
            const OrderType orderType = OrderType.BuyToCover;
            var order = _orderFactory.ConstructOrder(DateTime.Now, DateTime.Today.AddDays(1), orderType, "DE", 1, 100);

            const decimal expected = 7.95m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }
    }
}
