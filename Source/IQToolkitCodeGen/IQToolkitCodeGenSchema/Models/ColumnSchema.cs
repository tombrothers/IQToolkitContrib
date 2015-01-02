using System.Diagnostics;

namespace IQToolkitCodeGenSchema.Models {
#if DEBUGGER_DISPLAY
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
#endif
    internal class ColumnSchema : IColumnSchema {
        public string ColumnName { get; private set; }
        public string ColumnType { get; private set; }
        public string DefaultValue { get; private set; }
        public bool Nullable { get; private set; }
        public long? MaxLength { get; private set; }
        public short? Precision { get; private set; }
        public short? Scale { get; private set; }

        public ColumnSchema(string columnName, string columnType, string defaultValue, bool nullable, long? maxLength, short? precision, short? scale) {
            ArgumentUtility.CheckNotNullOrEmpty("columnName", columnName);
            ArgumentUtility.CheckNotNullOrEmpty("columnType", columnType);

            this.ColumnName = columnName;
            this.ColumnType = columnType;
            this.DefaultValue = defaultValue;
            this.Nullable = nullable;
            this.MaxLength = maxLength;
            this.Precision = precision;
            this.Scale = scale;
        }

#if DEBUGGER_DISPLAY
        private string DebuggerDisplay() {
            return string.Format("ColumnName={0} | ColumnType={1} | DefaultValue={2} | Nullable={3} | MaxLength={4} | Precision={5} | Scale={6}",
                                    this.ColumnName, this.ColumnType, this.DefaultValue, this.Nullable, this.MaxLength, this.Precision, this.Scale);
        }
#endif
    }
}
