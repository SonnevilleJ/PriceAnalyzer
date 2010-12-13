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
        public InvalidPricePeriodException()
            : base()
        {
        }

        public InvalidPricePeriodException(string message)
            : base(message)
        {
        }

        public InvalidPricePeriodException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected InvalidPricePeriodException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
