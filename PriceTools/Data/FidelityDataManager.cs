//namespace Sonneville.PriceTools.Data
//{
//    /// <summary>
//    /// Manages data files from Fidelity Investments.
//    /// </summary>
//    public static class FidelityDataManager
//    {
//        private static FidelityPortfolioCsvParser _portfolioParser;
//        private static bool _isInitialized;

//        /// <summary>
//        /// Gets the <see cref="IPortfolioCsvParser"/> for Fidelity data files.
//        /// </summary>
//        public static FidelityPortfolioCsvParser PortfolioParser
//        {
//            get
//            {
//                if(!_isInitialized)
//                {
//                    Initialize();
//                }
//                return _portfolioParser;
//            }
//        }

//        private static void Initialize()
//        {
//            _portfolioParser = new FidelityPortfolioCsvParser();
//            _isInitialized = true;
//        }
//    }
//}
