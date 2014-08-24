using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class DatesTestHours : DatesTest
    {
        protected override Resolution Resolution
        {
            get { return Resolution.Hours; }
        }
    }
}