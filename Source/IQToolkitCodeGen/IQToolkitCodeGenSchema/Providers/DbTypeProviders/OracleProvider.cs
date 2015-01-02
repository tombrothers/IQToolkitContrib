using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.DbTypeProviders {
    internal class OracleProvider : Provider {
        protected override string GetSingleParameterFormat(IColumnSchema columnSchema, IColumnTypeSchema columnTypeSchema) {
            if (columnTypeSchema.ColumnType == "TIMESTAMP") {
                return string.Format(columnTypeSchema.CreateFormat, columnSchema.Scale.Value);
            }

            return base.GetSingleParameterFormat(columnSchema, columnTypeSchema);
        }

        protected override object GetPrecision(IColumnSchema columnSchema) {
            if (!columnSchema.Precision.HasValue) {
                return "*";
            }

            return base.GetPrecision(columnSchema);
        }
    }
}