namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to buy shares.
    /// </summary>
    public interface IBuy : IShareTransaction, ILongTransaction, IAccumulationTransaction, IOpeningTransaction
    {
    }
}