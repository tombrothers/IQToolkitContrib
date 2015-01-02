using System.Collections.Generic;
using System.Diagnostics;

namespace IQToolkitCodeGenSchema.Schema {
#if DEBUGGER_DISPLAY
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
#endif
    internal class TableSchema {
        public string TableName { get; private set; }
        public bool IsView { get; private set; }
        public IList<ColumnSchema> Columns { get; internal set; }
        public IList<PrimaryKeySchema> PrimaryKeys { get; internal set; }
        public IList<AssociationSchema> Associations { get; internal set; }

        public TableSchema(string tableName, bool isView) {
            ArgumentUtility.CheckNotNullOrEmpty("tableName", tableName);

            this.TableName = tableName;
            this.IsView = isView;
        }

#if DEBUGGER_DISPLAY
        private string DebuggerDisplay() {
            return string.Format("TableName={0} | IsView={1}", this.TableName, this.IsView);
        }
#endif
    }
}
