using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Sonneville.PriceTools.Data
{
    public static class Serializer
    {

        public static string SerializeToXml(object obj)
        {
            using (var stream = new MemoryStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    var type = obj.GetType();
                    var serializer = new XmlSerializer(type);
                    serializer.Serialize(stream, obj);
                    stream.Position = 0;
                    return reader.ReadToEnd();
                }
            }
        }

        public static T DeserializeFromXml<T>(string xml)
        {
            using (var stream = new MemoryStream())
            {
                using (var textWriter = new StreamWriter(stream))
                {
                    if (!typeof (T).IsInterface) return TryDeserialize<T>(stream, typeof (T), textWriter, xml);
                    
                    var matchingClasses = GetMatchingClasses(typeof (T));
                    foreach (var matchingClass in matchingClasses)
                    {
                        try
                        {
                            return TryDeserialize<T>(stream, matchingClass, textWriter, xml);
                        }
                        catch (NotSupportedException)
                        {
                        }
                    }
                    throw new TypeLoadException(string.Format(Strings.Serializer_DeserializeFromXml_Could_not_find_a_compatible_type_to_implement_interface___0__, typeof(T).FullName));
                }
            }
        }

        private static T TryDeserialize<T>(Stream stream, Type type, TextWriter textWriter, string xml)
        {
            var deserializer = new XmlSerializer(type);
            textWriter.Write(xml);
            textWriter.Flush();
            stream.Position = 0;
            return (T) deserializer.Deserialize(stream);
        }

        private static IEnumerable<Type> GetMatchingClasses(Type type)
        {
            var assembly = type.Assembly;
            var exportedTypes = assembly.GetExportedTypes();
            return exportedTypes.Where(type.IsAssignableFrom).Where(exportedType => !exportedType.IsInterface);
        }
    }
}
