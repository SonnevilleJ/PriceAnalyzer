using System;
using System.Data;
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

            PreferredPriceSeriesProvider = new YahooPriceSeriesProvider();
        }

        public static bool CanConnectToInternet { get; set; }

        public static PriceSeriesProvider PreferredPriceSeriesProvider { get; set; }

        public static bool DatabaseIsActive { get; private set; }
    }
}
