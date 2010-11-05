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
        internal InvalidPositionException()
            :base()
        {
        }

        internal InvalidPositionException(string message)
            : base(message)
        {
        }

        internal InvalidPositionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        internal InvalidPositionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
