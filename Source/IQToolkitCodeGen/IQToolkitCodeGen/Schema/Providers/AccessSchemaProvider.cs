using System;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;

namespace IQToolkitCodeGen.Schema.Provider {
    [Export(typeof(ISchemaProvider))]
    internal class AccessSchemaProvider : OleDbSchemaProviderBase {
        public AccessSchemaProvider()
            : base(SchemaProviderName.Access) {
        }

        protected override DataTable GetTablesSchema() {
            DataTable tables = base.GetTablesSchema();
            tables.DefaultView.RowFilter = "Table_Type = 'Table' or Table_Type = 'View' ";
            return tables.DefaultView.ToTable();
        }

        public override void SetConnectionString(string connectionString) {
            if (string.IsNullOrEmpty(connectionString)) {
                throw new ArgumentException("Invalid connection string.");
            }

            // Assumes that if the connectionString has an equals sign it is considered to be a complete connection string.  Otherwise, it 
            // is a path to a mdb file.
            if (!connectionString.Contains("=")) {
                if (this.IsValidDataPath(connectionString)) {
                    connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", connectionString);
                }
                else {
                    throw new ApplicationException("Invalid connection string.");
                }
            }

            base.SetConnectionString(connectionString);
        }

        private bool IsValidDataPath(string path) {
            return (Path.GetExtension(path).ToLower() == ".mdb" && File.Exists(path));
        }
    }
}

