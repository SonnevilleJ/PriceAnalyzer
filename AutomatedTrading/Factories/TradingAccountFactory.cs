﻿using System;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// Constructs <see cref="ITradingAccount"/> objects.
    /// </summary>
    public class TradingAccountFactory : ITradingAccountFactory
    {
        private readonly MarginNotAllowed _defaultMarginSchedule = new MarginNotAllowed();
        private readonly FlatCommissionSchedule _defaultCommissionSchedule = new FlatCommissionSchedule(5.00m);
        private readonly OrderType _defaultOrderTypes = TradingAccountFeaturesFactory.ConstructFullTradingAccountFeatures().SupportedOrderTypes;
        private readonly IDeposit _defaultDeposit = TransactionFactory.ConstructDeposit(new DateTime(1900, 1, 1), 1000000.00m);
        private readonly IPortfolioFactory _portfolioFactory;

        public TradingAccountFactory()
        {
            _portfolioFactory = new PortfolioFactory();
        }

        #region BacktestingTradingAccount

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
        /// <param name="openingDeposit">The opening deposit to place in the <see cref="IPortfolio"/> used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public ITradingAccount ConstructBacktestingTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule, IDeposit openingDeposit)
        {
            var tradingAccountFeatures = TradingAccountFeaturesFactory.ConstructTradingAccountFeatures(orderTypes, commissionSchedule, marginSchedule);
            var portfolio = _portfolioFactory.ConstructPortfolio(openingDeposit);
            return new BacktestingTradingAccountImpl { Features = tradingAccountFeatures, Portfolio = portfolio };
        }

        #endregion

        #region SimulatedTradingAccount

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
        /// <param name="openingDeposit">The opening deposit to place in the <see cref="IPortfolio"/> used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        public ITradingAccount ConstructSimulatedTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule, IDeposit openingDeposit)
        {
            var tradingAccountFeatures = TradingAccountFeaturesFactory.ConstructTradingAccountFeatures(orderTypes, commissionSchedule, marginSchedule);
            var portfolio = _portfolioFactory.ConstructPortfolio(openingDeposit);
            return new SimulatedTradingAccountImpl {Features = tradingAccountFeatures, Portfolio = portfolio};
        }

        #endregion
    }
}
