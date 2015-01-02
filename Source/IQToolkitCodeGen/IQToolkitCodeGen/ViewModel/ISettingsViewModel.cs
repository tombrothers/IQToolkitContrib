using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IQToolkitCodeGen.Model;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGen.ViewModel {
    public interface ISettingsViewModel {
        IEnumerable<string> DataContextTemplates { get; }
        IEnumerable<string> EntityTemplates { get; }
        ICommand LoadSchemaCommand { get; }
        ICommand CustomizeSchemaSqlCommand { get; }
        IEnumerable<string> MappingTemplates { get; }
        ObservableCollection<IDatabase> Databases { get; }
        IDatabase SelectedDatabase { get; set; }
        ICodeGenSettings Settings { get; }
        IEnumerable<string> WcfDataServiceClientTemplates { get; }
        IEnumerable<string> WcfDataServiceDataContextTemplates { get; }
    }
}
