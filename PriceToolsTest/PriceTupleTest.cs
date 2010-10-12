using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Summary description for PriceTupleTest
    /// </summary>
    [TestClass]
    public class PriceTupleTest
    {
        public PriceTupleTest()
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
        public void ConstructPriceTupleFromSmallerPricePeriods()
        {
            PricePeriod p1, p2, p3;
            p1 = new PricePeriod(DateTime.Parse("1/1/2010"), DateTime.Parse("1/2/2010"), 10, 12, 10, 11, 50);
            p2 = new PricePeriod(DateTime.Parse("1/2/2010"), DateTime.Parse("1/3/2010"), 11, 13, 10, 13, 60);
            p3 = new PricePeriod(DateTime.Parse("1/3/2010"), DateTime.Parse("1/4/2010"), 13, 14, 9, 11, 80);

            PricePeriod target = (PricePeriod)(new PriceTuple(PriceTupleResolution.Days, p1, p2, p3));

            Assert.IsTrue(target.TimeSpan == new TimeSpan(3, 0, 0, 0));
            Assert.IsTrue(target.Open == 10);
            Assert.IsTrue(target.High == 14);
            Assert.IsTrue(target.Low == 9);
            Assert.IsTrue(target.Close == 11);
            Assert.IsTrue(target.Volume == 190);
        }

        [TestMethod]
        public void PriceTupleAddSubsequentPricePeriodTest()
        {
            PricePeriod p1, p2, p3, p4;
            p1 = new PricePeriod(DateTime.Parse("1/1/2010"), DateTime.Parse("1/2/2010"), 10, 12, 10, 11, 50);
            p2 = new PricePeriod(DateTime.Parse("1/2/2010"), DateTime.Parse("1/3/2010"), 11, 13, 10, 13, 60);
            p3 = new PricePeriod(DateTime.Parse("1/3/2010"), DateTime.Parse("1/4/2010"), 13, 14, 9, 11, 80);
            p4 = new PricePeriod(DateTime.Parse("1/4/2010"), DateTime.Parse("1/5/2010"), 12, 15, 11, 14, 55);

            PriceTuple target = new PriceTuple(PriceTupleResolution.Days, p1, p2, p3);

            Assert.IsTrue(target.TimeSpan == new TimeSpan(3, 0, 0, 0));
            Assert.IsTrue(target.Open == 10);
            Assert.IsTrue(target.High == 14);
            Assert.IsTrue(target.Low == 9);
            Assert.IsTrue(target.Close == 11);
            Assert.IsTrue(target.Volume == 190);

            target.AddPeriod(p4);

            Assert.IsTrue(target.TimeSpan == new TimeSpan(4, 0, 0, 0));
            Assert.IsTrue(target.Open == 10);
            Assert.IsTrue(target.High == 15);
            Assert.IsTrue(target.Low == 9);
            Assert.IsTrue(target.Close == 14);
            Assert.IsTrue(target.Volume == 245);
        }

        [TestMethod]
        public void PriceTupleAddPriorPricePeriodTest()
        {
        }

        [TestMethod]
        public void BinarySerializePriceTupleTest()
        {
            IPricePeriod p1, p2, p3;
            p1 = new PricePeriod(DateTime.Parse("1/1/2010"), DateTime.Parse("1/2/2010"), 10, 12, 10, 11, 50);
            p2 = new PricePeriod(DateTime.Parse("1/2/2010"), DateTime.Parse("1/3/2010"), 11, 13, 10, 13, 60);
            p3 = new PricePeriod(DateTime.Parse("1/3/2010"), DateTime.Parse("1/4/2010"), 13, 14, 9, 11, 80);

            IPriceTuple period = new PriceTuple(PriceTupleResolution.Days, p1, p2, p3);

            MemoryStream stream = new MemoryStream();
            PriceTuple.BinarySerialize(period, stream);
            stream.Position = 0;
            IPriceTuple result = PriceTuple.BinaryDeserialize(stream);
            Assert.AreEqual(result, period);
        }

        [TestMethod]
        public void PriceTupleSortTest()
        {
            IPricePeriod p1, p2, p3;
            p1 = new PricePeriod(DateTime.Parse("1/1/2010"), DateTime.Parse("1/2/2010"), 10, 12, 10, 11, 50);
            p2 = new PricePeriod(DateTime.Parse("1/2/2010"), DateTime.Parse("1/3/2010"), 11, 13, 10, 13, 60);
            p3 = new PricePeriod(DateTime.Parse("1/3/2010"), DateTime.Parse("1/4/2010"), 13, 14, 9, 11, 80);

            IPriceTuple period = new PriceTuple(PriceTupleResolution.Days, p3, p1, p2);
            Assert.IsTrue(period[0] == p1);
            Assert.IsTrue(period[1] == p2);
            Assert.IsTrue(period[2] == p3);
        }
    }
}
