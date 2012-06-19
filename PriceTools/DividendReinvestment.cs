using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to reinvest dividends.
    /// </summary>
    [Serializable]
    public sealed class DividendReinvestment : ShareTransaction, LongTransaction, AccumulationTransaction, OpeningTransaction
    {
    }
}