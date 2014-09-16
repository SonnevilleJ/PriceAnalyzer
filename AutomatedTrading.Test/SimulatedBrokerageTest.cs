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
    }
}