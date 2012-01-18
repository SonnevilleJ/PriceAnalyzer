using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class TransactionFactoryTest
    {
        public abstract class CashTransactionTestsBase
        {
            public abstract void SerializeTest();

            /// <summary>
            ///A test for Amount
            ///</summary>
            public abstract void AmountInvertedTest();

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            public abstract void SettlementDateTest();

            /// <summary>
            ///A test for OrderType
            ///</summary>
            public abstract void OrderTypeTest();

            /// <summary>
            ///A test for Amount
            ///</summary>
            public abstract void AmountCorrectTest();

            protected static void CashTransactionSerializeTest(OrderType transactionType)
            {
                var date = new DateTime(2012, 1, 17);
                const decimal amount = 10000.00m;

                var target = TransactionFactory.ConstructCashTransaction(transactionType, date, amount);

                var xml = Serializer.SerializeToXml(target);
                var result = Serializer.DeserializeFromXml<ICashTransaction>(xml);

                TestUtilities.AssertSameState(target, result);
            }

            protected static void CashTransactionSettlementDateTest(OrderType transactionType)
            {
                var date = new DateTime(2000, 1, 1);
                const decimal price = 2.00m;

                var target = TransactionFactory.ConstructCashTransaction(transactionType, date, price);

                var expected = date;
                var actual = target.SettlementDate;
                Assert.AreEqual(expected, actual);
            }

            protected static void CashTransactionOrderTypeTest(OrderType transactionType)
            {
                var date = new DateTime(2000, 1, 1);
                const decimal price = 2.00m;

                var target = TransactionFactory.ConstructCashTransaction(transactionType, date, price);

                var expected = transactionType;
                var actual = target.OrderType;
                Assert.AreEqual(expected, actual);
            }

            protected static void CashTransactionAmountInvertedTest(OrderType transactionType)
            {
                var date = new DateTime(2000, 1, 1);
                var price = GetInversePrice(transactionType);

                var target = TransactionFactory.ConstructCashTransaction(transactionType, date, price);

                var expected = GetCorrectPrice(transactionType);
                var actual = target.Amount;
                Assert.AreEqual(expected, actual);
            }

            protected static void CashTransactionAmountCorrectTest(OrderType transactionType)
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
                    case OrderType.Deposit:
                    case OrderType.DividendReceipt:
                        return 2.00m;
                    case OrderType.Withdrawal:
                        return -2.00m;
                    default:
                        return 0.00m;
                }
            }

            private static decimal GetInversePrice(OrderType transactionType)
            {
                return -GetCorrectPrice(transactionType);
            }
        }

        [TestClass]
        public class DepositTests : CashTransactionTestsBase
        {
            private const OrderType TransactionType = OrderType.Deposit;

            [TestMethod]
            public override void SerializeTest()
            {
                CashTransactionSerializeTest(TransactionType);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public override void AmountInvertedTest()
            {
                CashTransactionAmountInvertedTest(TransactionType);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public override void SettlementDateTest()
            {
                CashTransactionSettlementDateTest(TransactionType);
            }

            /// <summary>
            ///A test for OrderType
            ///</summary>
            [TestMethod]
            public override void OrderTypeTest()
            {
                CashTransactionOrderTypeTest(TransactionType);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public override void AmountCorrectTest()
            {
                CashTransactionAmountCorrectTest(TransactionType);
            }
        }

        [TestClass]
        public class WithdrawalTests : CashTransactionTestsBase
        {
            private const OrderType TransactionType = OrderType.Withdrawal;

            [TestMethod]
            public override void SerializeTest()
            {
                CashTransactionSerializeTest(TransactionType);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public override void AmountInvertedTest()
            {
                CashTransactionAmountInvertedTest(TransactionType);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public override void SettlementDateTest()
            {
                CashTransactionSettlementDateTest(TransactionType);
            }

            /// <summary>
            ///A test for OrderType
            ///</summary>
            [TestMethod]
            public override void OrderTypeTest()
            {
                CashTransactionOrderTypeTest(TransactionType);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public override void AmountCorrectTest()
            {
                CashTransactionAmountCorrectTest(TransactionType);
            }
        }

        [TestClass]
        public class DividendReceiptTests : CashTransactionTestsBase
        {
            private const OrderType TransactionType = OrderType.DividendReceipt;

            [TestMethod]
            public override void SerializeTest()
            {
                CashTransactionSerializeTest(TransactionType);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public override void AmountInvertedTest()
            {
                CashTransactionAmountInvertedTest(TransactionType);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public override void SettlementDateTest()
            {
                CashTransactionSettlementDateTest(TransactionType);
            }

            /// <summary>
            ///A test for OrderType
            ///</summary>
            [TestMethod]
            public override void OrderTypeTest()
            {
                CashTransactionOrderTypeTest(TransactionType);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public override void AmountCorrectTest()
            {
                CashTransactionAmountCorrectTest(TransactionType);
            }
        }
    }
}
