using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a basket of securities whose value changes over time.
    /// </summary>
    public interface SecurityBasket : IVariableValue
    {
        /// <summary>
        ///   Gets an enumeration of all <see cref = "ShareTransactionImpl" />s in this SecurityBasket.
        /// </summary>
        IEnumerable<ITransaction> Transactions { get; }
    }
}
