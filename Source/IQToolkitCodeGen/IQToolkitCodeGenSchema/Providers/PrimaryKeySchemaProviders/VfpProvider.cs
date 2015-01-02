using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.PrimaryKeySchemaProviders {
    internal class VfpProvider : IProvider {
        private readonly IDataTableFactory _dataTableFactory;

        public VfpProvider(IDataTableFactory dataTableFactory) {
            ArgumentUtility.CheckNotNull("dataTableFactory", dataTableFactory);

            this._dataTableFactory = dataTableFactory;
        }

        public IList<IPrimaryKeySchema> GetSchema(DbConnection connection, string tableName) {
            ArgumentUtility.CheckNotNull("connection", connection);
            ArgumentUtility.CheckNotNullOrEmpty("tableName", tableName);

            var primaryKeys = new List<IPrimaryKeySchema>();
            var indexes = this.GetDataTable(connection, tableName);
            var isDbc = this.IsDbc(connection);
            indexes.AsEnumerable()
                   .Where(row => Convert.ToBoolean(row["PRIMARY_KEY"]))
                   .ForEach(row => primaryKeys.Add(new PrimaryKeySchema(row["COLUMN_NAME"].ToString(),
                                   isDbc && this.IsAutoIncrement(connection, row))));

            return primaryKeys;
        }

        private bool IsAutoIncrement(DbConnection connection, DataRow row) {
            bool isAutoInc = false;
            const int FIELDINDEX_STEP_FOR_AUTOINCREMENTING = 18;

            connection.DoConnected(() => {
                using (var cmd = connection.CreateCommand()) {
                    string tableName = row["Table_Name"].ToString();

                    //  using a select statement to open the table because the the use statement didn't work
                    cmd.CommandText = string.Format("SELECT * FROM {0} WHERE .F.", tableName);
                    cmd.ExecuteNonQuery();

                    // get the field information
                    cmd.CommandText = string.Format("AFIELDS(laFields, '{0}')", tableName);
                    cmd.ExecuteScalar();

                    // using a arbitrary select statement to get return array value
                    cmd.CommandText = string.Format("SELECT TOP 1 laFields[{0}, {1}] from (DBC()) ORDER BY ObjectId",
                                                    row["Ordinal_Position"],
                                                    FIELDINDEX_STEP_FOR_AUTOINCREMENTING);

                    isAutoInc = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            });

            return isAutoInc;
        }

        private DataTable GetDataTable(DbConnection connection, string tableName) {
            DataTable indexes = null;

            connection.DoConnected(() => {
                indexes = connection.GetSchema("Indexes");
            });

            indexes.DefaultView.RowFilter = "Table_Name = '" + tableName + "'";

            return indexes.DefaultView.ToTable();
        }

        private bool IsDbc(DbConnection connection) {
            OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder(connection.ConnectionString);
            return Path.GetExtension(builder.DataSource).Equals(".dbc", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}