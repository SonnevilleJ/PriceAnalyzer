using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class DatesTestSeconds : DatesTest
    {
        protected override Resolution Resolution
        {
            get { return Resolution.Seconds; }
        }
    }
}