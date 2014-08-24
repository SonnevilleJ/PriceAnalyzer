using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class DatesTestSeconds : DatesTest
    {
        protected override Resolution Resolution
        {
            get { return Resolution.Seconds; }
        }
    }
}