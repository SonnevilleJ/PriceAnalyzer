using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class HoldingExtensionsTests
    {
        [TestMethod]
        public void GrossProfitTest()
        {
            var target = new Holding {Shares = 5, OpenPrice = 10, ClosePrice = 20, OpenCommission = 2, CloseCommission = 3};

            var expected = (target.Shares*(target.ClosePrice - target.OpenPrice));
            var actual = target.GrossProfit();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NetProfitTest()
        {
            var target = new Holding {Shares = 5, OpenPrice = 10, ClosePrice = 20, OpenCommission = 2, CloseCommission = 3};

            var expected = (target.Shares * (target.ClosePrice - target.OpenPrice)) - target.OpenCommission - target.CloseCommission;
            var actual = target.NetProfit();
            Assert.AreEqual(expected, actual);
        }
    }
}
