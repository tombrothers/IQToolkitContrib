using System.Data;
using System.Data.Common;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class TableSchemaProvider {
        private class SQLiteProvider : Provider {
            public SQLiteProvider(IUnityContainer container)
                : base(container) {
            }

            protected override DataTable GetDataTable(DbConnection connection) {
                var tables = this.GetTables(connection);

                if (!this._container.ExcludeViews()) {
                    var views = this.GetViews(connection);
                    tables.Merge(views);
                }

                return tables;
            }

            private DataTable GetViews(DbConnection connection) {
                DataTable views = null;

                connection.DoConnected(() => {
                    views = connection.GetSchema("Views");
                });

                views = views.DefaultView.ToTable(true, "Table_Name");
                views.Columns.Add("TABLE_TYPE");
                views.AsEnumerable().ForEach(row => row["Table_Type"] = "VIEW");

                return views;
            }

            private DataTable GetTables(DbConnection connection) {
                var tables = base.GetDataTable(connection);

                tables.DefaultView.RowFilter = "Table_Type ='table'";

                return tables.DefaultView.ToTable(true, "Table_Name", "Table_Type");
            }
        }
    }
}
