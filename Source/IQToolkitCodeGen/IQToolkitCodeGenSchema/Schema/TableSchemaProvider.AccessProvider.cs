using System.Data;
using System.Data.Common;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class TableSchemaProvider {
        private class AccessProvider : Provider {
            public AccessProvider(IUnityContainer container)
                : base(container) {
            }

            protected override DataTable GetDataTable(DbConnection connection) {
                var tables = base.GetDataTable(connection);

                tables.DefaultView.RowFilter = "Table_Type = 'Table'";

                if (!this._container.ExcludeViews()) {
                    tables.DefaultView.RowFilter += " or Table_Type = 'View' ";
                }

                return tables.DefaultView.ToTable(true, "Table_Name", "Table_Type");
            }
        }
    }
}
