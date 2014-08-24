using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public interface ISecurityBasket : IVariableValue<decimal>
    {
        IList<ITransaction> Transactions { get; }
    }
}
