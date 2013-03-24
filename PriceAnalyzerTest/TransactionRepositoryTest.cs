using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PriceAnalyzer;
using Sonneville.PriceTools;
using Sonneville.Utilities.Persistence;

namespace PriceAnalyzerTest
{
    [TestClass]
    public class TransactionRepositoryTest
    {
        [TestMethod]
        public void GenerateKeyForBuy()
        {
            var buy = TransactionFactory.ConstructBuy("DE", DateTime.Today, 100, 10, 7.95m);
            var repository = new Mock<IRepository<Guid, ITransaction>>();
            var testObject = new TransactionRepository(repository.Object);
            buy.Id = Guid.NewGuid();

            testObject.Persist(buy);

            repository.Verify(x => x.Save(buy.Id, buy));
        }
    }
}
