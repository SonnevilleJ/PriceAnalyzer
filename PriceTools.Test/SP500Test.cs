using System.Linq;
using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class SP500Test
    {
        private SP500 _index;

        [SetUp]
        public void Setup()
        {
            _index = new SP500();
        }

        [Test]
        public void SAndP500ShouldContain500Stocks()
        {
            var tickers = _index.GetTickers();

            Assert.AreEqual(500, tickers.Count());
        }
    }
}