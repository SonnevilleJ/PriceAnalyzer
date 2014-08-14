using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// A brokerage account used for trading financial securities.
    /// </summary>
    public interface ITradingAccount
    {
        /// <summary>
        /// The brokerage hosting this account.
        /// </summary>
        IBrokerage Brokerage { get; }

        /// <summary>
        /// The portfolio of transactions recorded by this TradingAccount.
        /// </summary>
        IPortfolio Portfolio { get; }

        /// <summary>
        /// The account number identifying this account at the <see cref="IBrokerage"/>.
        /// </summary>
        string AccountNumber { get; }

        /// <summary>
        /// Gets the list of features supported by this TradingAccount.
        /// </summary>
        TradingAccountFeatures Features { get; }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        void Submit(Order order);

        /// <summary>
        /// Attempts to cancel an <see cref="Order"/> before it is filled.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to attempt to cancel.</param>
        void TryCancelOrder(Order order);
    }
}