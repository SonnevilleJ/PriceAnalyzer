using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Sonneville.PriceToolsTest
{
    [TestClass()]
    public class MovingAverageTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod]
        public void FlatPeriodReturnsSameAverage()
        {
            DateTime date = new DateTime(2000, 1, 1);
            PricePeriod p0 = new PricePeriod(date, date, 1, 1, 1, 1);
            PricePeriod p1 = new PricePeriod(date.AddDays(1), date.AddDays(1), 2, 2, 2, 2);
            PricePeriod p2 = new PricePeriod(date.AddDays(2), date.AddDays(2), 3, 3, 3, 3);
            PricePeriod p3 = new PricePeriod(date.AddDays(3), date.AddDays(3), 4, 4, 4, 4);
            PricePeriod p4 = new PricePeriod(date.AddDays(4), date.AddDays(4), 5, 5, 5, 5);
            PricePeriod p5 = new PricePeriod(date.AddDays(5), date.AddDays(5), 6, 6, 6, 6);
            PricePeriod p6 = new PricePeriod(date.AddDays(6), date.AddDays(6), 7, 7, 7, 7);
            PricePeriod p7 = new PricePeriod(date.AddDays(7), date.AddDays(7), 8, 8, 8, 8);
            PricePeriod p8 = new PricePeriod(date.AddDays(8), date.AddDays(8), 9, 9, 9, 9);
            PricePeriod p9 = new PricePeriod(date.AddDays(9), date.AddDays(9), 10, 10, 10, 10);

            PriceSeries series = new PriceSeries(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);

            // create 4 day moving average
            const int range = 4;
            int span = series.Periods.Count - (range - 1);
            MovingAverage avg = new MovingAverage(series, range);
            Assert.IsTrue(avg.Range == range);
            avg.CalculateAll();
            Assert.IsTrue(avg.Last == 8.5m);
            Assert.IsTrue(avg[date.AddDays(3)] == 2.5m);
            Assert.IsTrue(avg[date.AddDays(4)] == 3.5m);
            Assert.IsTrue(avg[date.AddDays(5)] == 4.5m);
            Assert.IsTrue(avg[date.AddDays(6)] == 5.5m);
            Assert.IsTrue(avg[date.AddDays(7)] == 6.5m);
            Assert.IsTrue(avg[date.AddDays(8)] == 7.5m);
            Assert.IsTrue(avg[date.AddDays(9)] == 8.5m);
            Assert.IsTrue(avg.Span == span);
        }
    }
}
