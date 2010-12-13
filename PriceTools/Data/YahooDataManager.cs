using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Manages data files from Yahoo! Finance.
    /// </summary>
    public static class YahooDataManager
    {
        private static YahooPriceSeriesCsvParser _priceParser;
        private static bool _isInitialized;

        /// <summary>
        /// Gets the <see cref="IPriceSeriesCsvParser"/> for Fidelity data files.
        /// </summary>
        public static YahooPriceSeriesCsvParser PriceParser
        {
            get
            {
                if (!_isInitialized)
                {
                    Initialize();
                }
                return _priceParser;
            }
        }

        private static void Initialize()
        {
            _priceParser = new YahooPriceSeriesCsvParser();
            _isInitialized = true;

        }
    }
}
