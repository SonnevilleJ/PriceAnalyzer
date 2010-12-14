using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// The exception that is thrown when a Position is in an invalid state.
    /// </summary>
    [Serializable]
    public class InvalidPositionException : InvalidOperationException, ISerializable
    {
        /// <summary>
        /// Constructs an InvalidPositionException.
        /// </summary>
        public InvalidPositionException()
            :base()
        {
        }

        /// <summary>
        /// Constructs an InvalidPositionException with a message.
        /// </summary>
        /// <param name="message">The message contained within this InvalidPositionException.</param>
        public InvalidPositionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructs an InvalidPositionException with a message and an inner exception.
        /// </summary>
        /// <param name="message">The message contained within this InvalidPositionException.</param>
        /// <param name="inner">The inner exception contained within this InvalidPositionException.</param>
        public InvalidPositionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Deserializes an InvalidPositionException.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidPositionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
