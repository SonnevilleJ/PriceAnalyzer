using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Yahoo;
using Test.Sonneville.PriceTools.Data;

namespace Test.Sonneville.PriceTools.Yahoo
{
    [TestClass]
    public class YahooPriceDataProviderTest : PriceDataProviderTest
    {
        protected override PriceDataProvider GetTestObjectInstance()
        {
            return new YahooPriceDataProvider();
        }
    }
}
