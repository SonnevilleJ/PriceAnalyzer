using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class CashAccountFactoryTest
    {
        [TestMethod]
        public void MarginableCashAccountAssignMaxMargin()
        {
            const decimal maximumMargin = 100;

            var target = CashAccountFactory.ConstructMarginableCashAccount(maximumMargin);

            Assert.AreEqual(maximumMargin, target.MaximumMargin);
        }
    }
}