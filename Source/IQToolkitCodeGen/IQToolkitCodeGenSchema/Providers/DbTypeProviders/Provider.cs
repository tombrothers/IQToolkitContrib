using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.DbTypeProviders {
    internal class Provider : IProvider {
        public virtual string GetDbType(IColumnSchema columnSchema, IColumnTypeSchema columnTypeSchema, IPrimaryKeySchema primaryKeySchema) {
            var dbType = columnTypeSchema.CreateFormat;

            if (columnTypeSchema.CreateFormat.Contains("{0}")) {
                if (columnTypeSchema.CreateFormat.Contains("{1}")) {
                    dbType = string.Format(columnTypeSchema.CreateFormat, this.GetPrecision(columnSchema), columnSchema.Scale.Value);
                }
                else {
                    if (columnSchema.Precision.HasValue) {
                        dbType = string.Format(columnTypeSchema.CreateFormat, this.GetPrecision(columnSchema));
                    }
                    else {
                        dbType = string.Format(columnTypeSchema.CreateFormat, columnSchema.MaxLength.Value);
                    }
                }
            }

            return this.GetDbType(dbType, columnSchema, primaryKeySchema);
        }

        protected virtual string GetSingleParameterFormat(IColumnSchema columnSchema, IColumnTypeSchema columnTypeSchema) {
            if (columnSchema.Precision.HasValue) {
                return string.Format(columnTypeSchema.CreateFormat, this.GetPrecision(columnSchema));
            }
            else {
                return string.Format(columnTypeSchema.CreateFormat, columnSchema.MaxLength.Value);
            }
        }

        protected string GetDbType(string dbType, IColumnSchema columnSchema, IPrimaryKeySchema primaryKeySchema) {
            return dbType + this.GetPkIdentifier(primaryKeySchema) + this.GetNotNull(columnSchema);
        }

        protected virtual object GetPrecision(IColumnSchema columnSchema) {
            return columnSchema.Precision.Value;
        }

        protected virtual string GetPkIdentifier(IPrimaryKeySchema primaryKeySchema) {
            return string.Empty;
        }

        private string GetNotNull(IColumnSchema columnSchema) {
            if (!columnSchema.Nullable) {
                return " NOT NULL";
            }

            return string.Empty;
        }
    }
}