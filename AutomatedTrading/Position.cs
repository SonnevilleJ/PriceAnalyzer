namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    ///   Represents a Position taken using one or more <see cref = "ShareTransaction" />s.
    /// </summary>
    public interface Position : SecurityBasket
    {
        /// <summary>
        ///   Gets the ticker symbol held by this Position.
        /// </summary>
        string Ticker { get; }
    }
}
