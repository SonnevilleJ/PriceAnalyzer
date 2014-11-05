using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public interface ISecurityBasket : IVariableValue<decimal>
    {
        IEnumerable<ITransaction> Transactions { get; }
    }
}
