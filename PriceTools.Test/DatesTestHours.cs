using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class DatesTestHours : DatesTest
    {
        protected override Resolution Resolution
        {
            get { return Resolution.Hours; }
        }
    }
}