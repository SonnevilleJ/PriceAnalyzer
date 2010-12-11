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
            DateTime head1 = new DateTime(2000, 1, 1);
            PricePeriod p0 = new PricePeriod(head1, head1, 1, 1, 1, 1);
            PricePeriod p1 = new PricePeriod(head1.AddDays(1), head1.AddDays(1), 2, 2, 2, 2);
            PricePeriod p2 = new PricePeriod(head1.AddDays(2), head1.AddDays(2), 3, 3, 3, 3);
            PricePeriod p3 = new PricePeriod(head1.AddDays(3), head1.AddDays(3), 4, 4, 4, 4);
            PricePeriod p4 = new PricePeriod(head1.AddDays(4), head1.AddDays(4), 5, 5, 5, 5);
            PricePeriod p5 = new PricePeriod(head1.AddDays(5), head1.AddDays(5), 6, 6, 6, 6);
            PricePeriod p6 = new PricePeriod(head1.AddDays(6), head1.AddDays(6), 7, 7, 7, 7);
            PricePeriod p7 = new PricePeriod(head1.AddDays(7), head1.AddDays(7), 8, 8, 8, 8);
            PricePeriod p8 = new PricePeriod(head1.AddDays(8), head1.AddDays(8), 9, 9, 9, 9);
            PricePeriod p9 = new PricePeriod(head1.AddDays(9), head1.AddDays(9), 10, 10, 10, 10);

            PriceSeries series = new PriceSeries(PriceSeriesResolution.Days, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);

            // create 4 day moving average
            MovingAverage avg = new MovingAverage(series, 4);
            avg.CalculateAll();
            Assert.IsTrue(avg.Last == 8.5m);
            Assert.IsTrue(avg[0] == 8.5m);
            Assert.IsTrue(avg[-1] == 7.5m);
            Assert.IsTrue(avg[-2] == 6.5m);
            Assert.IsTrue(avg[-3] == 5.5m);
            Assert.IsTrue(avg[-4] == 4.5m);
            Assert.IsTrue(avg[-5] == 3.5m);
            Assert.IsTrue(avg[-6] == 2.5m);
            Assert.IsTrue(avg.Span == 7);
        }
    }
}
