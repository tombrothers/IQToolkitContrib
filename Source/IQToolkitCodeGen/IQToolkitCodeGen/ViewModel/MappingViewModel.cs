using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Event;
using IQToolkitCodeGen.Model;
using IQToolkitCodeGen.Service;
using Microsoft.Practices.Prism.Events;

namespace IQToolkitCodeGen.ViewModel {
    public class MappingViewModel : ViewModelBase, IMappingViewModel {
        private readonly IEventAggregator _eventAggregator;

        public ICommand AllTableSelectionCommand { get; private set; }
        public ICommand AllColumnSelectionCommand { get; private set; }
        public ICommand AllAssociationSelectionCommand { get; private set; }
        public ICommand GenerateFilesCommand { get; private set; }
        public ObservableCollection<Table> Tables { get; private set; }
        public ObservableCollection<Column> Columns { get; private set; }
        public ObservableCollection<Association> Associations { get; private set; }

        #region SelectedTable

        private Table _selectedTable;

        public Table SelectedTable {
            get {
                return this._selectedTable;
            }
            set {
                if (this._selectedTable != value) {
                    this._selectedTable = value;
                    this.OnPropertyChanged(() => SelectedTable);
                    this.UpdateColumns(value);
                    this.UpdateAssociations(value);
                }
            }
        }

        #endregion

        #region SelectedColumn

        private Column _selectedColumn;

        public Column SelectedColumn {
            get {
                return this._selectedColumn;
            }
            set {
                if (this._selectedColumn != value) {
                    this._selectedColumn = value;
                    this.OnPropertyChanged(() => SelectedColumn);
                }
            }
        }

        #endregion

        #region SelectedAssociation

        private Association _selectedAssociation;

        public Association SelectedAssociation {
            get {
                return this._selectedAssociation;
            }
            set {
                if (this._selectedAssociation != value) {
                    this._selectedAssociation = value;
                    this.OnPropertyChanged(() => SelectedAssociation);
                }
            }
        }

        #endregion

        #region AllTablesSelected

        private bool _allTablesSelected;

        public bool AllTablesSelected {
            get {
                return this._allTablesSelected;
            }
            set {
                if (this._allTablesSelected != value) {
                    this._allTablesSelected = value;
                    this.OnPropertyChanged(() => AllTablesSelected);
                }
            }
        }

        #endregion

        #region AllColumnsSelected

        private bool _allColumnsSelected;

        public bool AllColumnsSelected {
            get {
                return this._allColumnsSelected;
            }
            set {
                if (this._allColumnsSelected != value) {
                    this._allColumnsSelected = value;
                    this.OnPropertyChanged(() => AllColumnsSelected);
                }
            }
        }

        #endregion

        #region AllAssociationsSelected

        private bool _allAssociationsSelected;

        public bool AllAssociationsSelected {
            get {
                return this._allAssociationsSelected;
            }
            set {
                if (this._allAssociationsSelected != value) {
                    this._allAssociationsSelected = value;
                    this.OnPropertyChanged(() => AllAssociationsSelected);
                }
            }
        }

        #endregion

        public MappingViewModel(ICodeGenSettings settings, IEventAggregator eventAggregator) {
            this.Tables = new ObservableCollection<Table>();

            this.Columns = new ObservableCollection<Column>();
            this.Associations = new ObservableCollection<Association>();

            this._eventAggregator = eventAggregator;

            this.UpdateTables(settings.Tables);

            this.AllTableSelectionCommand = new Command(x => this.ExecuteAllTableSelectionCommand());
            this.AllColumnSelectionCommand = new Command(x => this.ExecuteAllColumnSelectionCommand());
            this.AllAssociationSelectionCommand = new Command(x => this.ExecuteAllAssociateSelectionCommand());
            this.GenerateFilesCommand = new Command(x => eventAggregator.GetEvent<GenerateFilesEvent>().Publish(null));

            this._eventAggregator.GetEvent<TablesChangedEvent>().Subscribe(this.UpdateTables);
        }

        private void ExecuteAllAssociateSelectionCommand() {
            foreach (var association in this.Associations) {
                association.Selected = this.AllAssociationsSelected;
            }
        }

        private void ExecuteAllColumnSelectionCommand() {
            foreach (var column in this.Columns) {
                column.Selected = this.AllColumnsSelected;
            }
        }

        private void ExecuteAllTableSelectionCommand() {
            foreach (var table in this.Tables) {
                table.Selected = this.AllTablesSelected;
            }
        }

        private void UpdateAssociations(Table table) {
            this.Associations.Clear();
            this.SelectedAssociation = null;

            if (table != null) {
                foreach (var association in table.Associations) {
                    this.Associations.Add(association);
                }

                this.SelectedAssociation = table.Associations.FirstOrDefault();
            }

            this.UpdateAllAssociationsSelected();
        }

        private void UpdateAllAssociationsSelected() {
            this.AllAssociationsSelected = this.Associations.Any() && this.Associations.All(x => x.Selected);
        }

        private void UpdateColumns(Table table) {
            this.Columns.Clear();
            this.SelectedColumn = null;

            if (table != null) {
                foreach (var column in table.Columns) {
                    this.Columns.Add(column);
                }

                this.SelectedColumn = table.Columns.FirstOrDefault();
            }

            this.UpdateAllColumnsSelected();
        }

        private void UpdateAllColumnsSelected() {
            this.AllColumnsSelected = this.Columns.Any() && this.Columns.All(x => x.Selected);
        }

        private void UpdateTables(IEnumerable<Table> tables) {
            this.Tables.Clear();
            this.SelectedTable = null;

            if (tables != null) {
                foreach (var table in tables) {
                    this.Tables.Add(table);
                }

                this.SelectedTable = tables.FirstOrDefault();
            }

            this.UpdateAllTablesSelected();
        }

        private void UpdateAllTablesSelected() {
            this.AllTablesSelected = this.Tables.Any() && this.Tables.All(x => x.Selected);
        }
    }
}
