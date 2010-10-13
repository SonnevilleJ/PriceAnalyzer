using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Sonneville.PriceToolsTest
{
    
    
    /// <summary>
    ///This is a test class for PricePeriodTest and is intended
    ///to contain all PricePeriodTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PricePeriodTest
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

        [TestMethod()]
        public void BinarySerializePricePeriodTest()
        {
            DateTime d1 = new DateTime(2010, 6, 1);
            DateTime d2 = new DateTime(2010, 8, 1);
            const decimal open = 100.00m;
            const decimal high = 120.00m;
            const decimal low = 80.00m;
            const decimal close = 110.00m;
            const UInt64 volume = 1000;

            PricePeriod period = new PricePeriod(d1, d2, open, high, low, close, volume);

            MemoryStream stream = new MemoryStream();
            PricePeriod.BinarySerialize(period, stream);
            stream.Position = 0;
            PricePeriod result = PricePeriod.BinaryDeserialize(stream);
            Assert.AreEqual(result, period);
        }

        [TestMethod()]
        public void PricePeriodConstructorTest()
        {
            DateTime d1 = new DateTime(2010, 6, 1);
            DateTime d2 = new DateTime(2010, 8, 1);
            const decimal open = 100.00m;
            const decimal high = 120.00m;
            const decimal low = 80.00m;
            const decimal close = 110.00m;
            const UInt64 volume = 1000;

            PricePeriod target = new PricePeriod(d1, d2, open, high, low, close, volume);
            
            Assert.IsTrue(target.Head == d1);
            Assert.IsTrue(target.Tail == d2);
            Assert.IsTrue(target.Open == open);
            Assert.IsTrue(target.High == high);
            Assert.IsTrue(target.Low == low);
            Assert.IsTrue(target.Close == close);
            Assert.IsTrue(target.Volume == volume);
        }

        [TestMethod()]
        public void PricePeriodConstructorTestNullVolume()
        {
            DateTime d1 = new DateTime(2010, 6, 1);
            DateTime d2 = new DateTime(2010, 8, 1);
            const decimal open = 100.00m;
            const decimal high = 120.00m;
            const decimal low = 80.00m;
            const decimal close = 110.00m;

            PricePeriod target = new PricePeriod(d1, d2, open, high, low, close);
            
            Assert.IsTrue(target.Head == d1);
            Assert.IsTrue(target.Tail == d2);
            Assert.IsTrue(target.Open == open);
            Assert.IsTrue(target.High == high);
            Assert.IsTrue(target.Low == low);
            Assert.IsTrue(target.Close == close);
            Assert.IsNull(target.Volume);
        }

        [TestMethod()]
        public void PricePeriodMismatchedDateTimes()
        {
            DateTime d1 = new DateTime(2010, 6, 1);
            DateTime d2 = new DateTime(2010, 8, 1);
            const decimal open = 100.00m;
            const decimal high = 120.00m;
            const decimal low = 80.00m;
            const decimal close = 110.00m;

            bool caught = false;

            try
            {
                PricePeriod target = new PricePeriod(d2, d1, open, high, low, close);
            }
            catch
            {
                caught = true;
            }

            Assert.IsTrue(caught);
        }

        [TestMethod()]
        public void PricePeriodMismatchedHighLow()
        {
            DateTime d1 = new DateTime(2010, 6, 1);
            DateTime d2 = new DateTime(2010, 8, 1);
            const decimal open = 100.00m;
            const decimal high = 120.00m;
            const decimal low = 80.00m;
            const decimal close = 110.00m;

            bool caught = false;

            try
            {
                PricePeriod target = new PricePeriod(d1, d2, open, low, high, close);
            }
            catch
            {
                caught = true;
            }

            Assert.IsTrue(caught);
        }

        [TestMethod()]
        public void PricePeriodMismatchedOpenHigh()
        {
            DateTime d1 = new DateTime(2010, 6, 1);
            DateTime d2 = new DateTime(2010, 8, 1);
            const decimal open = 100.00m;
            const decimal high = 120.00m;
            const decimal low = 80.00m;
            const decimal close = 110.00m;

            bool caught = false;

            try
            {
                PricePeriod target = new PricePeriod(d1, d2, high, open, low, close);
            }
            catch
            {
                caught = true;
            }

            Assert.IsTrue(caught);
        }

        [TestMethod()]
        public void PricePeriodMismatchedOpenLow()
        {
            DateTime d1 = new DateTime(2010, 6, 1);
            DateTime d2 = new DateTime(2010, 8, 1);
            const decimal open = 100.00m;
            const decimal high = 120.00m;
            const decimal low = 80.00m;
            const decimal close = 110.00m;

            bool caught = false;

            try
            {
                PricePeriod target = new PricePeriod(d1, d2, low, high, open, close);
            }
            catch
            {
                caught = true;
            }

            Assert.IsTrue(caught);
        }

    }
}
