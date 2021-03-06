﻿using System;
using NUnit.Framework;
using Sonneville.PriceTools.Implementation;
using Sonneville.PriceTools.TestUtilities;
using Sonneville.Utilities.Serialization;

namespace Sonneville.PriceTools.Test
{
    public abstract class CashTransactionTestsBase
    {
        private static readonly ITransactionFactory _transactionFactory;

        static CashTransactionTestsBase()
        {
            _transactionFactory = new TransactionFactory();
        }

        [Test]
        public abstract void SerializeTest();

        [Test]
        public abstract void AmountInvalidTest();

        [Test]
        public abstract void SettlementDateTest();

        [Test]
        public abstract void AmountValidTest();

        protected static void CashTransactionSerializeTest(OrderType transactionType)
        {
            var date = GetSettlementDate();
            var amount = GetValidAmount(transactionType);

            var target = _transactionFactory.ConstructCashTransaction(transactionType, date, amount);

            var xml = XmlSerializer.SerializeToXml(target);
            var result = XmlSerializer.DeserializeFromXml<CashTransaction>(xml);

            GenericTestUtilities.AssertSameReflectedProperties(target, result);
        }

        protected static void CashTransactionSettlementDateTest(OrderType transactionType)
        {
            var date = GetSettlementDate();
            var amount = GetValidAmount(transactionType);

            var target = _transactionFactory.ConstructCashTransaction(transactionType, date, amount);

            var expected = date;
            var actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        protected static void CashTransactionAmountInvalidTest(OrderType transactionType)
        {
            var date = GetSettlementDate();
            var amount = GetInversePrice(transactionType);

            var target = _transactionFactory.ConstructCashTransaction(transactionType, date, amount);

            var expected = GetValidAmount(transactionType);
            var actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        protected static void CashTransactionAmountValidTest(OrderType transactionType)
        {
            var date = GetSettlementDate();
            var amount = GetValidAmount(transactionType);

            var target = _transactionFactory.ConstructCashTransaction(transactionType, date, amount);

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