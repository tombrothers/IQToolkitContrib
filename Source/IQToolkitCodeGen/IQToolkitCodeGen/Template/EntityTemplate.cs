using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Template {
    public class EntityTemplate : TemplateBase {
        public string Namespace { get; set; }
        public Table Table { get; set; }
    }
}
