using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class DatesTestDays : DatesTest
    {
        protected override Resolution Resolution
        {
            get { return Resolution.Days; }
        }
    }
}