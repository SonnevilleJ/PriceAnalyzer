using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class DatesTestWeeks : DatesTest
    {
        protected override Resolution Resolution
        {
            get { return Resolution.Weeks; }
        }
    }
}