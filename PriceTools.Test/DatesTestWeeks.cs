using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
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