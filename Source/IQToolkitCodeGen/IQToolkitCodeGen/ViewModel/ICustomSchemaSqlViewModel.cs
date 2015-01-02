using System;
using System.Windows.Input;

namespace IQToolkitCodeGen.ViewModel {
    public interface ICustomSchemaSqlViewModel {
        string AssociationSchemaSql { get; set; }
        string ColumnSchemaSql { get; set; }
        string TableSchemaSql { get; set; }
        ICommand OkCommand { get; set; }
        ICommand CloseCommand { get; set; }
    }
}
