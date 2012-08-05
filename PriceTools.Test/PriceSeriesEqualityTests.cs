using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.SamplePriceData;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PriceSeriesEqualityTests
    {
        [TestMethod]
        public void PriceSeriesEqualsWithDifferentData()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;

            Assert.IsFalse(series1.Equals(series2));
        }

        [TestMethod]
        public void PriceSeriesEqualsWithSameData()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            Assert.IsTrue(series1.Equals(series2));
        }

        [TestMethod]
        public void PriceSeriesGetHashCodeWithDifferentData()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;

            Assert.AreNotEqual(series1.GetHashCode(), series2.GetHashCode());
        }

        [TestMethod]
        public void PriceSeriesGetHashCodeWithSameData()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            Assert.AreEqual(series1.GetHashCode(), series2.GetHashCode());
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithDifferentData()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series3 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series4 = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithSameData()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series3 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series4 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4 };

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithExtraseries()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series3 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series4 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series5 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4, series5 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithMissingseries()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series3 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            
            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentOrderCheck()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var series3 = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var series4 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4 };

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithDifferentData()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series3 = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var series4 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4 };

            Assert.IsFalse(list1.Equals(list2));
        }

        [TestMethod]
        public void EnumerableEqualsWithSameData()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series3 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series4 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4 };

            CollectionAssert.AreEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithExtraseries()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series3 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series4 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series5 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4, series5 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithMissingseries()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series3 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsOrderCheck()
        {
            var series1 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var series2 = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var series3 = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var series4 = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4 };

            CollectionAssert.AreNotEqual(list1, list2);
        }
    }
}
