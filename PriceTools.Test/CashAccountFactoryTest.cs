using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class CashAccountFactoryTest
    {
        private readonly ICashAccountFactory _cashAccountFactory;

        public CashAccountFactoryTest()
        {
            _cashAccountFactory = new CashAccountFactory();
        }

        [Test]
        public void MarginableCashAccountAssignMaxMargin()
        {
            const decimal maximumMargin = 100;

            var target = _cashAccountFactory.ConstructMarginableCashAccount(maximumMargin);

            Assert.AreEqual(maximumMargin, target.MaximumMargin);
        }
    }
}