using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Schema.Provider {
    [Export(typeof(ISchemaProvider))]
    internal class SQLiteSchemaProvider : SchemaProviderBase {
        public SQLiteSchemaProvider()
            : base(SchemaProviderName.SQLite, SchemaProviderType.SQLite) {
        }

        public override List<Association> GetAssociationList() {
            return new List<Association>();
        }

        protected override DataTable GetTablesSchema() {
            DataTable tables = base.GetTablesSchema();
            DataTable views = null;

            this.Connection.DoConnected(() => {
                views = this.Connection.GetSchema("Views");
            });

            tables.Merge(views);

            return tables;
        }

        public override void SetConnectionString(string connectionString) {
            if (string.IsNullOrEmpty(connectionString)) {
                throw new ArgumentException("Invalid connection string.");
            }

            // Assumes that if the connectionString has an equals sign it is considered to be a complete connection string.  Otherwise, it 
            // is a path to a sl3 file.
            if (!connectionString.Contains("=")) {
                if (this.IsValidDataPath(connectionString)) {
                    connectionString = string.Format("Version=3;Data Source={0}", connectionString);
                }
                else {
                    throw new ApplicationException("Invalid connection string.");
                }
            }

            base.SetConnectionString(connectionString);
        }

        private bool IsValidDataPath(string path) {
            return (Path.GetExtension(path).ToLower() == ".sl3" && File.Exists(path));
        }
    }
}
