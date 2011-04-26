//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Sonneville.PriceTools;
//using Sonneville.Utilities;

//namespace Sonneville.PriceToolsTest
//{
    
    
//    /// <summary>
//    ///This is a test class for PricePeriodTest and is intended
//    ///to contain all PricePeriodTest Unit Tests
//    ///</summary>
//    [TestClass]
//    public class PricePeriodTest
//    {
//        [TestMethod]
//        public void SerializePricePeriodTest()
//        {
//            DateTime head = new DateTime(2010, 6, 1);
//            DateTime tail = new DateTime(2010, 8, 1);
//            const decimal open = 100.00m;
//            const decimal high = 120.00m;
//            const decimal low = 80.00m;
//            const decimal close = 110.00m;
//            const Int64 volume = 1000;

//            SummarizedPricePeriod target = new SummarizedPricePeriod
//                                      {
//                                          Head = head,
//                                          Tail = tail,
//                                          Open = open,
//                                          High = high,
//                                          Low = low,
//                                          Close = close,
//                                          Volume = volume
//                                      };

//            IPricePeriod actual = ((IPricePeriod) TestUtilities.Serialize(target));
//            Assert.AreEqual(head, actual.Head);
//            Assert.AreEqual(tail, actual.Tail);
//            Assert.AreEqual(open, actual.Open);
//            Assert.AreEqual(high, actual.High);
//            Assert.AreEqual(low, actual.Low);
//            Assert.AreEqual(close, actual.Close);
//            Assert.AreEqual(volume, actual.Volume);
//        }

//        [TestMethod]
//        public void EntityPricePeriodTest()
//        {
//            DateTime head = new DateTime(2010, 6, 1);
//            DateTime tail = new DateTime(2010, 8, 1);
//            const decimal open = 100.00m;
//            const decimal high = 120.00m;
//            const decimal low = 80.00m;
//            const decimal close = 110.00m;
//            const Int64 volume = 1000;

//            IPricePeriod target = new PricePeriod(head, tail, open, high, low, close, volume);

//            TestUtilities.VerifyPriceSeriesEntity(target);
//        }

//        [TestMethod]
//        public void PricePeriodConstructorTest()
//        {
//            DateTime d1 = new DateTime(2010, 6, 1);
//            DateTime d2 = new DateTime(2010, 8, 1);
//            const decimal open = 100.00m;
//            const decimal high = 120.00m;
//            const decimal low = 80.00m;
//            const decimal close = 110.00m;
//            const Int64 volume = 1000;

//            IPricePeriod target = new PricePeriod(d1, d2, open, high, low, close, volume);
            
//            Assert.IsTrue(target.Head == d1);
//            Assert.IsTrue(target.Tail == d2);
//            Assert.IsTrue(target.Open == open);
//            Assert.IsTrue(target.High == high);
//            Assert.IsTrue(target.Low == low);
//            Assert.IsTrue(target.Close == close);
//            Assert.IsTrue(target.Volume == volume);
//        }

//        [TestMethod]
//        public void PricePeriodConstructorTestNullVolume()
//        {
//            DateTime d1 = new DateTime(2010, 6, 1);
//            DateTime d2 = new DateTime(2010, 8, 1);
//            const decimal open = 100.00m;
//            const decimal high = 120.00m;
//            const decimal low = 80.00m;
//            const decimal close = 110.00m;

//            IPricePeriod target = new PricePeriod(d1, d2, open, high, low, close);
            
//            Assert.IsTrue(target.Head == d1);
//            Assert.IsTrue(target.Tail == d2);
//            Assert.IsTrue(target.Open == open);
//            Assert.IsTrue(target.High == high);
//            Assert.IsTrue(target.Low == low);
//            Assert.IsTrue(target.Close == close);
//            Assert.IsNull(target.Volume);
//        }

//        [TestMethod]
//        public void PricePeriodMismatchedDateTimes()
//        {
//            DateTime d1 = new DateTime(2010, 6, 1);
//            DateTime d2 = new DateTime(2010, 8, 1);
//            const decimal open = 100.00m;
//            const decimal high = 120.00m;
//            const decimal low = 80.00m;
//            const decimal close = 110.00m;

//            bool caught = false;

//            try
//            {
//                new PricePeriod(d2, d1, open, high, low, close);
//            }
//            catch
//            {
//                caught = true;
//            }

//            Assert.IsTrue(caught);
//        }

//        [TestMethod]
//        public void PricePeriodMismatchedHighLow()
//        {
//            DateTime d1 = new DateTime(2010, 6, 1);
//            DateTime d2 = new DateTime(2010, 8, 1);
//            const decimal open = 100.00m;
//            const decimal high = 120.00m;
//            const decimal low = 80.00m;
//            const decimal close = 110.00m;

//            bool caught = false;

//            try
//            {
//                new PricePeriod(d1, d2, open, low, high, close);
//            }
//            catch
//            {
//                caught = true;
//            }

//            Assert.IsTrue(caught);
//        }

//        [TestMethod]
//        public void PricePeriodMismatchedOpenHigh()
//        {
//            DateTime d1 = new DateTime(2010, 6, 1);
//            DateTime d2 = new DateTime(2010, 8, 1);
//            const decimal open = 100.00m;
//            const decimal high = 120.00m;
//            const decimal low = 80.00m;
//            const decimal close = 110.00m;

//            bool caught = false;

//            try
//            {
//                new PricePeriod(d1, d2, high, open, low, close);
//            }
//            catch
//            {
//                caught = true;
//            }

//            Assert.IsTrue(caught);
//        }

//        [TestMethod]
//        public void PricePeriodMismatchedOpenLow()
//        {
//            DateTime d1 = new DateTime(2010, 6, 1);
//            DateTime d2 = new DateTime(2010, 8, 1);
//            const decimal open = 100.00m;
//            const decimal high = 120.00m;
//            const decimal low = 80.00m;
//            const decimal close = 110.00m;

//            bool caught = false;

//            try
//            {
//                new PricePeriod(d1, d2, low, high, open, close);
//            }
//            catch
//            {
//                caught = true;
//            }

//            Assert.IsTrue(caught);
//        }

//    }
//}
