namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to short-sell shares.
    /// </summary>
    public interface ISellShort : IShareTransaction, IShortTransaction, IDistributionTransaction, IOpeningTransaction
    {
    }
}