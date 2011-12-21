﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace TradingTest
{
    [TestClass]
    public class OrderExecutedEventArgsTest
    {
        private static void GetObjects(out DateTime executed, out Order order, out ShareTransaction transaction, out OrderExecutedEventArgs target)
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = 100.00m;
            const decimal commission = 5.00m;

            executed = issued.AddSeconds(3);
            order = new Order(issued, expired, orderType, ticker, shares, price);
            transaction = TransactionFactory.Instance.CreateShareTransaction(executed, orderType, ticker, price, shares, commission);
            target = new OrderExecutedEventArgs(executed, order, transaction);
        }

        [TestMethod]
        public void ExecutedTest()
        {
            DateTime executed;
            Order order;
            ShareTransaction transaction;
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
            Order order;
            ShareTransaction transaction;
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
            Order order;
            ShareTransaction transaction;
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
            const double shares = 5.0;
            const decimal price = 100.00m;
            const decimal commission = 5.00m;

            var executed = expired.AddTicks(1);
            var order = new Order(issued, expired, orderType, ticker, shares, price);
            var transaction = TransactionFactory.Instance.CreateShareTransaction(executed, orderType, ticker, price, shares, commission);
            
            new OrderExecutedEventArgs(executed, order, transaction);
        }
    }
}