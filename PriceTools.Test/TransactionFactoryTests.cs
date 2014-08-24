using System;
using NUnit.Framework;
using Sonneville.PriceTools.Implementation;
using Sonneville.Utilities.Serialization;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class TransactionFactoryTests
    {
        private readonly ITransactionFactory _factory;

        public TransactionFactoryTests()
        {
            _factory = new TransactionFactory();
        }

        [Test]
        public void FactoryAddsID()
        {
            var buy = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);

            Assert.IsNotNull(buy.Id);
        }

        [Test]
        public void AddsSameIdForSameTransaction()
        {
            var buy1 = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);
            var buy2 = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);

            Assert.AreEqual(buy1.Id, buy2.Id);
        }

        [Test]
        public void AddsDifferentIdForSameTransaction()
        {
            var buy = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);
            var sell = _factory.ConstructSell("DE", DateTime.Today, 5, 10, 0);

            Assert.AreNotEqual(buy.Id, sell.Id);
        }

        [Test]
        public void DifferentFactoriesResultInDifferentIDs()
        {
            var otherFactory = new TransactionFactory(Guid.NewGuid());

            var buy1 = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);
            var buy2 = otherFactory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);

            Assert.AreNotEqual(buy1.Id, buy2.Id);
        }

        [Test]
        public void SameFactoryGuidResultsInSameID()
        {
            var otherFactory = new TransactionFactory(Guid.Parse("26491456-7E65-421B-B4C3-984527311CED"));

            var buy1 = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);
            var buy2 = otherFactory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);

            Assert.AreEqual(buy1.Id, buy2.Id);
        }

        [Test]
        public void SerializedTransactionHasSameID()
        {
            var buy = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);

            var serialized = XmlSerializer.SerializeToXml(buy);
            var deserialized = XmlSerializer.DeserializeFromXml<Buy>(serialized);

            Assert.AreEqual(buy.Id, deserialized.Id);
        }

        [Test]
        public void SerializedTwiceHasSameID()
        {
            var buy = _factory.ConstructBuy("DE", DateTime.Today, 5, 10, 0);

            var serialized = XmlSerializer.SerializeToXml(buy);
            var deserialized = XmlSerializer.DeserializeFromXml<Buy>(serialized);

            serialized = XmlSerializer.SerializeToXml(deserialized);
            deserialized = XmlSerializer.DeserializeFromXml<Buy>(serialized);

            Assert.AreEqual(buy.Id, deserialized.Id);
        }
    }
}
