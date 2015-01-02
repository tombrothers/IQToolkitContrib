using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Event;
using IQToolkitCodeGen.Model;
using Microsoft.Practices.Prism.Events;

namespace IQToolkitCodeGen.ViewModel {
    public class CustomSchemaSqlViewModel : ViewModelBase, ICustomSchemaSqlViewModel {
        #region AssociationSchemaSql

        private string _associationSchemaSql;

        public string AssociationSchemaSql {
            get { return this._associationSchemaSql; }
            set {
                if (this._associationSchemaSql != value) {
                    this._associationSchemaSql = value;
                    this.OnPropertyChanged(() => AssociationSchemaSql);
                }
            }
        }

        #endregion

        #region ColumnSchemaSql

        private string _columnSchemaSql;

        public string ColumnSchemaSql {
            get { return this._columnSchemaSql; }
            set {
                if (this._columnSchemaSql != value) {
                    this._columnSchemaSql = value;
                    this.OnPropertyChanged(() => ColumnSchemaSql);
                }
            }
        }

        #endregion

        #region TableSchemaSql

        private string _tableSchemaSql;

        public string TableSchemaSql {
            get { return this._tableSchemaSql; }
            set {
                if (this._tableSchemaSql != value) {
                    this._tableSchemaSql = value;
                    this.OnPropertyChanged(() => TableSchemaSql);
                }
            }
        }

        #endregion

        public ObservableCollection<SchemaInfo> AssociationSchemaInfo { get; private set; }
        public ObservableCollection<SchemaInfo> ColumnSchemaInfo { get; private set; }
        public ObservableCollection<SchemaInfo> TableSchemaInfo { get; private set; }
        public ICommand OkCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        private readonly ICodeGenSettings _codeGenSettings;

        public CustomSchemaSqlViewModel(IEventAggregator eventAggregator, ICodeGenSettings codeGenSettings) {
            ArgumentUtility.CheckNotNull("eventAggregator", eventAggregator);
            ArgumentUtility.CheckNotNull("codeGenSettings", codeGenSettings);

            this._codeGenSettings = codeGenSettings;
            this._associationSchemaSql = codeGenSettings.AssociationSchemaSql;
            this._columnSchemaSql = codeGenSettings.ColumnSchemaSql;
            this._tableSchemaSql = codeGenSettings.TableSchemaSql;

            this.OkCommand = new Command(_ => this.ExecuteOkCommand());
            this.CloseCommand = new Command(_ => eventAggregator.GetEvent<CustomSchemaSqlViewVisibilityChangedEvent>().Publish(false));
            this.SetSchemaInfo();
        }

        private void SetSchemaInfo() {
            this.AssociationSchemaInfo = new ObservableCollection<SchemaInfo>(new List<SchemaInfo> { 
                new SchemaInfo { ColumnName = "ForeignKey", Type = "string" },
                new SchemaInfo { ColumnName = "TableName", Type = "string" },
                new SchemaInfo { ColumnName = "ColumnName", Type = "string" },
                new SchemaInfo { ColumnName = "RelatedTableName", Type = "string" },
                new SchemaInfo { ColumnName = "RelatedColumnName", Type = "string" }
            });

            this.ColumnSchemaInfo = new ObservableCollection<SchemaInfo>(new List<SchemaInfo> { 
                new SchemaInfo { ColumnName = "Column_Name", Type = "string" },
                new SchemaInfo { ColumnName = "Data_Type", Type = "string" },
                new SchemaInfo { ColumnName = "Column_Default", Type = "string" },
                new SchemaInfo { ColumnName = "Is_Nullable", Type = "bool" },
                new SchemaInfo { ColumnName = "Character_Maximum_Length", Type = "long?"},
                new SchemaInfo { ColumnName = "Numeric_Precision", Type = "short?"},
                new SchemaInfo { ColumnName = "Numeric_Scale", Type = "short?"}
            });

            this.TableSchemaInfo = new ObservableCollection<SchemaInfo>(new List<SchemaInfo> { 
                new SchemaInfo { ColumnName = "Table_Name", Type = "string"},
                new SchemaInfo { ColumnName = "Table_Type", Type = "string"}
            });
        }

        public void ExecuteOkCommand() {
            this._codeGenSettings.AssociationSchemaSql = this._associationSchemaSql;
            this._codeGenSettings.ColumnSchemaSql = this._columnSchemaSql;
            this._codeGenSettings.TableSchemaSql = this._tableSchemaSql;
            this.CloseCommand.Execute(null);
        }
    }
}