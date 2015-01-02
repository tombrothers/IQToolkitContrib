using System.Diagnostics;

namespace IQToolkitCodeGenSchema.Models {
#if DEBUGGER_DISPLAY
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
#endif
    internal class PrimaryKeySchema : IPrimaryKeySchema {
        public string ColumnName { get; private set; }
        public bool AutoIncrement { get; private set; }

        public PrimaryKeySchema(string columnName, bool autoIncrement) {
            ArgumentUtility.CheckNotNullOrEmpty("columnName", columnName);

            this.ColumnName = columnName;
            this.AutoIncrement = autoIncrement;
        }

#if DEBUGGER_DISPLAY
        private string DebuggerDisplay() {
            return string.Format("ColumnName={0} | AutoIncrement={1}",
                this.ColumnName, this.AutoIncrement);
        }
#endif
    }
}