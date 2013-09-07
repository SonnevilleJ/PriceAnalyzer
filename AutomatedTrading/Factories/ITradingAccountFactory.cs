using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface ITradingAccountFactory
    {
        /// <summary>
        /// Creates a backtesting <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, does not allow margin trading, imposes a flat commission of $5.00 per transaction, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <returns></returns>
        ITradingAccount ConstructBacktestingTradingAccount();

        /// <summary>
        /// Creates a backtesting <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, does not allow margin trading, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        ITradingAccount ConstructBacktestingTradingAccount(ICommissionSchedule commissionSchedule);

        /// <summary>
        /// Creates a backtesting <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, a flat commission of $5.00 per transaction, uses the given <see cref="IMarginSchedule"/>, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        ITradingAccount ConstructBacktestingTradingAccount(IMarginSchedule marginSchedule);

        /// <summary>
        /// Creates a backtesting <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        ITradingAccount ConstructBacktestingTradingAccount(ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule);

        /// <summary>
        /// Creates a backtesting <see cref="ITradingAccount"/> with an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="ITradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        ITradingAccount ConstructBacktestingTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule);

        /// <summary>
        /// Creates a backtesting <see cref="ITradingAccount"/>.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="ITradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="openingDeposit">The opening deposit to place in the <see cref="IPortfolio"/> used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        ITradingAccount ConstructBacktestingTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule, Deposit openingDeposit);

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, does not allow margin trading, imposes a flat commission of $5.00 per transaction, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <returns></returns>
        ITradingAccount ConstructSimulatedTradingAccount();

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, does not allow margin trading, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        ITradingAccount ConstructSimulatedTradingAccount(ICommissionSchedule commissionSchedule);

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, a flat commission of $5.00 per transaction, uses the given <see cref="IMarginSchedule"/>, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        ITradingAccount ConstructSimulatedTradingAccount(IMarginSchedule marginSchedule);

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> which accepts all <see cref="OrderType"/>s, and has an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        ITradingAccount ConstructSimulatedTradingAccount(ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule);

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/> with an opening deposit of $1,000,000.00.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="ITradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        ITradingAccount ConstructSimulatedTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule);

        /// <summary>
        /// Creates a simulated <see cref="ITradingAccount"/>.
        /// </summary>
        /// <param name="orderTypes">A binary and-ed set of <see cref="OrderType"/>s which should be accepted by the <see cref="ITradingAccount"/>.</param>
        /// <param name="commissionSchedule">The <see cref="ICommissionSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="marginSchedule">The <see cref="IMarginSchedule"/> which should be used by the <see cref="ITradingAccount"/>.</param>
        /// <param name="openingDeposit">The opening deposit to place in the <see cref="IPortfolio"/> used by the <see cref="ITradingAccount"/>.</param>
        /// <returns></returns>
        ITradingAccount ConstructSimulatedTradingAccount(OrderType orderTypes, ICommissionSchedule commissionSchedule, IMarginSchedule marginSchedule, Deposit openingDeposit);
    }
}