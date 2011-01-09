using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.Utilities
{
    public static class TestUtilities
    {
        /// <summary>
        ///   Performs a binary serialization of an object to a <see cref = "Stream" />.
        /// </summary>
        /// <param name = "obj">The object to serialize.</param>
        /// <param name = "stream">The <see cref = "Stream" /> to use for serialization.</param>
        private static void Serialize(ISerializable obj, Stream stream)
        {
            new BinaryFormatter().Serialize(stream, obj);
            stream.Flush();
            stream.Position = 0; // reset cursor to enable immediate deserialization
        }
        
        /// <summary>
        ///   Performs a binary deserialization of an object from a <see cref = "Stream" />.
        /// </summary>
        /// <param name = "stream">The <see cref = "Stream" /> to use for deserialization.</param>
        /// <returns>The deserialized object.</returns>
        private static ISerializable Deserialize(Stream stream)
        {
            return (ISerializable) new BinaryFormatter().Deserialize(stream);
        }

        /// <summary>
        /// Verifies that an object serializes and deserializes correctly and equates to the original object.
        /// </summary>
        /// <param name="expected">The object to test.</param>
        public static void VerifySerialization(ISerializable expected)
        {
            using (FileStream stream = GetTemporaryFile())
            {
                Serialize(expected, stream);
                object actual = Deserialize(stream);
                Assert.AreEqual(expected as object, actual);
            }
        }

        private static FileStream GetTemporaryFile()
        {
            string path = Path.GetTempPath();
            string filename = Guid.NewGuid() + ".tmp";
            return File.Open(string.Concat(path, filename), FileMode.CreateNew);
        }
    }
}
