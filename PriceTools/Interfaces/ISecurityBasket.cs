using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a basket of securities whose value changes over time.
    /// </summary>
    public interface ISecurityBasket : IVariableValue
    {
        /// <summary>
        ///   Gets an enumeration of all <see cref = "ShareTransaction" />s in this SecurityBasket.
        /// </summary>
        IEnumerable<ITransaction> Transactions { get; }
    }
}
