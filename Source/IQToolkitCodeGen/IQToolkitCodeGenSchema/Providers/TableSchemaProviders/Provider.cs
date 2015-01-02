using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.TableSchemaProviders {
    internal class Provider : IProvider {
        protected readonly ISchemaOptions SchemaOptions;

        public Provider(ISchemaOptions schemaOptions) {
            ArgumentUtility.CheckNotNull("schemaOptions", schemaOptions);

            this.SchemaOptions = schemaOptions;
        }

        public IList<ITableSchema> GetSchema(DbConnection connection) {
            var tables = this.GetDataTable(connection);

            return tables.AsEnumerable()
                         .Select(row => new TableSchema(this.GetTableName(row), this.GetIsView(row)))
                         .Cast<ITableSchema>()
                         .ToList();
        }

        private bool GetIsView(DataRow row) {
            return row.Field<string>("Table_Type").ToUpper() == "VIEW";
        }

        private string GetTableName(DataRow row) {
            return row.Field<string>("Table_Name");
        }

        protected virtual DataTable GetDataTable(DbConnection connection) {
            DataTable tables = null;

            connection.DoConnected(() => {
                tables = connection.GetSchema("Tables");
            });

            if (this.SchemaOptions.ExcludeViews) {
                tables.DefaultView.RowFilter = "Table_Type <> 'View'";

                return tables.DefaultView.ToTable();
            }
            else {
                return tables;
            }
        }
    }
}
