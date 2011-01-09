using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sonneville.Utilities
{
    public static class TestUtilities
    {
        /// <summary>
        ///   Performs a binary serialization of an object to a <see cref = "Stream" />.
        /// </summary>
        /// <param name = "obj">The object to serialize.</param>
        /// <param name = "stream">The <see cref = "Stream" /> to use for serialization.</param>
        public static void BinarySerialize(ISerializable obj, Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Flush();
            stream.Position = 0; // reset cursor so tests can perform immediate deserialization
        }

        /// <summary>
        ///   Performs a binary deserialization of an object from a <see cref = "Stream" />.
        /// </summary>
        /// <param name = "stream">The <see cref = "Stream" /> to use for deserialization.</param>
        /// <returns>The deserialized object.</returns>
        public static ISerializable BinaryDeserialize(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (ISerializable)formatter.Deserialize(stream);
        }

    }
}
