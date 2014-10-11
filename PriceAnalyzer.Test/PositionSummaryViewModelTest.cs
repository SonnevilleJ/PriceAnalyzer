using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.PriceAnalyzer.Test
{
    [TestFixture]
    public class PositionSummaryViewModelTest
    {
        private TransactionFactory _transactionFactory;
        private PositionSummaryViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _transactionFactory = new TransactionFactory();
            _viewModel = new PositionSummaryViewModel();
            _viewModel.TransactionSummaries.Clear();
        }

        [Test]
        public void UpdateTransactionsUpdatesTransactionSummaries()
        {
            var ticker = "DE";
            var settlementDate = new DateTime(2014, 9, 1);
            var shares = 1;
            var price = 1;
            var buy = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            IEnumerable<TransactionSummary> summaries = null;
            _viewModel.TransactionSummaries.CollectionChanged += (sender, e) => summaries = _viewModel.TransactionSummaries;

            _viewModel.UpdateTransactions(new List<IShareTransaction> {buy});

            Assert.IsNotNull(summaries);
            var transactionSummary = summaries.Single();
            Assert.AreEqual(ticker, transactionSummary.Ticker);
            Assert.AreEqual(shares, transactionSummary.Volume);
            Assert.AreEqual(price, transactionSummary.BoughtPrice);
        }
    }
}