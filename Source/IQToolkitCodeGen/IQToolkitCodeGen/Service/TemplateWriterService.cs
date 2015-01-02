using System.IO;
using IQToolkitCodeGen.Model;
using IQToolkitCodeGen.Template;

namespace IQToolkitCodeGen.Service {
    public class TemplateWriterService : ITemplateWriterService {
        private readonly string _basePath;
        private readonly ICodeGenSettings _codeGenSettings;

        public TemplateWriterService(IApplicationService applicationService, ICodeGenSettings codeGenSettings) {
            this._codeGenSettings = codeGenSettings;
            this._basePath = applicationService.TemplatePath;
        }

        public void CreateFiles() {
            if (!string.IsNullOrEmpty(this._codeGenSettings.DataContextTemplate)) {
                this.CreateDataContext();
            }

            if (!string.IsNullOrEmpty(this._codeGenSettings.MappingTemplate)) {
                this.CreateMapping();
            }

            if (!string.IsNullOrEmpty(this._codeGenSettings.EntityTemplate)) {
                this.CreateEntityFiles();
            }

            if (!string.IsNullOrEmpty(this._codeGenSettings.WcfDataServiceTemplate)) {
                this.CreateWcfDataService();
            }

            if (!string.IsNullOrEmpty(this._codeGenSettings.WcfDataServiceClientTemplate)) {
                this.CreateWcfDataServiceClient();
            }
        }

        private void CreateWcfDataServiceClient() {
            string templatePath = this.GetTemplateFilePath(this._codeGenSettings.WcfDataServiceClientTemplate, "WcfDataServiceClient");
            
            var engine = this.CreateTemplateEngine<WcfDataServiceTemplate>(templatePath);
            engine.Template.Tables = this._codeGenSettings.Tables;
            engine.Template.BaseClass = this._codeGenSettings.WcfDataServiceClientBaseClass;
            engine.Template.Namespace = this._codeGenSettings.WcfDataServiceClientNamespace;
            engine.Template.ClassName = this._codeGenSettings.WcfDataServiceClientClassName;
            engine.Template.EntityNamespace = this._codeGenSettings.EntityNamespace;

            engine.ToFile(this._codeGenSettings.WcfDataServiceClientOutputFile);
        }

        private void CreateWcfDataService() {
            string templatePath = this.GetTemplateFilePath(this._codeGenSettings.WcfDataServiceTemplate, "WcfDataServiceDataContext");
            
            var engine = this.CreateTemplateEngine<WcfDataServiceTemplate>(templatePath);
            engine.Template.Tables = this._codeGenSettings.Tables;
            engine.Template.BaseClass = this._codeGenSettings.WcfDataServiceBaseClass;
            engine.Template.Namespace = this._codeGenSettings.WcfDataServiceNamespace;
            engine.Template.ClassName = this._codeGenSettings.WcfDataServiceClassName;
            engine.Template.EntityNamespace = this._codeGenSettings.EntityNamespace;
            engine.Template.DataContextClass = this._codeGenSettings.DataContextNamespace + "." + this._codeGenSettings.DataContextClassName;

            engine.ToFile(this._codeGenSettings.WcfDataServiceOutputFile);
        }

        private void CreateDataContext() {
            string templatePath = this.GetTemplateFilePath(this._codeGenSettings.DataContextTemplate, "DataContext");
            
            var engine = this.CreateTemplateEngine<DataContextTemplate>(templatePath);
            
            engine.Template.Tables = this._codeGenSettings.Tables;
            engine.Template.BaseClass = this._codeGenSettings.DataContextBaseClass;
            engine.Template.Namespace = this._codeGenSettings.DataContextNamespace;
            engine.Template.ClassName = this._codeGenSettings.DataContextClassName;

            engine.ToFile(this._codeGenSettings.DataContextOutputFile);
        }

        private void CreateMapping() {
            string templatePath = this.GetTemplateFilePath(this._codeGenSettings.MappingTemplate, "Mapping");

            var engine = this.CreateTemplateEngine<MappingTemplate>(templatePath);
            engine.Template.Tables = this._codeGenSettings.Tables;
            engine.Template.ClassName = this._codeGenSettings.DataContextClassName;
            engine.Template.Namespace = this._codeGenSettings.DataContextNamespace;

            engine.ToFile(this._codeGenSettings.MappingOutputFile);
        }

        private void CreateEntityFiles() {
            if (!Directory.Exists(this._codeGenSettings.EntityOutputPath)) {
                Directory.CreateDirectory(this._codeGenSettings.EntityOutputPath);
            }

            string templatePath = this.GetTemplateFilePath(this._codeGenSettings.EntityTemplate, "Entity");

            var engine = this.CreateTemplateEngine<EntityTemplate>(templatePath);
            engine.Template.Namespace = this._codeGenSettings.EntityNamespace;

            for (int index = 0, total = this._codeGenSettings.Tables.Count; index < total; index++) {
                Table table = this._codeGenSettings.Tables[index];

                if (table.ShouldInclude()) {
                    string entityFile = Path.Combine(this._codeGenSettings.EntityOutputPath, table.EntityName);
                    entityFile += (this._codeGenSettings.EntityExtension.StartsWith(".") ? string.Empty : ".") + this._codeGenSettings.EntityExtension;
                    engine.Template.Table = table;
                    engine.ToFile(entityFile);
                    engine.Template.Clear();
                }
            }
        }

        private string GetTemplateFilePath(string template, string subfolder) {
            if (!File.Exists(template)) {
                template = Path.ChangeExtension(Path.Combine(this._basePath, subfolder, template), "cshtml");
            }

            return template;
        }

        private TemplateEngine<T> CreateTemplateEngine<T>(string templateFile)
            where T : ITemplateBase, new() {
            FileInfo fi = new FileInfo(templateFile);

            if (!fi.Exists) {
                throw new FileNotFoundException(templateFile);
            }

            return new TemplateEngine<T>(File.ReadAllText(fi.FullName));
        }
    }
}
