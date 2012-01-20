using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PricePeriodFactoryTest
    {
        [TestMethod]
        public void CreateStaticPricePeriodHeadAndWeeklyResolution()
        {
            var head = new DateTime(2011, 7, 4);
            var tail = new DateTime(2011, 7, 8, 23, 59, 59, 999);
            const decimal open = 50.00m;
            const decimal high = 65.00m;
            const decimal low = 45.00m;
            const decimal close = 60.00m;
            const long volume = 100;

            var target = PricePeriodFactory.CreateStaticPricePeriod(head, Resolution.Weeks, open, high, low, close, volume);

            Assert.AreEqual(head, target.Head);
            Assert.AreEqual(tail, target.Tail);
            Assert.AreEqual(open, target.Open);
            Assert.AreEqual(high, target.High);
            Assert.AreEqual(low, target.Low);
            Assert.AreEqual(close, target.Close);
            Assert.AreEqual(volume, target.Volume);
        }

        /// <summary>
        ///A test for CreateStaticPricePeriod
        ///</summary>
        [TestMethod]
        public void CreateStaticPricePeriodTest1()
        {
            var head = new DateTime(2011, 7, 4);
            var tail = new DateTime(2011, 7, 4, 23, 59, 59, 999);
            const decimal open = 50.00m;
            const decimal high = 65.00m;
            const decimal low = 45.00m;
            const decimal close = 60.00m;
            const long volume = 100;

            var target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(head, target.Head);
            Assert.AreEqual(tail, target.Tail);
            Assert.AreEqual(open, target.Open);
            Assert.AreEqual(high, target.High);
            Assert.AreEqual(low, target.Low);
            Assert.AreEqual(close, target.Close);
            Assert.AreEqual(volume, target.Volume);
        }

        /// <summary>
        ///A test for CreateStaticPricePeriod
        ///</summary>
        [TestMethod]
        public void CreateStaticPricePeriodTest2()
        {
            var head = new DateTime(2011, 7, 4);
            var tail = new DateTime(2011, 7, 4, 23, 59, 59, 999);
            const decimal close = 60.00m;

            var target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, null, null, null, close);

            Assert.AreEqual(head, target.Head);
            Assert.AreEqual(tail, target.Tail);
            Assert.AreEqual(close, target.Open);
            Assert.AreEqual(close, target.High);
            Assert.AreEqual(close, target.Low);
            Assert.AreEqual(close, target.Close);
            Assert.AreEqual(null, target.Volume);
        }

        public abstract class QuotedPricePeriodFactoryTestsBase
        {
            public abstract void ConstructQuotedPricePeriodQuotesCountTest();

            public abstract void ConstructQuotedPricePeriodHeadTest();

            public abstract void ConstructQuotedPricePeriodTailTest();

            public abstract void ConstructQuotedPricePeriodOpenTest();

            public abstract void ConstructQuotedPricePeriodHighTest();

            public abstract void ConstructQuotedPricePeriodLowTest();

            public abstract void ConstructQuotedPricePeriodCloseTest();

            public abstract void ConstructQuotedPricePeriodVolumeTest();

            public abstract QuotedPricePeriod CallFactoryMethod();
        }

        [TestClass]
        public class QuotedPricePeriodFactoryConstructor1Tests : QuotedPricePeriodFactoryTestsBase
        {
            [TestMethod]
            public override void ConstructQuotedPricePeriodQuotesCountTest()
            {
                var target = CallFactoryMethod();

                Assert.AreEqual(0, target.PriceQuotes.Count);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public override void ConstructQuotedPricePeriodHeadTest()
            {
                var target = CallFactoryMethod();

                var head = target.Head;
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public override void ConstructQuotedPricePeriodTailTest()
            {
                var target = CallFactoryMethod();

                var tail = target.Tail;
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public override void ConstructQuotedPricePeriodOpenTest()
            {
                var target = CallFactoryMethod();

                var open = target.Open;
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public override void ConstructQuotedPricePeriodHighTest()
            {
                var target = CallFactoryMethod();

                var high = target.High;
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public override void ConstructQuotedPricePeriodLowTest()
            {
                var target = CallFactoryMethod();

                var low = target.Low;
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public override void ConstructQuotedPricePeriodCloseTest()
            {
                var target = CallFactoryMethod();

                var close = target.Close;
            }

            [TestMethod]
            public override void ConstructQuotedPricePeriodVolumeTest()
            {
                var target = CallFactoryMethod();

                const long expected = 0;
                var actual = target.Volume;
                Assert.AreEqual(expected, actual);
            }

            public override QuotedPricePeriod CallFactoryMethod()
            {
                return PricePeriodFactory.ConstructQuotedPricePeriod();
            }
        }

        [TestClass]
        public class QuotedPricePeriodFactoryConstructor2Tests : QuotedPricePeriodFactoryTestsBase
        {
            [TestMethod]
            public override void ConstructQuotedPricePeriodQuotesCountTest()
            {
                var target = CallFactoryMethod();

                Assert.AreEqual(PriceQuotes.Count(), target.PriceQuotes.Count);
            }

            [TestMethod]
            public override void ConstructQuotedPricePeriodHeadTest()
            {
                var target = CallFactoryMethod();

                var expected = PriceQuotes.Min(pq => pq.SettlementDate);
                var actual = target.Head;
                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public override void ConstructQuotedPricePeriodTailTest()
            {
                var target = CallFactoryMethod();

                var expected = PriceQuotes.Max(pq => pq.SettlementDate);
                var actual = target.Tail;
                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public override void ConstructQuotedPricePeriodOpenTest()
            {
                var target = CallFactoryMethod();

                var expected = PriceQuotes.First().Price;
                var actual = target.Open;
                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public override void ConstructQuotedPricePeriodHighTest()
            {
                var target = CallFactoryMethod();

                var expected = PriceQuotes.Max(pq => pq.Price);
                var actual = target.High;
                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public override void ConstructQuotedPricePeriodLowTest()
            {
                var target = CallFactoryMethod();

                var expected = PriceQuotes.Min(pq => pq.Price);
                var actual = target.Low;
                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public override void ConstructQuotedPricePeriodCloseTest()
            {
                var target = CallFactoryMethod();

                var expected = PriceQuotes.Last().Price;
                var actual = target.Close;
                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public override void ConstructQuotedPricePeriodVolumeTest()
            {
                var target = CallFactoryMethod();

                var expected = PriceQuotes.Sum(pq => pq.Volume);
                var actual = target.Volume;
                Assert.AreEqual(expected, actual);
            }

            public override QuotedPricePeriod CallFactoryMethod()
            {
                return PricePeriodFactory.ConstructQuotedPricePeriod(PriceQuotes);
            }

            private static IEnumerable<PriceQuote> PriceQuotes
            {
                get
                {
                    var quote1 = TestUtilities.CreateQuote1();
                    var quote2 = TestUtilities.CreateQuote2();
                    var quote3 = TestUtilities.CreateQuote3();
                    return new[] {quote1, quote2, quote3};
                }
            }
        }
    }
}