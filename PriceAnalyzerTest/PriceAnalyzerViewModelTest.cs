using Sonneville.PriceAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace PriceAnalyzerTest
{
    [TestClass]
    public class PriceAnalyzerViewModelTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            IPriceSeries priceSeries = new PriceSeries();
            PriceAnalyzerViewModel vm = new PriceAnalyzerViewModel();
        }
    }
}
