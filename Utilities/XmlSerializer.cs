using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Sonneville.Utilities
{
    public static class XmlSerializer
    {
        /// <summary>
        /// Serializes an object to XML.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns></returns>
        public static string SerializeToXml<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    // cannot create an DataContractSerializer of an interface type
                    var type = obj.GetType();
                    var serializer = new DataContractSerializer(type);
                    serializer.WriteObject(stream, obj);
                    stream.Flush();
                    stream.Position = 0;
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Deserializes an object from XML.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="xml">The serialized XML.</param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string xml)
        {
            var result = default(T);

            // try deserializing to exact type, as long as it isn't an interface
            if (!typeof (T).IsInterface && TryDeserialize(typeof (T), xml, out result)) return result;

            // try deserializing to compatible types
            if (GetMatchingClasses(typeof (T)).Any(matchingClass => TryDeserialize(matchingClass, xml, out result))) return result;

            throw new TypeLoadException(string.Format("Could not find a compatible type to implement interface: {0}.", typeof (T).FullName));
        }

        /// <summary>
        /// Tries to deserialize an object from XML.
        /// </summary>
        /// <typeparam name="T">The type of object to return. Can be an interface.</typeparam>
        /// <param name="type">The type to attempt to construct.</param>
        /// <param name="xml">The serialized XML.</param>
        /// <param name="result">The deserialized object.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown when deserialization fails.</exception>
        private static bool TryDeserialize<T>(Type type, string xml, out T result)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (var textWriter = new StreamWriter(stream))
                    {
                        var deserializer = new DataContractSerializer(type);
                        textWriter.Write(xml);
                        textWriter.Flush();
                        stream.Position = 0;
                        result = (T) deserializer.ReadObject(stream);
                        return true;
                    }
                }
            }
            catch
            {
                result = default(T);
                return false;
            }
        }

        /// <summary>
        /// Gets all types which implement <paramref name="baseType"/>.
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        /// <remarks>Currently this only checks for implementing types in the same assembly as the base type.</remarks>
        private static IEnumerable<Type> GetMatchingClasses(Type baseType)
        {
            var assembly = baseType.Assembly;
            var exportedTypes = assembly.GetTypes();
            return exportedTypes.Where(exportedType => baseType.IsAssignableFrom(exportedType) && !exportedType.IsInterface);
        }
    }
}
