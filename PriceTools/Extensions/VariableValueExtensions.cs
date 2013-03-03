using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for <see cref="IVariableValue"/> objects.
    /// </summary>
    public static class VariableValueExtensions
    {
        /// <summary>
        ///   Gets a <see cref = "System.TimeSpan" /> value indicating the length of time covered by the <see cref="IVariableValue"/>.
        /// </summary>
        public static TimeSpan TimeSpan(this IVariableValue variableValue)
        {
            return variableValue.Tail - variableValue.Head;
        }

        /// <summary>
        /// Gets the value stored in the <see cref="IVariableValue"/>.
        /// </summary>
        /// <param name="variableValue"></param>
        /// <returns></returns>
        public static decimal Value(this IVariableValue variableValue)
        {
            return variableValue[variableValue.Tail];
        }
    }
}
