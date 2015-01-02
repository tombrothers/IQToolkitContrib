using System.Data;
using System.Data.Common;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class ColumnSchemaProvider {
        private class OracleProvider : OleDbProvider {
            protected override DataTable GetDataTable(DbConnection connection, string tableName) {
                DataTable columns = null;

                connection.DoConnected(() => {
                    //TODO: Get User Id from the connection string and use it for the owner.
                    string userId = "NORTHWIND";

                    columns = connection.GetSchema("Columns", new[] { userId, tableName });
                });

                // Changed column names to match what the base provider is expecting.
                columns.Columns["DataType"].ColumnName = "Data_Type";
                columns.Columns["DataType"].ColumnName = "Nullable";
                columns.Columns["Length"].ColumnName = "CHARACTER_MAXIMUM_LENGTH";
                columns.Columns["PRECISION"].ColumnName = "NUMERIC_PRECISION";
                columns.Columns["SCALE"].ColumnName = "NUMERIC_SCALE";

                return columns;
            }
        }
    }
}
