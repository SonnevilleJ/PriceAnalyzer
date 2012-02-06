namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to sell shares.
    /// </summary>
    public interface Sell : LongTransaction, DistributionTransaction, ClosingTransaction
    {
    }
}