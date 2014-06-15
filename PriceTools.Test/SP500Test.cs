using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class SP500Test
    {
        private SP500 _index;

        [TestInitialize]
        public void Setup()
        {
            _index = new SP500();
        }

        [TestMethod]
        public void SAndP500ShouldContain500Stocks()
        {
            var tickers = _index.GetTickers();

            Assert.AreEqual(500, tickers.Count());
        }
    }
}