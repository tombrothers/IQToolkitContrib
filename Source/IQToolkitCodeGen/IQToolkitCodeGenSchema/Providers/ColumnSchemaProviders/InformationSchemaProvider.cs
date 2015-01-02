using System.Data;
using System.Data.Common;
using IQToolkitCodeGenSchema.Factories;

namespace IQToolkitCodeGenSchema.Providers.ColumnSchemaProviders {
    internal class InformationSchemaProvider : Provider {
        private readonly IDataTableFactory _dataTableFactory;

        public InformationSchemaProvider(IDataTableFactory dataTableFactory) {
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