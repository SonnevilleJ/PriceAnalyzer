using System;

namespace Sonneville.PriceTools
{
    public interface ITransaction : IEquatable<ITransaction>
    {
        /// <summary>
        ///    Gets the DateTime that the Transaction occurred.
        ///  </summary>
        DateTime SettlementDate { get; }
    }
}