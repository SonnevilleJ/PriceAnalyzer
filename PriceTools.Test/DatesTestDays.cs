using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class DatesTestDays : DatesTest
    {
        protected override Resolution Resolution
        {
            get { return Resolution.Days; }
        }
    }
}