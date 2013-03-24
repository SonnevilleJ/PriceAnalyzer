using System;
using Sonneville.PriceTools;
using Sonneville.Utilities.Persistence;

namespace PriceAnalyzer
{
    public class TransactionRepository
    {
        private readonly IRepository<Guid, ITransaction> _repository;

        public TransactionRepository(IRepository<Guid, ITransaction> repository)
        {
            _repository = repository;
        }

        public void Persist(ITransaction transaction)
        {
            _repository.Save(transaction.Id, transaction);
        }
    }
}