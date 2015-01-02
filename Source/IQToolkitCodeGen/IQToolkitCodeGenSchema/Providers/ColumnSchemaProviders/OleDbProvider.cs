using System.Data;

namespace IQToolkitCodeGenSchema.Providers.ColumnSchemaProviders {
    internal class OleDbProvider : Provider {
        protected override string GetColumnType(DataRow row) {
            return row.Field<int>("Data_Type").ToString();
        }
    }
}