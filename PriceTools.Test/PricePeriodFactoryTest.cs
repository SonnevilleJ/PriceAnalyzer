using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    public abstract class StaticPricePeriodFactoryConstructorTestsBase
    {
        protected readonly IPricePeriodFactory PricePeriodFactory;

        protected StaticPricePeriodFactoryConstructorTestsBase()
        {
            PricePeriodFactory = new PricePeriodFactory();
        }

        public abstract void CreateStaticPricePeriodHeadTest();

        public abstract void CreateStaticPricePeriodTailTest();

        public virtual void CreateStaticPricePeriodValidOpenTest()
        {
        }

        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public virtual void CreateStaticPricePeriodInvalidOpenTest()
        {
        }

        [ExpectedException(typeof (InvalidOperationException))]
        public virtual void CreateStaticPricePeriodInvalidOpenAboveHighTest()
        {
        }

        [ExpectedException(typeof (InvalidOperationException))]
        public virtual void CreateStaticPricePeriodInvalidOpenBelowLowTest()
        {
        }

        public virtual void CreateStaticPricePeriodValidHighTest()
        {
        }

        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public virtual void CreateStaticPricePeriodInvalidHighTest()
        {
        }

        public virtual void CreateStaticPricePeriodValidLowTest()
        {
        }

        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public virtual void CreateStaticPricePeriodInvalidLowTest()
        {
        }

        [ExpectedException(typeof (InvalidOperationException))]
        public virtual void CreateStaticPricePeriodHighLowReversedTest()
        {
        }

        public abstract void CreateStaticPricePeriodValidCloseTest();

        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public virtual void CreateStaticPricePeriodInvalidCloseTest()
        {
        }

        [ExpectedException(typeof (InvalidOperationException))]
        public virtual void CreateStaticPricePeriodInvalidCloseAboveHighTest()
        {
        }

        [ExpectedException(typeof (InvalidOperationException))]
        public virtual void CreateStaticPricePeriodInvalidCloseBelowLowTest()
        {
        }

        public abstract void CreateStaticPricePeriodDefaultVolumeTest();

        public abstract void CreateStaticPricePeriodValidVolumeTest();

        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public abstract void CreateStaticPricePeriodInvalidVolumeTest();

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

        protected static decimal GetInvalidOpen()
        {
            return -GetValidOpen();
        }

        protected static decimal GetInvalidOpenAboveHigh()
        {
            return GetValidHigh() + 1;
        }

        protected static decimal GetInvalidOpenBelowLow()
        {
            return GetValidLow() - 1;
        }

        protected static decimal GetValidHigh()
        {
            return 65.00m;
        }

        protected static decimal GetInvalidHigh()
        {
            return -GetValidHigh();
        }

        protected static decimal GetValidLow()
        {
            return 45.00m;
        }

        protected static decimal GetInvalidLow()
        {
            return -GetValidLow();
        }

        protected static decimal GetValidClose()
        {
            return 60.00m;
        }

        protected static decimal GetInvalidClose()
        {
            return -GetValidClose();
        }

        protected static decimal GetInvalidCloseAboveHigh()
        {
            return GetValidHigh() + 1;
        }

        protected static decimal GetInvalidCloseBelowLow()
        {
            return GetValidLow() - 1;
        }

        protected static long GetValidVolume()
        {
            return 100;
        }

        protected static long GetInvalidVolume()
        {
            return -GetValidVolume();
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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);

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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);

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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(open, target.Open);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidOpenTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetInvalidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public override void CreateStaticPricePeriodInvalidOpenAboveHighTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetInvalidOpenAboveHigh();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public override void CreateStaticPricePeriodInvalidOpenBelowLowTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetInvalidOpenBelowLow();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);
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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(high, target.High);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidHighTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetInvalidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);
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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(low, target.Low);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidLowTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetInvalidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public override void CreateStaticPricePeriodHighLowReversedTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidLow();
            var low = GetValidHigh();
            var close = GetValidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);
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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(close, target.Close);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidCloseTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetInvalidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public override void CreateStaticPricePeriodInvalidCloseAboveHighTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetInvalidCloseAboveHigh();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public override void CreateStaticPricePeriodInvalidCloseBelowLowTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetInvalidCloseBelowLow();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);
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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close);

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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidVolumeTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetInvalidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);
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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, open, high, low, close, volume);

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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, close, volume);

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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, close, volume);

            Assert.AreEqual(tail, target.Tail);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidOpenTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, close, volume);

            Assert.AreEqual(close, target.Open);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidHighTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, close, volume);

            Assert.AreEqual(close, target.High);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidLowTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, close, volume);

            Assert.AreEqual(close, target.Low);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidCloseTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, close, volume);

            Assert.AreEqual(close, target.Close);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidCloseTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetInvalidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, close, volume);
        }

        public override void CreateStaticPricePeriodDefaultVolumeTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, close, volume);

            Assert.IsNull(target.Volume);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidVolumeTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, close, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidVolumeTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetInvalidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, close, volume);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodResolutionTest()
        {
            var head = GetHead();
            var resolution = GetResolution();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, resolution, close, volume);

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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(open, target.Open);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidOpenTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetInvalidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public override void CreateStaticPricePeriodInvalidOpenAboveHighTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetInvalidOpenAboveHigh();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public override void CreateStaticPricePeriodInvalidOpenBelowLowTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetInvalidOpenBelowLow();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(high, target.High);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidHighTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetInvalidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(low, target.Low);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidLowTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetInvalidLow();
            var close = GetValidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public override void CreateStaticPricePeriodHighLowReversedTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidLow();
            var low = GetValidHigh();
            var close = GetValidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(close, target.Close);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidCloseTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetInvalidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public override void CreateStaticPricePeriodInvalidCloseAboveHighTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetInvalidCloseAboveHigh();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public override void CreateStaticPricePeriodInvalidCloseBelowLowTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetInvalidCloseBelowLow();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close);

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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidVolumeTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var open = GetValidOpen();
            var high = GetValidHigh();
            var low = GetValidLow();
            var close = GetValidClose();
            var volume = GetInvalidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

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

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, close, volume);

            Assert.AreEqual(head, target.Head);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodTailTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, close, volume);

            Assert.AreEqual(tail, target.Tail);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidOpenTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, close, volume);

            Assert.AreEqual(close, target.Open);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidHighTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, close, volume);

            Assert.AreEqual(close, target.High);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidLowTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, close, volume);

            Assert.AreEqual(close, target.Low);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidCloseTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, close, volume);

            Assert.AreEqual(close, target.Close);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidCloseTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetInvalidClose();
            var volume = GetValidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, tail, close, volume);
        }

        public override void CreateStaticPricePeriodDefaultVolumeTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, close, volume);

            Assert.IsNull(target.Volume);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodValidVolumeTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, close, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CreateStaticPricePeriodInvalidVolumeTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetInvalidVolume();

            PricePeriodFactory.ConstructStaticPricePeriod(head, tail, close, volume);
        }

        [TestMethod]
        public override void CreateStaticPricePeriodResolutionTest()
        {
            var head = GetHead();
            var tail = GetTail();
            var close = GetValidClose();
            var volume = GetValidVolume();
            var resolution = GetResolution();

            var target = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, close, volume);

            Assert.AreEqual(resolution, target.Resolution);
        }
    }
}