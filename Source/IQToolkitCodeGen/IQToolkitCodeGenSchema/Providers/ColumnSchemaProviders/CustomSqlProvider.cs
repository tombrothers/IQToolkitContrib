using System.Data;
using System.Data.Common;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.ColumnSchemaProviders {
    internal class CustomSqlProvider : Provider {
        private readonly ISchemaOptions _schemaOptions;
        private readonly IDataTableFactory _dataTableFactory;

        public CustomSqlProvider(ISchemaOptions schemaOptions, IDataTableFactory dataTableFactory) {
            ArgumentUtility.CheckNotNull("schemaOptions", schemaOptions);
            ArgumentUtility.CheckNotNull("dataTableFactory", dataTableFactory);

            this._schemaOptions = schemaOptions;
            this._dataTableFactory = dataTableFactory;
        }

        protected override DataTable GetDataTable(DbConnection connection, string tableName) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = this._schemaOptions.ColumnSchemaSql.Replace("{{tableName}}", tableName);
                
                return this._dataTableFactory.CreateDataTable(command);
            }
        }
    }
}