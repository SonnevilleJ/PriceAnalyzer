﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    public abstract class CashTransactionTestsBase
    {
        /// <summary>
        /// A test for serialization
        /// </summary>
        [TestMethod]
        public abstract void SerializeTest();

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public abstract void AmountInvalidTest();

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [TestMethod]
        public abstract void SettlementDateTest();

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public abstract void AmountValidTest();

        protected static void CashTransactionSerializeTest(OrderType transactionType)
        {
            var date = GetSettlementDate();
            var amount = GetValidAmount(transactionType);

            var target = TransactionFactory.ConstructCashTransaction(transactionType, date, amount);

            var xml = Serializer.SerializeToXml(target);
            var result = Serializer.DeserializeFromXml<CashTransaction>(xml);

            GenericTestUtilities.AssertSameState(target, result);
        }

        protected static void CashTransactionSettlementDateTest(OrderType transactionType)
        {
            var date = GetSettlementDate();
            var amount = GetValidAmount(transactionType);

            var target = TransactionFactory.ConstructCashTransaction(transactionType, date, amount);

            var expected = date;
            var actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        protected static void CashTransactionAmountInvalidTest(OrderType transactionType)
        {
            var date = GetSettlementDate();
            var amount = GetInversePrice(transactionType);

            var target = TransactionFactory.ConstructCashTransaction(transactionType, date, amount);

            var expected = GetValidAmount(transactionType);
            var actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        protected static void CashTransactionAmountValidTest(OrderType transactionType)
        {
            var date = GetSettlementDate();
            var amount = GetValidAmount(transactionType);

            var target = TransactionFactory.ConstructCashTransaction(transactionType, date, amount);

            var expected = GetValidAmount(transactionType);
            var actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        private static DateTime GetSettlementDate()
        {
            return new DateTime(2000, 1, 1);
        }

        private static decimal GetValidAmount(OrderType transactionType)
        {
            switch (transactionType)
            {
                case OrderType.Deposit:
                case OrderType.DividendReceipt:
                    return 2.00m;
                case OrderType.Withdrawal:
                    return -2.00m;
                default:
                    throw new ArgumentOutOfRangeException("transactionType", transactionType, string.Format(@"Unknown OrderType: {0}.", transactionType));
            }
        }

        private static decimal GetInversePrice(OrderType transactionType)
        {
            return -GetValidAmount(transactionType);
        }
    }
}