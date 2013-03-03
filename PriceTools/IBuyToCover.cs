namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to buy shares to cover a previous short sell.
    /// </summary>
    public interface IBuyToCover : IShareTransaction, IShortTransaction, IAccumulationTransaction, IClosingTransaction
    {
    }
}