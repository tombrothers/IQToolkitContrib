using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IQToolkitCodeGen.Model;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers;
using IQToolkitCodeGenSchema;
using System.Windows.Threading;

namespace IQToolkitCodeGen.Service {
    public class SchemaService : ISchemaService {
        private readonly IDatabaseProvider _databaseProvider;
        private readonly ICodeGenSettings _codeGenSettings;
        private readonly IMessageService _messageService;
        private readonly ICodeGenSettingsService _codeGenSettingsService;
        private readonly Dispatcher _dispatcher;

        private IDatabase _lastUsedDatabase = null;
        private string _lastUsedConnectionString = null;

        public ReadOnlyCollection<IDatabase> Databases {
            get {
                return this._databaseProvider.Databases;
            }
        }

        public SchemaService(IDatabaseProvider databaeProvider, ICodeGenSettings codeGenSettings, IMessageService messageService, ICodeGenSettingsService codeGenSettingsService) {
            this._databaseProvider = databaeProvider;
            this._codeGenSettings = codeGenSettings;
            this._messageService = messageService;
            this._codeGenSettingsService = codeGenSettingsService;
            this._dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void GetSchema(IDatabase database) {
            List<Table> tables = this.GetTables(database);

            if (tables == null) {
                throw new ApplicationException("Error loading tables.");
            }

            Action action = () => this.SetTables(database, tables);
            this._dispatcher.Invoke(action);

            this._lastUsedDatabase = database;
            this._lastUsedConnectionString = this._codeGenSettings.ConnectionString;
        }

        private void SetTables(IDatabase database, List<Table> tables) {
            if (this.IsFirstDataLoad(database)) {
                this.UpdateAssociations(tables);
                this._codeGenSettings.Tables = tables;

                if (string.IsNullOrEmpty(this._codeGenSettingsService.FileName)) {
                    this.SelectAll(this._codeGenSettings.Tables);
                }
                else {
                    if (this._messageService.ShowSelectAll("Select all tables, columns and associations?")) {
                        this.SelectAll(this._codeGenSettings.Tables);
                    }
                }
            }
            else {
                this._codeGenSettings.Tables = this.SyncTables(this._codeGenSettings.Tables, tables);

                if (this._messageService.ShowSelectAll("Select all tables, columns and associations?")) {
                    this.SelectAll(this._codeGenSettings.Tables);
                }
            }
        }

        private List<Table> GetTables(IDatabase database) {
            if (database == null) {
                throw new ApplicationException("Provider not selected.");
            }

            var schemaOptions = new SchemaOptions {
                Database = database,
                ConnectionString = this._codeGenSettings.ConnectionString,
                ExcludeViews = this._codeGenSettings.ExcludeViews,
                NoPluralization = this._codeGenSettings.NoPluralization,
                TableSchemaSql = this._codeGenSettings.TableSchemaSql,
                ColumnSchemaSql = this._codeGenSettings.ColumnSchemaSql,
                AssociationSchemaSql = this._codeGenSettings.AssociationSchemaSql
            };

            var tables = Schema.GetSchema(schemaOptions).ToList();

            if (tables == null) {
                throw new ApplicationException("Error loading tables.");
            }

            return tables.Select(table => new Table(table.TableName) {
                EntityName = table.EntityName,
                Columns = table.Columns.Select(column => new Column(column)).ToList(),
                Associations = table.Associations.Select(association => new Association(association)).ToList()
            }).ToList();
        }

        private void SelectAll(IEnumerable<Table> tables) {
            if (tables != null) {
                foreach (var table in tables) {
                    table.Selected = true;

                    foreach (var column in table.Columns) {
                        column.Selected = true;
                    }

                    foreach (var association in table.Associations) {
                        association.Selected = true;
                    }
                }
            }
        }

        private bool IsFirstDataLoad(IDatabase database) {
            return this._codeGenSettings.Tables == null
                    || !this._codeGenSettings.Tables.Any()
                    || this._lastUsedDatabase == null
                    || string.IsNullOrEmpty(this._lastUsedConnectionString)
                    || this._lastUsedDatabase != database
                    || this._lastUsedConnectionString != this._codeGenSettings.ConnectionString;
        }

        private List<Table> SyncTables(List<Table> tables1, List<Table> tables2) {
            if (tables1 == null) {
                throw new ArgumentNullException("tables2");
            }

            if (tables2 == null) {
                throw new ArgumentNullException("tables2");
            }

            // get a list of tables that exist in both lists.  This will remove any deleted tables.
            List<Table> tables = (from t1 in tables1
                                  join t2 in tables2 on t1.TableName.ToLower() equals t2.TableName.ToLower()
                                  select t1).ToList();

            for (int tableIndex = 0, tableTotal = tables2.Count; tableIndex < tableTotal; tableIndex++) {
                Table newTable = tables2[tableIndex];
                Table table = tables.Find(d => d.TableName.Equals(newTable.TableName, StringComparison.CurrentCultureIgnoreCase));

                if (table == null) {
                    tables.Add(newTable);
                }
                else {
                    table.TableName = newTable.TableName;

                    // get a list of associations that exist in both lists.  This will remove any deleted associations.
                    table.Associations = (from c1 in (table.Associations ?? new List<Association>())
                                          join c2 in newTable.Associations on c1.AssociationName.ToLower() equals c2.AssociationName.ToLower()
                                          select c1).ToList();

                    // get a list of columns that exist in both lists.  This will remove any deleted columns.
                    table.Columns = (from c1 in (table.Columns ?? new List<Column>())
                                     join c2 in newTable.Columns on c1.ColumnName.ToLower() equals c2.ColumnName.ToLower()
                                     select c1).ToList();

                    newTable.Associations = newTable.Associations ?? new List<Association>();

                    if (newTable.Columns != null && newTable.Columns.Count > 0) {
                        for (int columnIndex = 0, columnTotal = newTable.Columns.Count; columnIndex < columnTotal; columnIndex++) {
                            Column newColumn = newTable.Columns[columnIndex];

                            Column column = table.Columns.Find(d => d.ColumnName.Equals(newColumn.ColumnName, StringComparison.CurrentCultureIgnoreCase));

                            if (column == null) {
                                table.Columns.Add(newColumn);
                            }
                            else {
                                column.ColumnName = newColumn.ColumnName;
                                column.MaxLength = newColumn.MaxLength;
                            }

                            Association newAssociation = newTable.Associations.Find(d => d.TableName.Equals(table.TableName, StringComparison.CurrentCultureIgnoreCase) && d.ColumnName.Equals(column.ColumnName, StringComparison.CurrentCultureIgnoreCase));

                            if (newAssociation != null) {
                                newAssociation.Table = table;
                                newAssociation.Column = column;

                                Association association = table.Associations.Find(d => d.ColumnName.Equals(column.ColumnName, StringComparison.CurrentCultureIgnoreCase));

                                if (association == null) {
                                    table.Associations.Add(newAssociation);
                                }
                                else {
                                    association.Update(newAssociation);
                                }
                            }
                        }
                    }

                    table.Columns = table.Columns.OrderBy(column => column.PropertyName).ToList();
                }
            }

            this.UpdateAssociations(tables);
            return tables.OrderBy(table => table.EntityName).ToList();
        }

        private void UpdateAssociations(IEnumerable<Table> tables) {
            foreach (var table in tables) {
                foreach (var association in table.Associations) {
                    association.Table = table;
                    association.Column = association.Table.Columns.Find(c => c.ColumnName.Equals(association.ColumnName, StringComparison.InvariantCultureIgnoreCase));
                    association.RelatedTable = tables.FirstOrDefault(t => t.TableName.Equals(association.RelatedTableName, StringComparison.InvariantCultureIgnoreCase));

                    if (association.RelatedTable != null) {
                        association.RelatedColumn = association.RelatedTable.Columns.Find(c => c.ColumnName.Equals(association.ColumnName, StringComparison.InvariantCultureIgnoreCase));

                        if (association.RelatedColumn == null) {
                            association.RelatedColumn = association.RelatedTable.Columns.Find(c => c.ColumnName.Equals(association.RelatedColumnName, StringComparison.InvariantCultureIgnoreCase));
                        }
                    }
                }

                table.Associations = table.Associations.OrderBy(association => association.PropertyName).ToList();
            }
        }
    }
}
