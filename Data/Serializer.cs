using System;
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
                    var type = typeof (T).IsInterface ? GetMatchingClass(typeof (T)) : typeof (T);
                    var deserializer = new XmlSerializer(type);
                    textWriter.Write(xml);
                    textWriter.Flush();
                    stream.Position = 0;
                    return (T) deserializer.Deserialize(stream);
                }
            }
        }

        private static Type GetMatchingClass(Type type)
        {
            return type.Assembly.GetExportedTypes().Where(type.IsAssignableFrom).FirstOrDefault(exportedType => !exportedType.IsInterface);
        }
    }
}
