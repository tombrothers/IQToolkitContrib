using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.DbTypeProviders {
    internal class VfpProvider : Provider {
        public override string GetDbType(IColumnSchema columnSchema, IColumnTypeSchema columnTypeSchema, IPrimaryKeySchema primaryKeySchema) {
            if (columnTypeSchema.Type == typeof(string) && !columnSchema.MaxLength.HasValue) {
                return this.GetDbType("M", columnSchema, primaryKeySchema);
            }

            return base.GetDbType(columnSchema, columnTypeSchema, primaryKeySchema);
        }

        protected override string GetPkIdentifier(IPrimaryKeySchema primaryKeySchema) {
            if (primaryKeySchema == null || !primaryKeySchema.AutoIncrement) {
                return string.Empty;
            }

            return " AUTOINC";
        }
    }
}