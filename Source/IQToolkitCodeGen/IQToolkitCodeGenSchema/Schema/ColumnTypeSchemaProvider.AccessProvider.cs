using System.Data;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class ColumnTypeSchemaProvider {
        private class AccessProvider : Provider {
            protected override string GetColumnType(DataRow row) {
                return row["NativeDataType"].ToString();
            }

            protected override string GetCreateFormatString(DataRow row) {
                return row.Field<string>("TypeName");
            }
        }
    }
}