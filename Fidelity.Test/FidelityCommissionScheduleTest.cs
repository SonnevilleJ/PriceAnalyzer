using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Test.Utilities;

namespace Sonneville.PriceTools.Fidelity.Test
{
    [TestClass]
    public class FidelityCommissionScheduleTest
    {
        [TestMethod]
        public void DefaultCommissionBuy()
        {
            var target = new FidelityCommissionSchedule();
            const OrderType orderType = OrderType.Buy;
            var order = OrderFactory.ConstructOrder(DateTime.Now, DateTime.Today.AddDays(1), orderType, TestUtilities.GetUniqueTicker(), 1, 100);

            const decimal expected = 7.95m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DefaultCommissionSell()
        {
            var target = new FidelityCommissionSchedule();
            const OrderType orderType = OrderType.Sell;
            var order = OrderFactory.ConstructOrder(DateTime.Now, DateTime.Today.AddDays(1), orderType, TestUtilities.GetUniqueTicker(), 1, 100);

            const decimal expected = 7.95m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DefaultCommissionSellShort()
        {
            var target = new FidelityCommissionSchedule();
            const OrderType orderType = OrderType.SellShort;
            var order = OrderFactory.ConstructOrder(DateTime.Now, DateTime.Today.AddDays(1), orderType, TestUtilities.GetUniqueTicker(), 1, 100);

            const decimal expected = 7.95m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DefaultCommissionBuyToCover()
        {
            var target = new FidelityCommissionSchedule();
            const OrderType orderType = OrderType.BuyToCover;
            var order = OrderFactory.ConstructOrder(DateTime.Now, DateTime.Today.AddDays(1), orderType, TestUtilities.GetUniqueTicker(), 1, 100);

            const decimal expected = 7.95m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }
    }
}
