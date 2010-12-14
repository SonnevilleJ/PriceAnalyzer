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
        /// <summary>
        /// Constructs an InvalidPricePeriodException.
        /// </summary>
        public InvalidPricePeriodException()
            : base()
        {
        }

        /// <summary>
        /// Constructs an InvalidPricePeriodException with a message.
        /// </summary>
        /// <param name="message">The message contained within this InvalidPricePeriodException.</param>
        public InvalidPricePeriodException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructs an InvalidPricePeriodException with a message and an inner exception.
        /// </summary>
        /// <param name="message">The message contained within this InvalidPricePeriodException.</param>
        /// <param name="inner">The inner exception contained within this InvalidPricePeriodException.</param>
        public InvalidPricePeriodException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Deserializes an InvalidPricePeriodException.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidPricePeriodException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
