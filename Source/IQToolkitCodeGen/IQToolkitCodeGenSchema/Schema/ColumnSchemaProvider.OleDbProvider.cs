using System.Data;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class ColumnSchemaProvider {
        private class OleDbProvider : Provider {
            protected override string GetColumnType(DataRow row) {
                return row.Field<int>("Data_Type").ToString();
            }
        }
    }
}
