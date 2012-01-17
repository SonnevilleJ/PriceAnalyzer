using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class DepositTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var target = new Deposit {SettlementDate = date, Amount = amount};

            var xml = Serializer.SerializeToXml(target);
            var result = Serializer.DeserializeFromXml<ICashTransaction>(xml);

            TestUtilities.AssertSameState(target, result);
        }

        /// <summary>
        ///A test for Date
        ///</summary>
        [TestMethod]
        public void DepositSettlementDateTest()
        {
            var date = new DateTime(2011, 1, 9);

            var target = new Deposit
                             {
                                 SettlementDate = date,
                             };

            var expected = date;
            var actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for OrderType
        ///</summary>
        [TestMethod]
        public void DepositOrderTypeTest()
        {
            var target = new Deposit();

            const OrderType expected = OrderType.Deposit;
            var actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public void DepositAmountPositiveTest()
        {
            const decimal amount = 1000.00m;

            var target = new Deposit
                             {
                                 Amount = amount
                             };

            const decimal expected = 1000.00m;
            var actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public void DepositAmountNegativeTest()
        {
            const decimal amount = -1000.00m;

            var target = new Deposit
                             {
                                 Amount = amount
                             };

            const decimal expected = 1000.00m;
            var actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }
    }
}
