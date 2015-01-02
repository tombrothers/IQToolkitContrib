using System.Diagnostics;

namespace IQToolkitCodeGenSchema.Schema {
#if DEBUGGER_DISPLAY
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
#endif
    internal class ColumnSchema {
        public string ColumnName { get; private set; }
        public string ColumnType { get; private set; }
        public bool Nullable { get; private set; }
        public long? MaxLength { get; private set; }
        public short? Precision { get; private set; }
        public short? Scale { get; private set; }

        public ColumnSchema(string columnName, string columnType, bool nullable, long? maxLength, short? precision, short? scale) {
            ArgumentUtility.CheckNotNullOrEmpty("columnName", columnName);
            ArgumentUtility.CheckNotNullOrEmpty("columnType", columnType);

            this.ColumnName = columnName;
            this.ColumnType = columnType;
            this.Nullable = nullable;
            this.MaxLength = maxLength;
            this.Precision = precision;
            this.Scale = scale;
        }

#if DEBUGGER_DISPLAY
        private string DebuggerDisplay() {
            return string.Format("ColumnName={0} | ColumnType={1} | Nullable={2} | MaxLength={3} | Precision={4} | Scale={5}",
                                    this.ColumnName, this.ColumnType, this.Nullable, this.MaxLength, this.Precision, this.Scale);
        }
#endif
    }
}
