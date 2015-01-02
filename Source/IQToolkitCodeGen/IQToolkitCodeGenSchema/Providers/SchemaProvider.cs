using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Services;
using Microsoft.Practices.Unity;
using System.Diagnostics;

namespace IQToolkitCodeGenSchema.Providers {
    internal class SchemaProvider : ISchemaProvider {
        private readonly IColumnSchemaServiceFactory _columnSchemaServiceFactory;
        private readonly IPrimaryKeySchemaServiceFactory _primaryKeySchemaServiceFactory;
        private readonly IAssociationSchemaService _associationSchemaService;
        private readonly IDbTypeService _dbTypeService;
        private readonly ITableSchemaService _tableSchemaService;
        private readonly IPropertyNameDeDuplicateService _propertyNameDeDuplicateService;
        private readonly IList<IColumnTypeSchema> _columnTypes;
        private readonly IPluralizationService _pluralizationService;

        internal static ISchemaProvider Create(ISchemaOptions schemaOptions) {
            ArgumentUtility.CheckNotNull("schemaOptions", schemaOptions);
            ArgumentUtility.CheckNotNull("schemaOptions.Database", schemaOptions.Database);
            ArgumentUtility.CheckNotNullOrEmpty("schemaOptions.ConnectionString", schemaOptions.ConnectionString);

            var container = new UnityContainer();
            ContainerConfigurator.Configure(container, schemaOptions);

            return container.Resolve<ISchemaProvider>();
        }

        public SchemaProvider(IColumnSchemaServiceFactory columnSchemaServiceFactory,
                              IPrimaryKeySchemaServiceFactory primaryKeySchemaServiceFactory,
                              ITableSchemaService tableSchemaService,
                              IColumnTypeSchemaService columnTypeSchemaService,
                              IDbTypeService dbTypeService,
                              IAssociationSchemaService associationSchemaService,
                              IPluralizationService pluralizationService,
                              IPropertyNameDeDuplicateService propertyNameDeDuplicateService) {
            ArgumentUtility.CheckNotNull("columnSchemaServiceFactory", columnSchemaServiceFactory);
            ArgumentUtility.CheckNotNull("primaryKeySchemaServiceFactory", primaryKeySchemaServiceFactory);
            ArgumentUtility.CheckNotNull("tableSchemaService", tableSchemaService);
            ArgumentUtility.CheckNotNull("columnTypeSchemaService", columnTypeSchemaService);
            ArgumentUtility.CheckNotNull("dbTypeService", dbTypeService);
            ArgumentUtility.CheckNotNull("associationSchemaService", associationSchemaService);
            ArgumentUtility.CheckNotNull("pluralizationService", pluralizationService);
            ArgumentUtility.CheckNotNull("propertyNameDeDuplicateService", propertyNameDeDuplicateService);

            this._columnSchemaServiceFactory = columnSchemaServiceFactory;
            this._primaryKeySchemaServiceFactory = primaryKeySchemaServiceFactory;

            this._tableSchemaService = tableSchemaService;
            this._dbTypeService = dbTypeService;
            this._associationSchemaService = associationSchemaService;
            this._propertyNameDeDuplicateService = propertyNameDeDuplicateService;

            this._columnTypes = columnTypeSchemaService.GetSchema();
            this._pluralizationService = pluralizationService;
        }

        public IEnumerable<ITable> GetSchema() {
            List<ITable> tables = new List<ITable>();

            foreach (var tableSchema in this.GetTableSchema()) {
                var table = new Table(tableSchema, this._pluralizationService);

                table.Columns = this.GetColumns(tableSchema);
                table.Associations = this.GetAssociations(tableSchema);

                tables.Add(table);

                var propertyNames = table.Columns.Cast<IPropertyName>().Union(table.Associations.Cast<IPropertyName>());
                this._propertyNameDeDuplicateService.Deduplicate(propertyNames);
            }

            return tables;
        }

        private IEnumerable<IAssociation> GetAssociations(ITableSchema tableSchema) {
            var list = new List<IAssociation>();

            foreach (var associationSchema in tableSchema.Associations) {
                list.Add(new Association(associationSchema));
            }

            return list;
        }

        private IEnumerable<IColumn> GetColumns(ITableSchema tableSchema) {
            var list = new List<IColumn>();

            foreach (var columnSchema in tableSchema.Columns) {
                var column = new Column(columnSchema);
                var columnType = this.GetColumnType(columnSchema);

                if (columnType != null) {
                    column.PropertyType = columnType.Type.FullName;

                    var primaryKey = tableSchema.PrimaryKeys.FirstOrDefault(x => x.ColumnName.Equals(columnSchema.ColumnName, StringComparison.InvariantCultureIgnoreCase));

                    if (primaryKey != null) {
                        column.PrimaryKey = true;
                        column.Generated = primaryKey.AutoIncrement;
                    }

                    column.DbType = this._dbTypeService.GetDbType(columnSchema, columnType, primaryKey);
                }

                list.Add(column);
            }

            return list;
        }

        private IColumnTypeSchema GetColumnType(IColumnSchema columnSchema) {
            var columnType = this._columnTypes.FirstOrDefault(x => x.ColumnType.Equals(columnSchema.ColumnType, StringComparison.InvariantCultureIgnoreCase));

            if (columnType == null) {
                var index = columnSchema.ColumnType.IndexOf("(");

                if (index >= 0) {
                    var columnTypeName = columnSchema.ColumnType.Substring(0, index);
                    columnType = this._columnTypes.FirstOrDefault(x => x.ColumnType.Equals(columnTypeName, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return columnType;
        }

        private IList<ITableSchema> GetTableSchema() {
            IList<ITableSchema> tables = null;
            IList<IAssociationSchema> associations = null;

            try {
                Parallel.Invoke(() => { tables = this._tableSchemaService.GetSchema(); },
                                () => { associations = this._associationSchemaService.GetSchema(); });

                Parallel.ForEach(tables, (table) => {
                    var primaryKeysProvider = this._primaryKeySchemaServiceFactory.Create();
                    table.PrimaryKeys = primaryKeysProvider.GetSchema(table.TableName);

                    var columnSchemaService = this._columnSchemaServiceFactory.Create();
                    table.Columns = columnSchemaService.GetSchema(table.TableName);

                    table.Associations = associations.Where(x => x.TableName == table.TableName).ToList();
                });
            }
            catch (AggregateException ex) {
                if (ex.InnerExceptions.Count == 1) {
                    throw ex.InnerException;
                }

                throw ex.InnerExceptions[0];
            }

            return tables;
        }
    }
}