using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class TransactionFactoryTest
    {
        [TestMethod]
        public void ConstructDepositTest()
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var target = TransactionFactory.ConstructDeposit(date, amount);

            Assert.AreEqual(OrderType.Deposit, target.OrderType);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(Math.Abs(amount), Math.Abs(target.Amount));
        }

        [TestMethod]
        public void ConstructWithdrawalTest()
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var target = TransactionFactory.ConstructWithdrawal(date, amount);

            Assert.AreEqual(OrderType.Withdrawal, target.OrderType);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(Math.Abs(amount), Math.Abs(target.Amount));
        }

        [TestClass]
        public class DividendReceiptTests
        {
            [TestMethod]
            public void SerializeTest()
            {
                var date = new DateTime(2012, 1, 17);
                const decimal amount = 10000.00m;

                var target = TransactionFactory.ConstructDividendReceipt(date, amount);

                var xml = Serializer.SerializeToXml(target);
                var result = Serializer.DeserializeFromXml<ICashTransaction>(xml);

                TestUtilities.AssertSameState(target, result);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public void DividendReceiptAmountNegativeTest()
            {
                var date = new DateTime(2000, 1, 1);
                const decimal price = -2.0m;        // bought at $-2.00 per share - error

                var target = TransactionFactory.ConstructDividendReceipt(date, price);

                var expected = Math.Abs(price);
                var actual = target.Amount;
                Assert.AreEqual(expected, actual);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public void DividendReceiptSettlementDateTest()
            {
                var date = new DateTime(2000, 1, 1);
                const decimal price = 2.00m;

                var target = TransactionFactory.ConstructDividendReceipt(date, price);

                var expected = date;
                var actual = target.SettlementDate;
                Assert.AreEqual(expected, actual);
            }

            /// <summary>
            ///A test for OrderType
            ///</summary>
            [TestMethod]
            public void DividendReceiptOrderTypeTest()
            {
                var date = new DateTime(2000, 1, 1);
                const decimal price = 2.00m;

                var target = TransactionFactory.ConstructDividendReceipt(date, price);

                const OrderType expected = OrderType.DividendReceipt;
                var actual = target.OrderType;
                Assert.AreEqual(expected, actual);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public void DividendReceiptAmountPositiveTest()
            {
                var date = new DateTime(2000, 1, 1);
                const decimal price = 2.00m;        // received $2.00 per share

                var target = TransactionFactory.ConstructDividendReceipt(date, price);

                const decimal expected = price;
                var actual = target.Amount;
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void ConstructCashTransactionDepositTest()
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var expected = TransactionFactory.ConstructDeposit(date, amount);
            var actual = TransactionFactory.ConstructCashTransaction(OrderType.Deposit, date, amount);
            TestUtilities.AssertSameState(expected, actual);
        }

        [TestMethod]
        public void ConstructCashTransactionWithdrawalTest()
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var expected = TransactionFactory.ConstructWithdrawal(date, amount);
            var actual = TransactionFactory.ConstructCashTransaction(OrderType.Withdrawal, date, amount);
            TestUtilities.AssertSameState(expected, actual);
        }

        [TestMethod]
        public void ConstructCashTransactionDividendReceiptTest()
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var expected = TransactionFactory.ConstructDividendReceipt(date, amount);
            var actual = TransactionFactory.ConstructCashTransaction(OrderType.DividendReceipt, date, amount);
            TestUtilities.AssertSameState(expected, actual);
        }
    }
}
