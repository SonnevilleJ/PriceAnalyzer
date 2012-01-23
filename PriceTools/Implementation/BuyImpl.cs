using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to buy shares.
    /// </summary>
    [Serializable]
    internal sealed class BuyImpl : ShareTransactionImpl, Buy
    {
    }
}