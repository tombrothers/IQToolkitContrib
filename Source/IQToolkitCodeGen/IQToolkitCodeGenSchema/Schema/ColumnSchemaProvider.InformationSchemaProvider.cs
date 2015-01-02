using System.Data;
using System.Data.Common;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class ColumnSchemaProvider {
        private class InformationSchemaProvider : Provider {
            private readonly DataTableFactory _dataTableFactory;

            public InformationSchemaProvider(DataTableFactory dataTableFactory) {
                ArgumentUtility.CheckNotNull("dataTableFactory", dataTableFactory);

                this._dataTableFactory = dataTableFactory;
            }

            protected override DataTable GetDataTable(DbConnection connection, string tableName) {
                var command = connection.CreateCommand();

                command.CommandText = string.Format(@"
select Table_Name, Column_Name, Data_Type, Is_Nullable, Character_Maximum_Length, Numeric_Precision, Numeric_Scale
    from information_schema.columns
    where Table_Name = '{0}'", tableName.Replace("'", "''"));

                return this._dataTableFactory.CreateDataTable(command);
            }
        }
    }
}
