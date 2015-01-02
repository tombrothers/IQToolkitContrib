namespace IQToolkitCodeGenSchema.Schema {
    internal partial class DbTypeProvider {
        private class VfpProvider : Provider {
            public override string GetDbType(ColumnSchema columnSchema, ColumnTypeSchema columnTypeSchema, PrimaryKeySchema primaryKeySchema) {
                if (columnTypeSchema.Type == typeof(string) && !columnSchema.MaxLength.HasValue) {
                    return this.GetDbType("M", columnSchema, primaryKeySchema);
                }

                return base.GetDbType(columnSchema, columnTypeSchema, primaryKeySchema);
            }

            protected override string GetPkIdentifier(PrimaryKeySchema primaryKeySchema) {
                if (primaryKeySchema == null || !primaryKeySchema.AutoIncrement) {
                    return string.Empty;
                }

                return " AUTOINC";
            }
        }
    }
}
