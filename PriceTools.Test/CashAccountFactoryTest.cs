using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class CashAccountFactoryTest
    {
        private readonly ICashAccountFactory _cashAccountFactory;

        public CashAccountFactoryTest()
        {
            _cashAccountFactory = new CashAccountFactory();
        }

        [TestMethod]
        public void MarginableCashAccountAssignMaxMargin()
        {
            const decimal maximumMargin = 100;

            var target = _cashAccountFactory.ConstructMarginableCashAccount(maximumMargin);

            Assert.AreEqual(maximumMargin, target.MaximumMargin);
        }
    }
}