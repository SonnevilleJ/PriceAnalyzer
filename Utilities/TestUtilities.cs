using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.Utilities
{
    public static class TestUtilities
    {
        #region Tickers

        private static readonly IList<string> TickersUsed = new List<string>();
        private static readonly IList<string> Tickers = new List<string>
                           {
                               "A", "AA", "AAPL", "ABC", "ABT", "ACE", "ACN", "ADBE", "ADI", "ADM", "ADP", "ADSK", "AEE"
                               , "AEP", "AES", "AET", "AFL", "AGN", "AIG", "AIV", "AIZ", "AKAM", "ALL", "ALTR", "ALXN",
                               "AMAT", "AMD", "AMGN", "AMP", "AMT", "AMZN", "AN", "ANF", "ANR", "AON", "APA", "APC",
                               "APD", "APH", "APOL", "ARG", "ATI", "AVB", "AVP", "AVY", "AXP", "AZO", "BA", "BAC", "BAX"
                               , "BBBY", "BBT", "BBY", "BCR", "BDX", "BEAM", "BEN", "BF.B", "BHI", "BIG", "BIIB", "BK",
                               "BLK", "BLL", "BMC", "BMS", "BMY", "BRCM", "BRK.B", "BSX", "BTU", "BWA", "BXP", "C", "CA"
                               , "CAG", "CAH", "CAM", "CAT", "CB", "CBE", "CBG", "CBS", "CCE", "CCI", "CCL", "CELG",
                               "CERN", "CF", "CFN", "CHK", "CHRW", "CI", "CINF", "CL", "CLF", "CLX", "CMA", "CMCSA",
                               "CME", "CMG", "CMI", "CMS", "CNP", "CNX", "COF", "COG", "COH", "COL", "COP", "COST",
                               "COV", "CPB", "CRM", "CSC", "CSCO", "CSX", "CTAS", "CTL", "CTSH", "CTXS", "CVC", "CVH",
                               "CVS", "CVX", "D", "DD", /* "DE", */ "DELL", "DF", "DFS", "DGX", "DHI", "DHR", "DIS", "DISCA",
                               "DLTR", "DNB", "DNR", "DO", "DOV", "DOW", "DPS", "DRI", "DTE", "DTV", "DUK", "DV", "DVA",
                               "DVN", "EA", "EBAY", "ECL", "ED", "EFX", "EIX", "EL", "EMC", "EMN", "EMR", "EOG", "EQR",
                               "EQT", "ESRX", "ESV", "ETFC", "ETN", "ETR", "EW", "EXC", "EXPD", "EXPE", "F", "FAST",
                               "FCX", "FDO", "FDX", "FE", "FFIV", "FHN", "FII", "FIS", "FISV", "FITB", "FLIR", "FLR",
                               "FLS", "FMC", "FOSL", "FRX", "FSLR", "FTI", "FTR", "GAS", "GCI", "GD", "GE", "GILD",
                               "GIS", "GLW", "GME", "GNW", "GOOG", "GPC", "GPS", "GS", "GT", "GWW", "HAL", "HAR", "HAS",
                               "HBAN", "HCBK", "HCN", "HCP", "HD", "HES", "HIG", "HNZ", "HOG", "HON", "HOT", "HP", "HPQ"
                               , "HRB", "HRL", "HRS", "HSP", "HST", "HSY", "HUM", /* "IBM", */ "ICE", "IFF", "IGT", "INTC",
                               "INTU", "IP", "IPG", "IR", "IRM", "ISRG", "ITW", "IVZ", "JBL", "JCI", "JCP", "JDSU",
                               "JEC", "JNJ", "JNPR", "JOY", "JPM", "JWN", "K", "KEY", "KFT", "KIM", "KLAC", "KMB", "KMI"
                               , "KMX", "KO", "KR", "KSS", "L", "LEG", "LEN", "LH", "LIFE", "LLL", "LLTC", "LLY", "LM",
                               "LMT", "LNC", "LO", "LOW", "LRCX", "LSI", "LTD", "LUK", "LUV", "LXK", "M", "MA", "MAR",
                               "MAS", "MAT", "MCD", "MCHP", "MCK", "MCO", "MDT", "MET", "MHP", "MJN", "MKC", "MMC",
                               "MMM", "MNST", "MO", "MOLX", "MON", "MOS", "MPC", "MRK", "MRO", "MS", /* "MSFT", */ "MSI",
                               "MTB", "MU", "MUR", "MWV", "MYL", "NBL", "NBR", "NDAQ", "NE", "NEE", "NEM", "NFLX", "NFX"
                               , "NI", "NKE", "NOC", "NOV", "NRG", "NSC", "NTAP", "NTRS", "NU", "NUE", "NVDA", "NWL",
                               "NWSA", "NYX", "OI", "OKE", "OMC", "ORCL", "ORLY", "OXY", "PAYX", "PBCT", "PBI", "PCAR",
                               "PCG", "PCL", "PCLN", "PCP", "PCS", "PDCO", "PEG", "PEP", "PFE", "PFG", "PG", "PGR", "PH"
                               , "PHM", "PKI", "PLD", "PLL", "PM", "PNC", "PNW", "POM", "PPG", "PPL", "PRGO", "PRU",
                               "PSA", "PSX", "PWR", "PX", "PXD", "QCOM", "QEP", "R", "RAI", "RDC", "RF", "RHI", "RHT",
                               "RL", "ROK", "ROP", "ROST", "RRC", "RRD", "RSG", "RTN", "S", "SAI", "SBUX", "SCG", "SCHW"
                               , "SE", "SEE", "SHLD", "SHW", "SIAL", "SJM", "SLB", "SLM", "SNA", "SNDK", "SNI", "SO",
                               "SPG", "SPLS", "SRCL", "SRE", "STI", "STJ", "STT", "STX", "STZ", "SUN", "SWK", "SWN",
                               "SWY", "SYK", "SYMC", "SYY", "T", "TAP", "TDC", "TE", "TEG", "TEL", "TER", "TGT", "THC",
                               "TIE", "TIF", "TJX", "TMK", "TMO", "TRIP", "TROW", "TRV", "TSN", "TSO", "TSS", "TWC",
                               "TWX", "TXN", "TXT", "TYC", "UNH", "UNM", "UNP", "UPS", "URBN", "USB", "UTX", "V", "VAR",
                               "VFC", "VIAB", "VLO", "VMC", "VNO", "VRSN", "VTR", "VZ", "WAG", "WAT", "WDC", "WEC",
                               "WFC", "WFM", "WHR", "WIN", "WLP", "WM", "WMB", "WMT", "WPI", "WPO", "WPX", "WU", "WY",
                               "WYN", "WYNN", "X", "XEL", "XL", "XLNX", "XOM", "XRAY", "XRX", "XYL", "YHOO", "YUM",
                               "ZION", "ZMH",
                           };

        #endregion

        #region Price Period tools

        public static PricePeriod CreatePeriod1()
        {
            var head = new DateTime(2011, 3, 14);
            var tail = head.GetFollowingClose();
            const decimal open = 100.00m;
            const decimal high = 110.00m;
            const decimal low = 90.00m;
            const decimal close = 100.00m;
            const long volume = 20000;

            return PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        public static PricePeriod CreatePeriod2()
        {
            var head = new DateTime(2011, 3, 15);
            var tail = head.GetFollowingClose();
            const decimal open = 100.00m;
            const decimal high = 120.00m;
            const decimal low = 100.00m;
            const decimal close = 110.00m;

            return PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close);
        }

        public static PricePeriod CreatePeriod3()
        {
            var head = new DateTime(2011, 3, 16);
            var tail = head.GetFollowingClose();
            const decimal open = 110.00m;
            const decimal high = 110.00m;
            const decimal low = 80.00m;
            const decimal close = 90.00m;
            const long volume = 10000;

            return PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        #endregion

        #region Price Quote tools

        public static PriceTick CreateTick1()
        {
            return PriceTickFactory.ConstructPriceTick(DateTime.Parse("2/28/2011 9:30 AM"), 10, 50);
        }

        public static PriceTick CreateTick2()
        {
            return PriceTickFactory.ConstructPriceTick(DateTime.Parse("3/1/2011 10:00 AM"), 9, 60);
        }

        public static PriceTick CreateTick3()
        {
            return PriceTickFactory.ConstructPriceTick(DateTime.Parse("3/2/2011 2:00 PM"), 14, 50);
        }

        public static PriceTick CreateQuote4()
        {
            return PriceTickFactory.ConstructPriceTick(DateTime.Parse("3/2/2011 4:00 PM"), 11, 30);
        }

        #endregion

        #region Trading Account tools

        /// <summary>
        /// Creates a simulated <see cref="TradingAccount"/> which accepts all <see cref="OrderType"/>s, does not allow margin trading, imposes a flat commission of $5.00 per transaction, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <returns></returns>
        public static TradingAccount CreateSimulatedTradingAccount()
        {
            return CreateSimulatedTradingAccount(new MarginNotAllowed());
        }

        /// <summary>
        /// Creates a simulated <see cref="TradingAccount"/> which accepts all <see cref="OrderType"/>s, a flat commission of $5.00 per transaction, uses the given <see cref="IMarginSchedule"/>, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <returns></returns>
        public static TradingAccount CreateSimulatedTradingAccount(IMarginSchedule marginSchedule)
        {
            return CreateSimulatedTradingAccount(new FlatCommissionSchedule(5.00m), marginSchedule);
        }

        /// <summary>
        /// Creates a simulated <see cref="TradingAccount"/> which accepts all <see cref="OrderType"/>s, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <returns></returns>
        public static TradingAccount CreateSimulatedTradingAccount(ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            var orderTypes = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures().SupportedOrderTypes;
            return CreateSimulatedTradingAccount(orderTypes, commissionSchedule, marginSchedule);
        }

        /// <summary>
        /// Creates a simulated <see cref="TradingAccount"/> with an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="TradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <returns></returns>
        public static TradingAccount CreateSimulatedTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            // default deposit of $1,000,000
            var deposit = TransactionFactory.ConstructDeposit(new DateTime(1900, 1, 1), 1000000.00m);
            return CreateSimulatedTradingAccount(orderTypes, commissionSchedule, marginSchedule, deposit);
        }

        /// <summary>
        /// Creates a simulated <see cref="TradingAccount"/>.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="TradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="TradingAccount"/>.</param>
        /// <param name="openingDeposit">The opening deposit to place in the <see cref="Portfolio"/> used by the <see cref="TradingAccount"/>.</param>
        /// <returns></returns>
        public static TradingAccount CreateSimulatedTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule, Deposit openingDeposit)
        {
            var tradingAccountFeatures = TradingAccountFeaturesFactory.CreateTradingAccountFeatures(orderTypes, commissionSchedule, marginSchedule);
            var portfolio = PortfolioFactory.ConstructPortfolio();
            portfolio.Deposit(openingDeposit);
            return new SimulatedTradingAccountImpl {Features = tradingAccountFeatures, Portfolio = portfolio};
        }

        /// <summary>
        /// Creates an <see cref="ShareTransaction"/> which would result from the perfect execution of <paramref name="order"/>.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use as the SettlementDate for the resulting <see cref="ShareTransaction"/>.</param>
        /// <param name="order">The <see cref="Order"/> which should define the parameters for the resulting <see cref="ShareTransaction"/>.</param>
        /// <param name="commission">The commission that should be charged for the resulting <see cref="ShareTransaction"/>.</param>
        /// <returns></returns>
        public static ShareTransaction CreateShareTransaction(DateTime settlementDate, Order order, decimal commission)
        {
            return TransactionFactory.ConstructShareTransaction(order.OrderType, order.Ticker, settlementDate, order.Shares, order.Price, commission);
        }

        #endregion

        /// <summary>
        /// Asserts that each property is identical for the two instances of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AssertSameState<T>(T expected, T actual)
        {
            var properties = expected.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.GetIndexParameters().Length != 0) continue;

                Assert.AreEqual(propertyInfo.GetValue(expected, null), propertyInfo.GetValue(actual, null));
            }
        }

        /// <summary>
        /// Gets a ticker symbol guaranteed to be unique across all threads creating tickers with this method.
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueTicker()
        {
            lock (TickersUsed)
            {
                var ticker = Tickers[0];
                Tickers.Remove(ticker);
                TickersUsed.Add(ticker);
                return ticker;
            }
        }
    }
}
