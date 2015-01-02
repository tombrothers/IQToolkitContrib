using System.Data;
using System.Data.Common;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.TableSchemaProviders {
    internal class AccessProvider : Provider {
        public AccessProvider(ISchemaOptions schemaOptions)
            : base(schemaOptions) {
        }

        protected override DataTable GetDataTable(DbConnection connection) {
            var tables = base.GetDataTable(connection);

            tables.DefaultView.RowFilter = "Table_Type = 'Table'";

            if (!this.SchemaOptions.ExcludeViews) {
                tables.DefaultView.RowFilter += " or Table_Type = 'View' ";
            }

            return tables.DefaultView.ToTable(true, "Table_Name", "Table_Type");
        }
    }
}