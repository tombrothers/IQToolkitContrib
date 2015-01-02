using System;

namespace IQToolkitCodeGen.Service {
    public interface IXmlSerializerService {
        T DeserializeFile<T>(string file);
        string ToXml(object instance);
    }
}
