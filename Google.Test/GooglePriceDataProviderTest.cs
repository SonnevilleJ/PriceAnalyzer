using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Google;
using Test.Sonneville.PriceTools.Data;

namespace Test.Sonneville.PriceTools.Google
{
    [TestClass]
    public class GooglePriceDataProviderTest : PriceDataProviderTest
    {
        protected override PriceDataProvider GetTestObjectInstance()
        {
            return new GooglePriceDataProvider();
        }
    }
}
