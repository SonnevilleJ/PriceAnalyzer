namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to reinvest dividends.
    /// </summary>
    public interface DividendReinvestment : LongTransaction, AccumulationTransaction, OpeningTransaction
    {
    }
}