using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IQToolkitCodeGenSchema.Schema;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGenSchema {
    public class SchemaProvider : ISchemaProvider {
        private readonly IUnityContainer _container;
        private readonly TableSchemaProvider _tableSchemaProvider;
        private readonly AssociationSchemaProvider _associationSchemaProvider;
        private readonly DbTypeProvider _dbTypeProvider;
        private readonly IList<ColumnTypeSchema> _columnTypes;
        private readonly DuplicatePropertyNameResolver _duplicatePropertyNameResolver;
        private readonly IPluralizationService _pluralizationService;

        public SchemaProvider(ISchemaOptions schemaOptions) {
            ArgumentUtility.CheckNotNull("schemaOptions", schemaOptions);
            ArgumentUtility.CheckNotNull("schemaOptions.Database", schemaOptions.Database);
            ArgumentUtility.CheckNotNullOrEmpty("schemaOptions.ConnectionString", schemaOptions.ConnectionString);

            this._container = ContainerConfigurator.Configure(new UnityContainer(), schemaOptions);

            this._tableSchemaProvider = this._container.Resolve<TableSchemaProvider>();
            this._associationSchemaProvider = this._container.Resolve<AssociationSchemaProvider>();
            this._dbTypeProvider = this._container.Resolve<DbTypeProvider>();

            var columnTypeSchemaProvider = this._container.Resolve<ColumnTypeSchemaProvider>();
            this._columnTypes = columnTypeSchemaProvider.GetSchema();
            this._duplicatePropertyNameResolver = this._container.Resolve<DuplicatePropertyNameResolver>();
            this._pluralizationService = this._container.Resolve<PluralizationService>();
        }

        public IEnumerable<ITable> GetSchema() {
            List<ITable> tables = new List<ITable>();

            foreach (var tableSchema in this.GetTableSchema()) {
                var table = new Table(tableSchema, this._pluralizationService);

                table.Columns = this.GetColumns(tableSchema);
                table.Associations = this.GetAssociations(tableSchema);

                tables.Add(table);

                var propertyNames = table.Columns.Cast<IPropertyName>().Union(table.Associations.Cast<IPropertyName>());
                this._duplicatePropertyNameResolver.Deduplicate(propertyNames);
            }
            
            return tables;
        }

        private IEnumerable<IAssociation> GetAssociations(TableSchema tableSchema) {
            var list = new List<IAssociation>();

            foreach (var associationSchema in tableSchema.Associations) {
                list.Add(new Association(associationSchema));
            }

            return list;
        }

        private IEnumerable<IColumn> GetColumns(TableSchema tableSchema) {
            var list = new List<IColumn>();

            foreach (var columnSchema in tableSchema.Columns) {
                var column = new Column(columnSchema);
                var columnType = this._columnTypes.First(x => x.ColumnType.Equals(columnSchema.ColumnType, StringComparison.InvariantCultureIgnoreCase));
                column.PropertyType = columnType.Type.FullName;

                var primaryKey = tableSchema.PrimaryKeys.FirstOrDefault(x => x.ColumnName.Equals(columnSchema.ColumnName, StringComparison.InvariantCultureIgnoreCase));

                if (primaryKey != null) {
                    column.PrimaryKey = true;
                    column.Generated = primaryKey.AutoIncrement;
                }

                column.DbType = this._dbTypeProvider.GetDbType(columnSchema, columnType, primaryKey);
                list.Add(column);
            }

            return list;
        }

        private IList<TableSchema> GetTableSchema() {
            IList<TableSchema> tables = null;
            IList<AssociationSchema> associations = null;

            Parallel.Invoke(() => { tables = this._tableSchemaProvider.GetSchema(); },
                            () => { associations = this._associationSchemaProvider.GetSchema(); });

            Parallel.ForEach(tables, (table) => {
                var primaryKeysProvider = this._container.Resolve<PrimaryKeySchemaProvider>();
                table.PrimaryKeys = primaryKeysProvider.GetSchema(table.TableName);

                var columnsProvider = this._container.Resolve<ColumnSchemaProvider>();
                table.Columns = columnsProvider.GetSchema(table.TableName);

                table.Associations = associations.Where(x => x.TableName == table.TableName).ToList();
            });

            return tables;
        }
    }
}