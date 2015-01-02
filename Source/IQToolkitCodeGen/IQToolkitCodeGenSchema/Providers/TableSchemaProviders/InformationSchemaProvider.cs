using System.Data;
using System.Data.Common;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.TableSchemaProviders {
    internal class InformationSchemaProvider : Provider {
        private readonly IDataTableFactory _dataTableFactory;

        public InformationSchemaProvider(ISchemaOptions schemaOptions, IDataTableFactory dataTableFactory)
            : base(schemaOptions) {
            ArgumentUtility.CheckNotNull("dataTableFactory", dataTableFactory);

            this._dataTableFactory = dataTableFactory;
        }

        protected override DataTable GetDataTable(DbConnection connection) {
            var command = connection.CreateCommand();
            var tableTypes = "'Base Table', 'Table'";

            if (!this.SchemaOptions.ExcludeViews) {
                tableTypes += ", 'View'";
            }

            command.CommandText = string.Format(@"
select Table_Name, Table_Type
    from Information_Schema.Tables
    where Table_Type in ({0})", tableTypes);

            return this._dataTableFactory.CreateDataTable(command);
        }
    }
}