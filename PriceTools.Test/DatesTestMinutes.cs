using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class DatesTestMinutes : DatesTest
    {
        protected override Resolution Resolution
        {
            get { return Resolution.Minutes; }
        }
    }
}