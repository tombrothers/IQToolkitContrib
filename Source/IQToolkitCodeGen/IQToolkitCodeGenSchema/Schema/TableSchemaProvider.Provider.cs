using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class TableSchemaProvider {
        private class Provider {
            protected readonly IUnityContainer _container;

            public Provider(IUnityContainer container) {
                ArgumentUtility.CheckNotNull("container", container);

                this._container = container;
            }

            public IList<TableSchema> GetSchema(DbConnection connection) {
                var tables = this.GetDataTable(connection);

                return tables.AsEnumerable()
                             .Select(row => new TableSchema(this.GetTableName(row), this.GetIsView(row)))
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

                if (this._container.ExcludeViews()) {
                    tables.DefaultView.RowFilter = "Table_Type <> 'View'";
                    return tables.DefaultView.ToTable();
                }
                else {
                    return tables;
                }
            }
        }
    }
}
