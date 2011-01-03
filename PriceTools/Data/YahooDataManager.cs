namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Manages data files from Yahoo! Finance.
    /// </summary>
    public static class YahooDataManager
    {
        #region Private Members

        private static readonly YahooPriceSeriesProvider _priceParser = new YahooPriceSeriesProvider();

        #endregion

        #region Public Members

        /// <summary>
        /// Gets the <see cref="PriceSeriesProvider"/> for Yahoo data files.
        /// </summary>
        public static YahooPriceSeriesProvider PriceParser
        {
            get
            {
                return _priceParser;
            }
        }

        #endregion
    }
}
