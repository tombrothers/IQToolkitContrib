using System.Data;
using System.Data.Common;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.TableSchemaProviders {
    internal class CustomSqlProvider : Provider {
        private readonly IDataTableFactory _dataTableFactory;

        public CustomSqlProvider(ISchemaOptions schemaOptions, IDataTableFactory dataTableFactory)
            : base(schemaOptions) {
            ArgumentUtility.CheckNotNull("dataTableFactory", dataTableFactory);

            this._dataTableFactory = dataTableFactory;
        }

        protected override DataTable GetDataTable(DbConnection connection) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = this.SchemaOptions.TableSchemaSql;

                return this._dataTableFactory.CreateDataTable(command);
            }
        }
    }
}
