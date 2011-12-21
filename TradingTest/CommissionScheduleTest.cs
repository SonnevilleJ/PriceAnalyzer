using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace Sonneville.TradingTest
{
    [TestClass]
    public class CommissionScheduleTest
    {
        [TestMethod]
        public void DefaultCommissionBuy()
        {
            var target = new FidelityCommissionSchedule();
            var order = new Order(DateTime.Now, DateTime.Today.AddDays(1), OrderType.Buy, "DE", 1, 100);

            const decimal expected = 0.00m;
            var actual = target.PriceCheck(order);

            Assert.AreEqual(expected, actual);
        }
    }
}
