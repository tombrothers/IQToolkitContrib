using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Schema.Provider {
    [Export(typeof(ISchemaProvider))]
    internal partial class VfpSchemaProvider : OleDbSchemaProviderBase {
        public bool IsDbc { get; private set; }

        public VfpSchemaProvider()
            : base(SchemaProviderName.Vfp) {
        }

        public override List<PrimaryKey> GetPrimaryKeyList(string tableName) {
            List<PrimaryKey> primaryKeys = new List<PrimaryKey>();

            if (string.IsNullOrEmpty(tableName)) {
                return primaryKeys;
            }

            DataTable indexes = null;

            this.Connection.DoConnected(() => {
                indexes = this.Connection.GetSchema("Indexes");
            });

            indexes.DefaultView.RowFilter = "Table_Name = '" + tableName + "'";
            indexes = indexes.DefaultView.ToTable();

            foreach (DataRow row in indexes.Rows) {
                if (Convert.ToBoolean(row["PRIMARY_KEY"])) {
                    primaryKeys.Add(new PrimaryKey {
                        ColumnName = row["COLUMN_NAME"].ToString(),
                        AutoIncrement = this.IsDbc && this.IsAutoIncrement(row)
                    });
                }
            }

            return primaryKeys;
        }

        private bool IsAutoIncrement(DataRow row) {
            bool isAutoInc = false;
            const int FIELDINDEX_STEP_FOR_AUTOINCREMENTING = 18;

            this.Connection.DoConnected(() => {
                using (DbCommand cmd = this.Connection.CreateCommand()) {
                    string tableName = row["Table_Name"].ToString();

                    //  using a select statement to open the table because the the use statement didn't work
                    cmd.CommandText = string.Format("SELECT * FROM {0} WHERE .F.", tableName);
                    cmd.ExecuteNonQuery();

                    // get the field information
                    cmd.CommandText = string.Format("AFIELDS(laFields, '{0}')", tableName);
                    cmd.ExecuteScalar();

                    // using a arbitrary select statement to get return array value
                    cmd.CommandText = string.Format("SELECT TOP 1 laFields[{0}] from (DBC()) ORDER BY ObjectId", FIELDINDEX_STEP_FOR_AUTOINCREMENTING);
                    isAutoInc = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            });

            return isAutoInc;
        }

        public override List<ColumnMetaInfo> GetColumnMetaInfo() {
            DataTable columns = null;
            DataTable dataTypes = null;

            this.Connection.DoConnected(() => {
                dataTypes = this.Connection.GetSchema("DataTypes");

                List<string> tableNames = this.GetTableNames();

                const int TABLE_LIMIT = 100; // arbitrary number
                if (tableNames.Count > TABLE_LIMIT) {
                    // Getting the columns for each table instead of getting all columns at once due to problems with large database containers.
                    foreach (string tableName in tableNames) {
                        DataTable dt = this.Connection.GetSchema("Columns", new string[] { null, null, tableName });

                        if (columns == null) {
                            columns = dt;
                        }
                        else {
                            columns.Merge(dt);
                        }
                    }
                }
                else {
                    columns = this.Connection.GetSchema("Columns");
                }
            });

            dataTypes = this.RemoveDuplicateDataTypes(dataTypes);

            return this.GetColumnMetaInfo(columns, dataTypes);
        }

        private DataTable RemoveDuplicateDataTypes(DataTable dataTypes) {
            DataTable results = dataTypes.Clone();
            List<int> values = new List<int>();

            foreach (DataRow row in dataTypes.Rows) {
                int dataTypeValue = row.Field<int>("ProviderDbType");

                if (!values.Contains(dataTypeValue)) {
                    results.Rows.Add(row.ItemArray);
                    values.Add(dataTypeValue);
                }
            }

            return results;
        }

        public override long GetMaxLength(System.Data.DataRow dr) {
            long maxLength = base.GetMaxLength(dr);
            int MAX_VFP_CHAR_SIZE = 254;

            // don't care about the length of memo fields
            if (maxLength > MAX_VFP_CHAR_SIZE) {
                return -1;
            }

            return maxLength;
        }

        public override void SetConnectionString(string connectionString) {
            if (string.IsNullOrEmpty(connectionString)) {
                throw new ArgumentException("Invalid connection string.");
            }

            // Assumes that if the connectionString has an equals sign it is considered to be a complete connection string.  Otherwise, it 
            // is either a path to a dbc or a path to a directory that contains dbfs.
            if (!connectionString.Contains("=")) {
                if (this.IsValidDataPath(connectionString)) {
                    connectionString = string.Format("Provider=VFPOLEDB;Data Source={0}", connectionString);
                }
                else {
                    throw new ApplicationException("Invalid connection string.");
                }
            }

            base.SetConnectionString(connectionString);
            OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder(this.Connection.ConnectionString);
            this.IsDbc = Path.GetExtension(builder.DataSource).Equals(".dbc", StringComparison.CurrentCultureIgnoreCase);
        }

        private bool IsValidDataPath(string path) {
            return (Path.GetExtension(path).ToLower() == ".dbc" && File.Exists(path)) || Directory.Exists(path);
        }
    }
}
