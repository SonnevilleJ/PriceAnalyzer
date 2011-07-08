using System;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Stores the settings used by the PriceTools framework.
    /// </summary>
    public static class Settings
    {
        static Settings()
        {
            SetDefaultSettings();
        }

        /// <summary>
        /// Sets all settings to defaults.
        /// </summary>
        public static void SetDefaultSettings()
        {
            CanConnectToInternet = true;
            TimespanToDownload = new TimeSpan(7, 0, 0, 0);

            PreferredPriceSeriesProvider = new YahooPriceSeriesProvider();
        }

        /// <summary>
        /// Gets or sets a value indicating if the framework is allowed to connect to the Internet.
        /// </summary>
        public static bool CanConnectToInternet { get; set; }

        /// <summary>
        /// Gets or sets the preferred <see cref="PriceSeriesProvider"/> to use if one is not specified.
        /// </summary>
        public static PriceSeriesProvider PreferredPriceSeriesProvider { get; set; }

        /// <summary>
        /// Gets a value indicating if the database connection is available.
        /// </summary>
        public static bool DatabaseIsActive { get; private set; }

        /// <summary>
        /// Gets the default timespan to download for price history.
        /// </summary>
        public static TimeSpan TimespanToDownload { get; set; }
    }
}
