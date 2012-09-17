using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class DatesTestWeeks : DatesTest
    {
        protected override Resolution Resolution
        {
            get { return Resolution.Weeks; }
        }
    }
}