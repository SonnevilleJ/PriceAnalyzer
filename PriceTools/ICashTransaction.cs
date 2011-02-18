namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for an <see cref="ICashAccount"/>.
    /// </summary>
    public interface ICashTransaction : ITransaction
    {
        /// <summary>
        ///   Gets the amount of cash in this ICashTransaction.
        /// </summary>
        decimal Amount { get; }
    }
}
