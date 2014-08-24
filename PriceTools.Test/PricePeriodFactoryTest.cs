using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    public abstract class StaticPricePeriodFactoryConstructorTestsBase
    {
        public abstract void CreateStaticPricePeriodHeadTest();

        public abstract void CreateStaticPricePeriodTailTest();

        public virtual void CreateStaticPricePeriodValidOpenTest()
        {
        }

        public virtual void CreateStaticPricePeriodValidHighTest()
        {
        }

        public virtual void CreateStaticPricePeriodValidLowTest()
        {
        }

        public abstract void CreateStaticPricePeriodValidCloseTest();

        public abstract void CreateStaticPricePeriodDefaultVolumeTest();

        public abstract void CreateStaticPricePeriodValidVolumeTest();

        public abstract void CreateStaticPricePeriodResolutionTest();

        protected static DateTime GetHead()
        {
            return new DateTime(2011, 7, 4);
        }

        protected static DateTime GetTail()
        {
            return new DateTime(2011, 7, 8, 23, 59, 59, 999);
        }

        protected static Resolution GetResolution()
        {
            return Resolution.Weeks;
        }

        protected static decimal GetValidOpen()
        {
            return 50.00m;
        }

        protected static decimal GetValidHigh()
        {
            return 65.00m;
        }

        protected static decimal GetValidLow()
        {
            return 45.00m;
        }

        protected static decimal GetValidClose()
        {
            return 60.00m;
        }

        protected static long GetValidVolume()
        {
            return 100;
        }
    }

    [TestClass]
    public class StaticPricePeriodFactoryConstructor4Tests : StaticPricePeriodFactoryConstructorTestsBase
    {
        [TestMethod]
        public override void CreateStaticPricePeriodHeadTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(head, target.Head);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodTailTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(tail, target.Tail);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidOpenTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(open, target.Open);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidHighTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(high, target.High);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidLowTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(low, target.Low);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidCloseTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(close, target.Close);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodDefaultVolumeTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();

            var target = new PricePeriod(head, resolution, open, high, low, close);

            Assert.IsNull(target.Volume);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidVolumeTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodResolutionTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(resolution, target.Resolution);
        }
    }

    [TestClass]
    public class StaticPricePeriodFactoryConstructor3Tests : StaticPricePeriodFactoryConstructorTestsBase
    {
        [TestMethod]
        public override void CreateStaticPricePeriodHeadTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, close, volume);

            Assert.AreEqual(head, target.Head);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodTailTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, close, volume);

            Assert.AreEqual(tail, target.Tail);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidOpenTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, close, volume);

            Assert.AreEqual(close, target.Open);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidHighTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, close, volume);

            Assert.AreEqual(close, target.High);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidLowTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, close, volume);

            Assert.AreEqual(close, target.Low);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidCloseTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, close, volume);

            Assert.AreEqual(close, target.Close);
        }

        public override void CreateStaticPricePeriodDefaultVolumeTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, close, volume);

            Assert.IsNull(target.Volume);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidVolumeTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, close, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodResolutionTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, resolution, close, volume);

            Assert.AreEqual(resolution, target.Resolution);
        }
    }

    [TestClass]
    public class StaticPricePeriodFactoryConstructor2Tests : StaticPricePeriodFactoryConstructorTestsBase
    {
        [TestMethod]
        public override void CreateStaticPricePeriodHeadTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(head, target.Head);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodTailTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(tail, target.Tail);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidOpenTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(open, target.Open);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidHighTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(high, target.High);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidLowTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(low, target.Low);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidCloseTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(close, target.Close);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodDefaultVolumeTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();

            var target = new PricePeriod(head, tail, open, high, low, close, null);

            Assert.IsNull(target.Volume);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidVolumeTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodResolutionTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();
            var resolution = GetResolution();

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(resolution, target.Resolution);
        }
    }

    [TestClass]
    public class StaticPricePeriodFactoryConstructor1Tests : StaticPricePeriodFactoryConstructorTestsBase
    {
        [TestMethod]
        public override void CreateStaticPricePeriodHeadTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, close, volume);

            Assert.AreEqual(head, target.Head);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodTailTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, close, volume);

            Assert.AreEqual(tail, target.Tail);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidOpenTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, close, volume);

            Assert.AreEqual(close, target.Open);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidHighTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, close, volume);

            Assert.AreEqual(close, target.High);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidLowTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, close, volume);

            Assert.AreEqual(close, target.Low);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidCloseTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, close, volume);

            Assert.AreEqual(close, target.Close);
        }

        public override void CreateStaticPricePeriodDefaultVolumeTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, close, volume);

            Assert.IsNull(target.Volume);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidVolumeTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = new PricePeriod(head, tail, close, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodResolutionTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();
            var resolution = GetResolution();

            var target = new PricePeriod(head, tail, close, volume);

            Assert.AreEqual(resolution, target.Resolution);
        }
    }
}