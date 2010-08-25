using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    [Serializable]
    public class InvalidPricePeriodException : Exception, ISerializable
    {
        public InvalidPricePeriodException()
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

        public InvalidPricePeriodException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
