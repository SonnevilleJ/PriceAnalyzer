using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// The exception that is thrown when a PricePeriod is in an invalid state.
    /// </summary>
    [Serializable]
    public class InvalidPricePeriodException : InvalidOperationException, ISerializable
    {
        internal InvalidPricePeriodException()
            : base()
        {
        }

        internal InvalidPricePeriodException(string message)
            : base(message)
        {
        }

        internal InvalidPricePeriodException(string message, Exception inner)
            : base(message, inner)
        {
        }

        internal InvalidPricePeriodException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
