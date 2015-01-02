using System.Data;

namespace IQToolkitCodeGenSchema.Providers.ColumnTypeSchemaProviders {
    internal class AccessProvider : Provider {
        protected override string GetColumnType(DataRow row) {
            return row["NativeDataType"].ToString();
        }

        protected override string GetCreateFormatString(DataRow row) {
            return row.Field<string>("TypeName");
        }
    }
}