using System;
using System.Data;
using System.Data.Objects.DataClasses;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Sonneville.Utilities
{
    public static class TestUtilities
    {
        /// <summary>
        ///   Performs a binary serialization of an object to a <see cref = "Stream" />.
        /// </summary>
        /// <param name = "obj">The object to serialize.</param>
        /// <param name = "stream">The <see cref = "Stream" /> to use for serialization.</param>
        private static void Serialize(object obj, Stream stream)
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
        private static object Deserialize(Stream stream)
        {
            return new BinaryFormatter().Deserialize(stream);
        }

        /// <summary>
        /// Verifies that an object serializes and deserializes correctly and equates to the original object.
        /// </summary>
        /// <param name="expected">The object to test.</param>
        public static void VerifySerialization(object expected)
        {
            using (FileStream stream = GetTemporaryFile())
            {
                Serialize(expected, stream);
                object actual = Deserialize(stream);
                Assert.AreEqual(expected, actual);
            }
        }

        private static FileStream GetTemporaryFile()
        {
            string path = Path.GetTempPath();
            string filename = Guid.NewGuid() + ".tmp";
            return File.Open(string.Concat(path, filename), FileMode.CreateNew);
        }

        private static void VerifyEntitySerialize(EntityObject target, string context)
        {
            using (var ctx = new Container())
            {
                Assert.AreEqual(EntityState.Detached, target.EntityState);

                ctx.AddObject(context, target);
                Assert.AreEqual(EntityState.Added, target.EntityState);

                ctx.SaveChanges();
                Assert.AreEqual(EntityState.Unchanged, target.EntityState);

                ctx.DeleteObject(target);
                Assert.AreEqual(EntityState.Deleted, target.EntityState);

                ctx.SaveChanges();
                Assert.AreEqual(EntityState.Detached, target.EntityState);
            }
        }

        public static void VerifyTransactionEntity(ITransaction transaction)
        {
            VerifyEntitySerialize((Transaction)transaction, "Transactions");
        }
    }
}
