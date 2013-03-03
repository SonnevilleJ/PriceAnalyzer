namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to sell shares.
    /// </summary>
    public interface ISell : IShareTransaction, ILongTransaction, IDistributionTransaction, IClosingTransaction
    {
    }
}