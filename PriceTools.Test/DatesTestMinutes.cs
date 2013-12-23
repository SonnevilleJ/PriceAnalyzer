using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class DatesTestMinutes : DatesTest
    {
        protected override Resolution Resolution
        {
            get { return Resolution.Minutes; }
        }
    }
}