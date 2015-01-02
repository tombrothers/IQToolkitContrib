using System.Collections.ObjectModel;
using System.Windows.Input;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.ViewModel {
    public interface IMappingViewModel {
        ICommand AllTableSelectionCommand { get; }
        ICommand AllColumnSelectionCommand { get; }
        ICommand AllAssociationSelectionCommand { get; }
        ICommand GenerateFilesCommand { get; }
        ObservableCollection<Table> Tables { get; }
        Table SelectedTable { get; set; }
        Column SelectedColumn { get; set; }
        Association SelectedAssociation { get; set; }
        bool AllTablesSelected { get; set; }
        bool AllColumnsSelected { get; set; }
        bool AllAssociationsSelected { get; set; }
    }
}
