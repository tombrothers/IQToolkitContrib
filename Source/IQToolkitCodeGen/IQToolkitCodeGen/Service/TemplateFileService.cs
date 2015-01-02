using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGen.Service {
    public class TemplateFileService : ITemplateFileService {
        public IEnumerable<string> DataContextTemplates { get; private set; }
        public IEnumerable<string> EntityTemplates { get; private set; }
        public IEnumerable<string> MappingTemplates { get; private set; }
        public IEnumerable<string> WcfDataServiceDataContextTemplates { get; private set; }
        public IEnumerable<string> WcfDataServiceClientTemplates { get; private set; }

        [InjectionConstructor]
        public TemplateFileService()
            : this(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)) {
        }
        public TemplateFileService(string applicationPath) {
            string templatePath = Path.Combine(applicationPath, "Template");

            this.DataContextTemplates = this.GetTemplates(Path.Combine(templatePath, "DataContext"));
            this.EntityTemplates = this.GetTemplates(Path.Combine(templatePath, "Entity"));
            this.MappingTemplates = this.GetTemplates(Path.Combine(templatePath, "Mapping"));
            this.WcfDataServiceDataContextTemplates = this.GetTemplates(Path.Combine(templatePath, "WcfDataServiceDataContext"));
            this.WcfDataServiceClientTemplates = this.GetTemplates(Path.Combine(templatePath, "WcfDataServiceClient"));
        }

        private IEnumerable<string> GetTemplates(string path) {
            if (Directory.Exists(path)) {
                return Directory.GetFiles(path, "*.cshtml").Select(x => Path.GetFileNameWithoutExtension(x));
            }

            return new List<string>();
        }
    }
}
