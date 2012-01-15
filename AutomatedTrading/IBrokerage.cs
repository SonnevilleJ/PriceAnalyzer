namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IBrokerage
    {
        /// <summary>
        /// Collects credentials from the user and logs the user into the brokerage.
        /// </summary>
        void LogIn();

        /// <summary>
        /// Logs the user into the brokerage.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        void LogIn(string username, string password);

        /// <summary>
        /// Gets the <see cref="TradingAccount"/> associated with the user's brokerage account.
        /// </summary>
        /// <returns>The <see cref="TradingAccount"/> associated with the user's brokerage account.</returns>
        TradingAccount GetTradingAccount();
    }
}
