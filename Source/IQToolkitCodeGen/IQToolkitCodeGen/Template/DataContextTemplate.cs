using System.Collections.Generic;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Template {
    public class DataContextTemplate : TemplateBase {
        public string BaseClass { get; set; }
        public string ClassName { get; set; }
        public string Namespace { get; set; }
        public List<Table> Tables { get; set; }
    }
}
