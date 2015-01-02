namespace IQToolkitCodeGenSchema.Schema {
    internal partial class DbTypeProvider {
        private class Provider {
            public virtual string GetDbType(ColumnSchema columnSchema, ColumnTypeSchema columnTypeSchema, PrimaryKeySchema primaryKeySchema) {
                var dbType = columnTypeSchema.CreateFormat;

                if (columnTypeSchema.CreateFormat.Contains("{0}")) {
                    if (columnTypeSchema.CreateFormat.Contains("{1}")) {
                        dbType = string.Format(columnTypeSchema.CreateFormat, columnSchema.Precision.Value, columnSchema.Scale.Value);
                    }
                    else {
                        if (columnSchema.Precision.HasValue) {
                            dbType = string.Format(columnTypeSchema.CreateFormat, columnSchema.Precision.Value);
                        }
                        else {
                            dbType = string.Format(columnTypeSchema.CreateFormat, columnSchema.MaxLength.Value);
                        }
                    }
                }

                return this.GetDbType(dbType, columnSchema, primaryKeySchema);
            }

            protected string GetDbType(string dbType, ColumnSchema columnSchema, PrimaryKeySchema primaryKeySchema) {
                return dbType + this.GetPkIdentifier(primaryKeySchema) + this.GetNotNull(columnSchema);
            }

            protected virtual string GetPkIdentifier(PrimaryKeySchema primaryKeySchema) {
                return string.Empty;
            }

            private string GetNotNull(ColumnSchema columnSchema) {
                if (!columnSchema.Nullable) {
                    return " NOT NULL";
                }

                return string.Empty;
            }
        }
    }
}
