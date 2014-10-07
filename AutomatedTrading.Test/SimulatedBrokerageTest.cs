using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class SimulatedBrokerageTest
    {
        private SimulatedBrokerage _simulatedBroker;

        [SetUp]
        public void Setup()
        {
            _simulatedBroker = new SimulatedBrokerage();
        }

        [Test]
        public void GetOpenOrdersReturnsZeroOrdersByDefault()
        {
            var openOrders = _simulatedBroker.GetOpenOrders();

            CollectionAssert.IsEmpty(openOrders);
        }

        [Test]
        public void GetOpenOrdersReturnsPreviouslySubmittedOrders()
        {
            var expectedOrders = new List<Order>
            {
                new Order(),
                new Order()
            };
            foreach(var order in expectedOrders)
            {
                _simulatedBroker.SubmitOrders(new List<Order> {order});
            }

            var actualOrders = _simulatedBroker.GetOpenOrders();

            CollectionAssert.AreEquivalent(expectedOrders, actualOrders);
        }

        [Test]
        public void CanceledOrderDoesNotAppearInListOfOpenOrders()
        {
            var expectedOrders = new List<Order>
            {
                new Order
                {
                    Ticker = "DE"
                },
                new Order
                {
                    Ticker = "IBM"
                }
            };
            _simulatedBroker.SubmitOrders(expectedOrders);
            
            _simulatedBroker.CancelOrder(expectedOrders.First());

            var openOrders = _simulatedBroker.GetOpenOrders();
            CollectionAssert.DoesNotContain(openOrders, expectedOrders.First());
            CollectionAssert.Contains(openOrders, expectedOrders.Last());
        }

        [Test]
        public void CancelOrderDoesNothingIfOrderHasNotBeenSubmitted()
        {
            var expectedOrders = new List<Order>
            {
                new Order
                {
                    Ticker = "DE"
                },
                new Order
                {
                    Ticker = "IBM"
                }
            };
            _simulatedBroker.SubmitOrders(expectedOrders);
            
            _simulatedBroker.CancelOrder(new Order {Ticker = "MSFT"});

            var openOrders = _simulatedBroker.GetOpenOrders();
            CollectionAssert.AreEquivalent(openOrders, expectedOrders);
        }

        [Test]
        public void GetAllOrdersReturnsAllSubmittedOrders()
        {
            var expectedOrders = new List<Order>
            {
                new Order
                {
                    Ticker = "DE"
                },
                new Order
                {
                    Ticker = "IBM"
                }
            };
            _simulatedBroker.SubmitOrders(expectedOrders);
            _simulatedBroker.CancelOrder(expectedOrders.First());

            var actualOrders = _simulatedBroker.GetAllOrders();

            CollectionAssert.AreEquivalent(expectedOrders, actualOrders);
        }

        [Test]
        public void GetAllOrdersReturnsOnlySubmittedOrders()
        {
            var expectedOrders = new List<Order>
            {
                new Order
                {
                    Ticker = "DE"
                },
                new Order
                {
                    Ticker = "IBM"
                }
            };
            _simulatedBroker.SubmitOrders(expectedOrders);
            _simulatedBroker.CancelOrder(new Order {Ticker = "MSFT"});

            var actualOrders = _simulatedBroker.GetAllOrders();

            CollectionAssert.AreEquivalent(expectedOrders, actualOrders);
        }

        [Test]
        public void GetTransactionsConvertsOrdersOlderThan1DayToTransactions()
        {
            var startDate = new DateTime(2014, 9, 1);
            var expectedOrders = new List<Order>
            {
                new Order
                {
                    OrderType = OrderType.Buy,
                    Ticker = "DE",
                    Issued = startDate
                },
                new Order
                {
                    Ticker = "IBM",
                    Issued = new DateTime(2014, 10, 1)
                }
            };
            _simulatedBroker.SubmitOrders(expectedOrders);

            var transaction = _simulatedBroker.GetTransactions(DateTime.MinValue, startDate.AddDays(1)).Single();
            Assert.AreEqual(expectedOrders.First().Ticker, transaction.Ticker);

            var openOrder = _simulatedBroker.GetOpenOrders().Single();
            Assert.AreEqual(expectedOrders.Last(), openOrder);
        }

        [Test]
        public void GetTransactionsReturnsAllPreviousTransactions()
        {
            var firstOrderTime = new DateTime(2014, 9, 1);
            var secondOrderTime = new DateTime(2014, 10, 1);
            var expectedOrders = new List<Order>
            {
                new Order
                {
                    OrderType = OrderType.Buy,
                    Ticker = "DE",
                    Issued = firstOrderTime
                },
                new Order
                {
                    Ticker = "IBM",
                    Issued = secondOrderTime
                }
            };
            _simulatedBroker.SubmitOrders(expectedOrders);

            _simulatedBroker.GetTransactions(DateTime.MinValue, firstOrderTime.AddDays(1));
            var transactions = _simulatedBroker.GetTransactions(DateTime.MinValue, secondOrderTime.AddDays(1));

            Assert.AreEqual(2, transactions.Count());
            Assert.AreEqual(expectedOrders.First().Ticker, transactions.First().Ticker);
            Assert.AreEqual(expectedOrders.Last().Ticker, transactions.Last().Ticker);

            var openOrders = _simulatedBroker.GetOpenOrders();
            CollectionAssert.IsEmpty(openOrders);
        }
    }
}