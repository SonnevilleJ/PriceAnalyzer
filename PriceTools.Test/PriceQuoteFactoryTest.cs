using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data;
using Sonneville.Utilities;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class PriceTickFactoryTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetValidPrice();
            var volume = GetValidVolume();

            var target = PriceTickFactory.ConstructPriceTick(settlementDate, price, volume);

            var xml = XmlSerializer.SerializeToXml(target);
            var result = XmlSerializer.DeserializeFromXml<IPriceTick>(xml);

            GenericTestUtilities.AssertSameState(target, result);
        }
        
        [TestMethod]
        public void SettlementDateTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetValidPrice();
            var volume = GetValidVolume();

            var target = PriceTickFactory.ConstructPriceTick(settlementDate, price, volume);

            Assert.AreEqual(settlementDate, target.SettlementDate);
        }

        [TestMethod]
        public void PriceValidTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetValidPrice();
            var volume = GetValidVolume();

            var target = PriceTickFactory.ConstructPriceTick(settlementDate, price, volume);

            Assert.AreEqual(price, target.Price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PriceInvalidTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetInvalidPrice();
            var volume = GetValidVolume();

            PriceTickFactory.ConstructPriceTick(settlementDate, price, volume);
        }

        [TestMethod]
        public void VolumeDefaultTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetValidPrice();

            var target = PriceTickFactory.ConstructPriceTick(settlementDate, price);

            Assert.IsNull(target.Volume);
        }

        [TestMethod]
        public void VolumeValidTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetValidPrice();
            var volume = GetValidVolume();

            var target = PriceTickFactory.ConstructPriceTick(settlementDate, price, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VolumeInvalidTest()
        {
            var settlementDate = GetSettlementDate();
            var price = GetValidPrice();
            var volume = GetInvalidVolume();

            PriceTickFactory.ConstructPriceTick(settlementDate, price, volume);
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
