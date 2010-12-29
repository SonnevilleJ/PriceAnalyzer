namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Manages data files from Yahoo! Finance.
    /// </summary>
    public static class YahooDataManager
    {
        private static YahooPriceSeriesProvider _priceParser;
        private static bool _isInitialized;

        /// <summary>
        /// Gets the <see cref="IPriceSeriesProvider"/> for Yahoo data files.
        /// </summary>
        public static YahooPriceSeriesProvider PriceParser
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
            _priceParser = new YahooPriceSeriesProvider();
            _isInitialized = true;

        }
    }
}
