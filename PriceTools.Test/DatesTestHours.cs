using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
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