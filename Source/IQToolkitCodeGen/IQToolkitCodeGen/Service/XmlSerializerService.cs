using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace IQToolkitCodeGen.Service {
    public class XmlSerializerService : IXmlSerializerService {
        public T DeserializeFile<T>(string file) {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (FileStream fs = new FileStream(file, FileMode.Open)) {
                using (XmlReader reader = new XmlTextReader(fs)) {
                    return (T)serializer.Deserialize(reader);
                }
            }
        }

        public string ToXml(object instance) {
            XmlSerializer s = new XmlSerializer(instance.GetType());

            using (StringWriter sw = new StringWriter()) {
                s.Serialize(sw, instance);
                return sw.ToString();
            }
        }
    }
}
