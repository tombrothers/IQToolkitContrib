using System.IO;
using System.Text;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Service {
    public class CodeGenSettingsService : ICodeGenSettingsService {
        private readonly ICodeGenSettings _settings;
        private readonly IFileDialogService _fileDialogService;
        private readonly IMostRecentlyUsedFileService _mostRecentlyUsedFileService;
        private readonly IXmlSerializerService _xmlSerializerService;

        public string FileName { get; private set; }

        public CodeGenSettingsService(ICodeGenSettings settings, IFileDialogService fileDialogService, IMostRecentlyUsedFileService mostRecentlyUsedFileService, IXmlSerializerService xmlSerializerService) {
            this._settings = settings;
            this._fileDialogService = fileDialogService;
            this._mostRecentlyUsedFileService = mostRecentlyUsedFileService;
            this._xmlSerializerService = xmlSerializerService;
        }

        public void Reset() {
            this.FileName = null;

            foreach (var property in this._settings.GetType().GetProperties()) {
                if (property.PropertyType == typeof(string)) {
                    property.SetValue(this._settings, string.Empty, null);
                }
                else {
                    property.SetValue(this._settings, null, null);
                }
            }

            this._settings.DataContextBaseClass = "IQToolkitContrib.DataContext";
            this._settings.EntityExtension = ".cs";
            this._settings.WcfDataServiceBaseClass = "IQToolkitContrib.ADataServiceContext";
        }

        public void Save() {
            string fileName = this._fileDialogService.GetSaveFileName();

            if (!string.IsNullOrWhiteSpace(fileName)) {
                this.Save(fileName);
            }
        }

        public void Save(string fileName) {
            this.FileName = fileName;
            File.WriteAllText(fileName, this._xmlSerializerService.ToXml(this._settings), Encoding.Unicode);
            this._mostRecentlyUsedFileService.AddFile(fileName);
            this._settings.IsDirty = false;
        }

        public void Open() {
            string fileName = this._fileDialogService.GetOpenFileName();

            if (!string.IsNullOrWhiteSpace(fileName)) {
                this.Open(fileName);
            }
        }

        public void Open(string fileName) {
            this.FileName = fileName;
            var settings = this._xmlSerializerService.DeserializeFile<CodeGenSettings>(fileName);

            foreach (var property in this._settings.GetType().GetProperties()) {
                object value = property.GetValue(settings, null);
                property.SetValue(this._settings, value, null);
            }

            this._mostRecentlyUsedFileService.AddFile(fileName);
            this._settings.IsDirty = false;
        }
    }
}
