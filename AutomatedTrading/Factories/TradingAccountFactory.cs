using System;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// Constructs <see cref="ITradingAccount"/> objects.
    /// </summary>
    public class TradingAccountFactory : ITradingAccountFactory
    {
        private readonly IMarginSchedule _defaultMarginSchedule;
        private readonly ICommissionSchedule _defaultCommissionSchedule;
        private readonly OrderType _defaultOrderTypes;
        private readonly Deposit _defaultDeposit;
        private readonly IPortfolioFactory _portfolioFactory;
        private readonly ITradingAccountFeaturesFactory _tradingAccountFeaturesFactory;

        public TradingAccountFactory()
            : this(new TransactionFactory(), new PortfolioFactory(), new TradingAccountFeaturesFactory(), new MarginNotAllowed(), new FlatCommissionSchedule(5.00m))
        {
        }

        public TradingAccountFactory(ITransactionFactory transactionFactory, IPortfolioFactory portfolioFactory, ITradingAccountFeaturesFactory tradingAccountFeaturesFactory, IMarginSchedule defaultMarginSchedule, ICommissionSchedule defaultCommissionSchedule)
        {
            _defaultCommissionSchedule = defaultCommissionSchedule;
            _defaultMarginSchedule = defaultMarginSchedule;
            _portfolioFactory = portfolioFactory;
            _tradingAccountFeaturesFactory = tradingAccountFeaturesFactory;
            _defaultOrderTypes = _tradingAccountFeaturesFactory.ConstructFullTradingAccountFeatures().SupportedOrderTypes;
            _defaultDeposit = transactionFactory.ConstructDeposit(new DateTime(1900, 1, 1), 1000000.00m);
        }

        /// <summary>
        /// Creates a backtesting <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, does not allow margin trading, imposes a flat commission of $5.00 per transaction, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <returns></returns>
        public ITradingAccount ConstructBacktestingTradingAccount()
        {
            return ConstructBacktestingTradingAccount(_defaultCommissionSchedule);
        }

        /// <summary>
        /// Creates a backtesting <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, does not allow margin trading, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public ITradingAccount ConstructBacktestingTradingAccount(ICommissionSchedule commissionSchedule)
        {
            return ConstructBacktestingTradingAccount(commissionSchedule, _defaultMarginSchedule);
        }

        /// <summary>
        /// Creates a backtesting <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, a flat commission of $5.00 per transaction, uses the given <see cref="IMarginSchedule"/>, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public ITradingAccount ConstructBacktestingTradingAccount(IMarginSchedule marginSchedule)
        {
            return ConstructBacktestingTradingAccount(_defaultCommissionSchedule, marginSchedule);
        }

        /// <summary>
        /// Creates a backtesting <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public ITradingAccount ConstructBacktestingTradingAccount(ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            return ConstructBacktestingTradingAccount(_defaultOrderTypes, commissionSchedule, marginSchedule);
        }

        /// <summary>
        /// Creates a backtesting <see cref="ITradingAccount"/> with an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="ITradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public ITradingAccount ConstructBacktestingTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            return ConstructBacktestingTradingAccount(orderTypes, commissionSchedule, marginSchedule, _defaultDeposit);
        }

        /// <summary>
        /// Creates a backtesting <see cref="ITradingAccount"/>.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="ITradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="openingDeposit">The opening deposit to place in the <see cref="Portfolio"/> used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public ITradingAccount ConstructBacktestingTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule, Deposit openingDeposit)
        {
            var tradingAccountFeatures = _tradingAccountFeaturesFactory.ConstructTradingAccountFeatures(orderTypes, commissionSchedule, marginSchedule);
            var portfolio = _portfolioFactory.ConstructPortfolio(openingDeposit);
            return new BacktestingTradingAccountImpl(Guid.Parse("DBE826D1-C5C3-476C-A665-80D920E2321E"), "1234") { Features = tradingAccountFeatures, Portfolio = portfolio };
        }

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, does not allow margin trading, imposes a flat commission of $5.00 per transaction, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <returns></returns>
        public ITradingAccount ConstructSimulatedTradingAccount()
        {
            return ConstructSimulatedTradingAccount(_defaultCommissionSchedule);
        }

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, does not allow margin trading, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public ITradingAccount ConstructSimulatedTradingAccount(ICommissionSchedule commissionSchedule)
        {
            return ConstructSimulatedTradingAccount(commissionSchedule, _defaultMarginSchedule);
        }

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, a flat commission of $5.00 per transaction, uses the given <see cref="IMarginSchedule"/>, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public ITradingAccount ConstructSimulatedTradingAccount(IMarginSchedule marginSchedule)
        {
            return ConstructSimulatedTradingAccount(_defaultCommissionSchedule, marginSchedule);
        }

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public ITradingAccount ConstructSimulatedTradingAccount(ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            return ConstructSimulatedTradingAccount(_defaultOrderTypes, commissionSchedule, marginSchedule);
        }

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> with an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="ITradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public ITradingAccount ConstructSimulatedTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule)
        {
            return ConstructSimulatedTradingAccount(orderTypes, commissionSchedule, marginSchedule, _defaultDeposit);
        }

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/>.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="ITradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="openingDeposit">The opening deposit to place in the <see cref="Portfolio"/> used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public ITradingAccount ConstructSimulatedTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule, Deposit openingDeposit)
        {
            var tradingAccountFeatures = _tradingAccountFeaturesFactory.ConstructTradingAccountFeatures(orderTypes, commissionSchedule, marginSchedule);
            var portfolio = _portfolioFactory.ConstructPortfolio(openingDeposit);
            return new SimulatedTradingAccountImpl(Guid.Parse("DBE826D1-C5C3-476C-A665-80D920E2321E"), "5ACE3C35-B81C-4528-9E1E-76036CF92EE4") {Features = tradingAccountFeatures, Portfolio = portfolio};
        }
    }
}
