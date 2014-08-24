using System;

namespace Sonneville.PriceTools
{
    public static class VariableValueExtensions
    {
        public static TimeSpan TimeSpan<T>(this IVariableValue<T> variableValue)
        {
            return variableValue.Tail - variableValue.Head;
        }

        public static T Value<T>(this IVariableValue<T> variableValue)
        {
            return variableValue[variableValue.Tail];
        }
    }
}
