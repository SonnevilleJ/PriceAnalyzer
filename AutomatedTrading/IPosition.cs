namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    ///   Represents a Position taken using one or more <see cref = "IShareTransaction" />s.
    /// </summary>
    public interface IPosition : ISecurityBasket
    {
        /// <summary>
        ///   Gets the ticker symbol held by this Position.
        /// </summary>
        string Ticker { get; }
    }
}
