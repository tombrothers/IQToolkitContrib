using System.Data;
using System.Data.Common;

namespace IQToolkitCodeGenSchema.Providers.ColumnSchemaProviders {
    internal class OracleProvider : Provider {
        private readonly IOracleUserProvider _oracleUserProvider;

        public OracleProvider(IOracleUserProvider oracleUserProvider) {
            ArgumentUtility.CheckNotNull("oracleUserProvider", oracleUserProvider);

            this._oracleUserProvider = oracleUserProvider;
        }

        protected override DataTable GetDataTable(DbConnection connection, string tableName) {
            DataTable columns = null;

            connection.DoConnected(() => {
                columns = connection.GetSchema("Columns", new[] { this._oracleUserProvider.User, tableName });
            });

            // Changed column names to match what the base provider is expecting.
            columns.Columns["DataType"].ColumnName = "Data_Type";
            columns.Columns["Length"].ColumnName = "CHARACTER_MAXIMUM_LENGTH";
            columns.Columns["PRECISION"].ColumnName = "NUMERIC_PRECISION";
            columns.Columns["SCALE"].ColumnName = "NUMERIC_SCALE";

            return columns;
        }
    }
}