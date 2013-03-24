using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;

namespace Test.Sonneville.PriceTools.AutomatedTrading
{
    [TestClass]
    public class OrderExecutedEventArgsTest
    {
        private static readonly IOrderFactory OrderFactory;
        private static readonly ITransactionFactory TransactionFactory;

        static OrderExecutedEventArgsTest()
        {
            OrderFactory = new OrderFactory();
            TransactionFactory = new TransactionFactory();
        }

        private static void GetObjects(out DateTime executed, out IOrder order, out IShareTransaction transaction, out OrderExecutedEventArgs target)
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const decimal shares = 5;
            const decimal price = 100.00m;
            const decimal commission = 5.00m;

            executed = issued.AddSeconds(3);
            order = OrderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);
            transaction = TransactionFactory.ConstructShareTransaction(orderType, ticker, executed, shares, price, commission);
            target = new OrderExecutedEventArgs(executed, order, transaction);
        }

        [TestMethod]
        public void ExecutedTest()
        {
            DateTime executed;
            IOrder order;
            IShareTransaction transaction;
            OrderExecutedEventArgs target;
            GetObjects(out executed, out order, out transaction, out target);

            var expected = executed;
            var actual = target.Executed;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OrderTest()
        {
            DateTime executed;
            IOrder order;
            IShareTransaction transaction;
            OrderExecutedEventArgs target;
            GetObjects(out executed, out order, out transaction, out target);

            var expected = order;
            var actual = target.Order;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TransactionTest()
        {
            DateTime executed;
            IOrder order;
            IShareTransaction transaction;
            OrderExecutedEventArgs target;
            GetObjects(out executed, out order, out transaction, out target);

            var expected = transaction;
            var actual = target.Transaction;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExecutedAfterExpiredFails()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const decimal shares = 5;
            const decimal price = 100.00m;
            const decimal commission = 5.00m;

            var executed = expired.AddTicks(1);
            var order = OrderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);
            var transaction = TransactionFactory.ConstructShareTransaction(orderType, ticker, executed, shares, price, commission);
            
            new OrderExecutedEventArgs(executed, order, transaction);
        }
    }
}
