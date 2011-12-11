namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to reinvest dividends.
    /// </summary>
    public sealed class DividendReinvestment : ShareTransaction
    {
        /// <summary>
        /// Constructs a DividendReinvestment Transaction.
        /// </summary>
        public DividendReinvestment()
        {
            OrderType = OrderType.DividendReinvestment;
        }
    }
}