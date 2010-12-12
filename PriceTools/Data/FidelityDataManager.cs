using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Manages data files from Fidelity Investments.
    /// </summary>
    public static class FidelityDataManager
    {
        private static FidelityPriceSeriesCsvParser _priceParser;
        private static FidelityPortfolioCsvParser _portfolioParser;
        private static bool _isInitialized;

        /// <summary>
        /// Gets the <see cref="IPriceSeriesCsvParser"/> for Fidelity data files.
        /// </summary>
        public static FidelityPriceSeriesCsvParser PriceParser
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

        /// <summary>
        /// Gets the <see cref="IPortfolioCsvParser"/> for Fidelity data files.
        /// </summary>
        public static FidelityPortfolioCsvParser PortfolioParser
        {
            get
            {
                if(!_isInitialized)
                {
                    Initialize();
                }
                return _portfolioParser;
            }
        }

        private static void Initialize()
        {
            _priceParser = new FidelityPriceSeriesCsvParser();
            _portfolioParser = new FidelityPortfolioCsvParser();
            _isInitialized = true;
        }
    }
}
