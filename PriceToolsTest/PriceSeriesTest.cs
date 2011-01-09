using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    /// Summary description for PriceSeriesTest
    /// </summary>
    [TestClass]
    public class PriceSeriesTest
    {
        public PriceSeriesTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ConstructPriceSeriesFromSmallerPricePeriods()
        {
            PricePeriod p1 = new PricePeriod(DateTime.Parse("1/1/2010"), DateTime.Parse("1/2/2010"), 10, 12, 10, 11, 50);
            PricePeriod p2 = new PricePeriod(DateTime.Parse("1/2/2010"), DateTime.Parse("1/3/2010"), 11, 13, 10, 13, 60);
            PricePeriod p3 = new PricePeriod(DateTime.Parse("1/3/2010"), DateTime.Parse("1/4/2010"), 13, 14, 9, 11, 80);

            IPricePeriod target = new PriceSeries(p1, p2, p3);

            Assert.IsTrue(target.TimeSpan == new TimeSpan(3, 0, 0, 0));
            Assert.IsTrue(target.Open == 10);
            Assert.IsTrue(target.High == 14);
            Assert.IsTrue(target.Low == 9);
            Assert.IsTrue(target.Close == 11);
            Assert.IsTrue(target.Volume == 190);
        }

        [TestMethod]
        public void PriceSeriesAddSubsequentPricePeriodTest()
        {
            IPricePeriod p1 = new PricePeriod(DateTime.Parse("1/1/2010"), DateTime.Parse("1/2/2010"), 10, 12, 10, 11, 50);
            IPricePeriod p2 = new PricePeriod(DateTime.Parse("1/2/2010"), DateTime.Parse("1/3/2010"), 11, 13, 10, 13, 60);
            IPricePeriod p3 = new PricePeriod(DateTime.Parse("1/3/2010"), DateTime.Parse("1/4/2010"), 13, 14, 9, 11, 80);
            IPricePeriod p4 = new PricePeriod(DateTime.Parse("1/4/2010"), DateTime.Parse("1/5/2010"), 12, 15, 11, 14, 55);

            IPriceSeries target = new PriceSeries(p1, p2, p3);

            Assert.IsTrue(target.TimeSpan == new TimeSpan(3, 0, 0, 0));
            Assert.IsTrue(target.Open == 10);
            Assert.IsTrue(target.High == 14);
            Assert.IsTrue(target.Low == 9);
            Assert.IsTrue(target.Close == 11);
            Assert.IsTrue(target.Volume == 190);

            target.InsertPeriod(p4);

            Assert.IsTrue(target.TimeSpan == new TimeSpan(4, 0, 0, 0));
            Assert.IsTrue(target.Open == 10);
            Assert.IsTrue(target.High == 15);
            Assert.IsTrue(target.Low == 9);
            Assert.IsTrue(target.Close == 14);
            Assert.IsTrue(target.Volume == 245);
        }

        [TestMethod]
        public void PriceSeriesAddPriorPricePeriodTest()
        {
            IPricePeriod p1 = new PricePeriod(DateTime.Parse("1/6/2011"), DateTime.Parse("1/6/2011"), 10, 12, 10, 11, 50);
            IPricePeriod p2 = new PricePeriod(DateTime.Parse("1/7/2011"), DateTime.Parse("1/7/2011"), 11, 13, 10, 13, 60);

            PriceSeries series = new PriceSeries(p2);
            Assert.IsTrue(series.Open == p2.Open);

            series.InsertPeriod(p1);
            Assert.IsTrue(series.Open == p1.Open);
        }

        [TestMethod]
        public void BinarySerializePriceSeriesTest()
        {
            IPricePeriod p1 = new PricePeriod(DateTime.Parse("1/1/2010"), DateTime.Parse("1/2/2010"), 10, 12, 10, 11, 50);
            IPricePeriod p2 = new PricePeriod(DateTime.Parse("1/2/2010"), DateTime.Parse("1/3/2010"), 11, 13, 10, 13, 60);
            IPricePeriod p3 = new PricePeriod(DateTime.Parse("1/3/2010"), DateTime.Parse("1/4/2010"), 13, 14, 9, 11, 80);

            PriceSeries expected = new PriceSeries(p1, p2, p3);

            TestUtilities.VerifySerialization(expected);
        }

        [TestMethod]
        public void PriceSeriesSortTest()
        {
            DateTime d1 = DateTime.Parse("1/1/2010");
            DateTime d2 = DateTime.Parse("1/2/2010");
            DateTime d3 = DateTime.Parse("1/3/2010");
            IPricePeriod p1 = new PricePeriod(d1, d1, 10, 12, 10, 11, 50);
            IPricePeriod p2 = new PricePeriod(d2, d2, 11, 13, 10, 13, 60);
            IPricePeriod p3 = new PricePeriod(d3, d3, 13, 14, 9, 11, 80);

            IPriceSeries period = new PriceSeries(p3, p1, p2);
            Assert.IsTrue(period[d1] == p1);
            Assert.IsTrue(period[d2] == p2);
            Assert.IsTrue(period[d3] == p3);
        }

        /// <summary>
        ///A test for Span
        ///</summary>
        [TestMethod()]
        public void SpanTest()
        {
            DateTime head = new DateTime(2011, 1, 6);
            DateTime tail = new DateTime(2011, 1, 6);
            const decimal open = 1.0m;
            const decimal high = 1.5m;
            const decimal low = 0.5m;
            const decimal close = 1.0m;

            const int max = 10;
            PriceSeries target = new PriceSeries(new PricePeriod(head, tail, open, high, low, close));
            for (int i = 1; i < max; i++)
            {
                Assert.IsTrue(target.Periods.Count == i);
                target.InsertPeriod(new PricePeriod(head.AddDays(i), tail.AddDays(i), open, high, low, close));
            }
            Assert.IsTrue(target.Periods.Count == max);
        }
    }
}
