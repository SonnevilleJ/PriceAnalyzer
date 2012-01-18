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
                CashTransactionSerializeTest(OrderType.DividendReceipt);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public void DividendReceiptAmountNegativeTest()
            {
                CashTransactionAmountInvertedTest(OrderType.DividendReceipt);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public void DividendReceiptSettlementDateTest()
            {
                CashTransactionSettlementDateTest(OrderType.DividendReceipt);
            }

            /// <summary>
            ///A test for OrderType
            ///</summary>
            [TestMethod]
            public void DividendReceiptOrderTypeTest()
            {
                CashTransactionOrderTypeTest(OrderType.DividendReceipt);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public void DividendReceiptAmountPositiveTest()
            {
                CashTransactionAmountCorrectTest(OrderType.DividendReceipt);
            }
        }

        #region Test runners

        private static void CashTransactionSerializeTest(OrderType transactionType)
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var target = TransactionFactory.ConstructCashTransaction(transactionType, date, amount);

            var xml = Serializer.SerializeToXml(target);
            var result = Serializer.DeserializeFromXml<ICashTransaction>(xml);

            TestUtilities.AssertSameState(target, result);
        }

        private static void CashTransactionSettlementDateTest(OrderType transactionType)
        {
            var date = new DateTime(2000, 1, 1);
            const decimal price = 2.00m;

            var target = TransactionFactory.ConstructCashTransaction(transactionType, date, price);

            var expected = date;
            var actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        private static void CashTransactionOrderTypeTest(OrderType transactionType)
        {
            var date = new DateTime(2000, 1, 1);
            const decimal price = 2.00m;

            var target = TransactionFactory.ConstructCashTransaction(transactionType, date, price);

            var expected = transactionType;
            var actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        private static void CashTransactionAmountInvertedTest(OrderType transactionType)
        {
            var date = new DateTime(2000, 1, 1);
            var price = GetInversePrice(transactionType);

            var target = TransactionFactory.ConstructCashTransaction(transactionType, date, price);

            var expected = GetCorrectPrice(transactionType);
            var actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        private static void CashTransactionAmountCorrectTest(OrderType transactionType)
        {
            var date = new DateTime(2000, 1, 1);
            var price = GetCorrectPrice(transactionType);

            var target = TransactionFactory.ConstructCashTransaction(transactionType, date, price);

            var expected = GetCorrectPrice(transactionType);
            var actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        private static decimal GetCorrectPrice(OrderType transactionType)
        {
            switch (transactionType)
            {
                case OrderType.DividendReceipt:
                    return 2.00m;
                default:
                    return 0.00m;
            }
        }

        private static decimal GetInversePrice(OrderType transactionType)
        {
            return -GetCorrectPrice(transactionType);
        }

        #endregion

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
