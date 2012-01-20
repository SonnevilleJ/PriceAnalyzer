﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PriceQuoteFactoryTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetValidPrice();
            var volume = GetValidVolume();

            var target = PriceQuoteFactory.ConstructPriceQuote(settlementDate, price, volume);

            var xml = Serializer.SerializeToXml(target);
            var result = Serializer.DeserializeFromXml<PriceQuote>(xml);

            TestUtilities.AssertSameState(target, result);
        }
        
        [TestMethod]
        public void SettlementDateTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetValidPrice();
            var volume = GetValidVolume();

            var target = PriceQuoteFactory.ConstructPriceQuote(settlementDate, price, volume);

            Assert.AreEqual(settlementDate, target.SettlementDate);
        }

        [TestMethod]
        public void PriceValidTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetValidPrice();
            var volume = GetValidVolume();

            var target = PriceQuoteFactory.ConstructPriceQuote(settlementDate, price, volume);

            Assert.AreEqual(price, target.Price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PriceInvalidTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetInvalidPrice();
            var volume = GetValidVolume();

            PriceQuoteFactory.ConstructPriceQuote(settlementDate, price, volume);
        }

        [TestMethod]
        public void VolumeValidTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetValidPrice();
            var volume = GetValidVolume();

            var target = PriceQuoteFactory.ConstructPriceQuote(settlementDate, price, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VolumeInvalidTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetValidPrice();
            var volume = GetInvalidVolume();

            PriceQuoteFactory.ConstructPriceQuote(settlementDate, price, volume);
        }

        [TestMethod]
        public void ToStringTest()
        {
            var settlementDate = new DateTime(2011, 12, 28);
            const decimal price = 10.00m;
            const long volume = 300;

            var target = PriceQuoteFactory.ConstructPriceQuote(settlementDate, price, volume);

            var actual = target.ToString();

            Assert.IsTrue(actual.Contains(settlementDate.ToString()));
            Assert.IsTrue(actual.Contains(price.ToString()));
        }

        private static DateTime GetSettlementDate()
        {
            return new DateTime(2012, 1, 19);
        }

        private static decimal GetValidPrice()
        {
            return 100.00m;
        }

        private static decimal GetInvalidPrice()
        {
            return -GetValidPrice();
        }

        private static long GetValidVolume()
        {
            return 5;
        }

        private static long GetInvalidVolume()
        {
            return -GetValidVolume();
        }
    }
}
