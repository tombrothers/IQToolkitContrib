using System.Collections.Generic;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Template {
    public class MappingTemplate : TemplateBase {
        public List<Table> Tables { get; set; }
        public string Namespace { get; set; }
        public string ClassName { get; set; }
    }
}
