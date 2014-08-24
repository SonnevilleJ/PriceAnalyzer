using System;

namespace Sonneville.PriceTools
{
    public interface IVariableValue<out T>
    {
        T this[DateTime dateTime] { get; }

        DateTime Head { get; }

        DateTime Tail { get; }
    }
}