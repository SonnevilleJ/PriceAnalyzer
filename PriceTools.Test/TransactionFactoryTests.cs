using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class TransactionFactoryTests
    {
        private readonly ITransactionFactory _factory;

        public TransactionFactoryTests()
        {
            _factory = new TransactionFactory();
        }

        [TestMethod]
        public void FactoryAddsID()
        {
            var buy = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);

            Assert.IsNotNull(buy.Id);
        }

        [TestMethod]
        public void AddsSameIdForSameTransaction()
        {
            var buy1 = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);
            var buy2 = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);

            Assert.AreEqual(buy1.Id, buy2.Id);
        }

        [TestMethod]
        public void AddsDifferentIdForSameTransaction()
        {
            var buy = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);
            var sell = _factory.ConstructSell("DE", DateTime.Today, 5, 10, 0);

            Assert.AreNotEqual(buy.Id, sell.Id);
        }

        [TestMethod]
        public void DifferentFactoriesResultInDifferentIDs()
        {
            var otherFactory = new TransactionFactory(Guid.NewGuid());

            var buy1 = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);
            var buy2 = otherFactory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);

            Assert.AreNotEqual(buy1.Id, buy2.Id);
        }

        [TestMethod]
        public void SameFactoryGuidResultsInSameID()
        {
            var otherFactory = new TransactionFactory(Guid.Parse("26491456-7E65-421B-B4C3-984527311CED"));

            var buy1 = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);
            var buy2 = otherFactory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);

            Assert.AreEqual(buy1.Id, buy2.Id);
        }
    }
}
