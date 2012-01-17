using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Sonneville.PriceTools.Data
{
    public static class Serializer
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
                    // cannot create an XmlSerializer of an interface type
                    var type = obj.GetType();
                    var serializer = new XmlSerializer(type);
                    serializer.Serialize(stream, obj);
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
            if (!typeof (T).IsInterface) return TryDeserialize<T>(typeof (T), xml);

            var matchingClasses = GetMatchingClasses(typeof (T));
            foreach (var matchingClass in matchingClasses)
            {
                try
                {
                    return TryDeserialize<T>(matchingClass, xml);
                }
                catch (NotSupportedException)
                {
                }
            }
            throw new TypeLoadException(string.Format(Strings.Serializer_DeserializeFromXml_Could_not_find_a_compatible_type_to_implement_interface___0__,
                                                      typeof (T).FullName));
        }

        /// <summary>
        /// Tries to deserialize an object from XML.
        /// </summary>
        /// <typeparam name="T">The type of object to return. Can be an interface.</typeparam>
        /// <param name="type">The type to attempt to construct.</param>
        /// <param name="xml">The serialized XML.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown when deserialization fails.</exception>
        private static T TryDeserialize<T>(Type type, string xml)
        {
            using (var stream = new MemoryStream())
            {
                using (var textWriter = new StreamWriter(stream))
                {
                    var deserializer = new XmlSerializer(type);
                    textWriter.Write(xml);
                    textWriter.Flush();
                    stream.Position = 0;
                    return (T) deserializer.Deserialize(stream);
                }
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
            var exportedTypes = assembly.GetExportedTypes();
            return exportedTypes.Where(exportedType => baseType.IsAssignableFrom(exportedType) && !exportedType.IsInterface);
        }
    }
}
