using System.Collections.Generic;
using System.Diagnostics;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Models {
#if DEBUGGER_DISPLAY
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
#endif
    internal class TableSchema : ITableSchema {
        public string TableName { get; private set; }
        public bool IsView { get; private set; }
        public IList<IColumnSchema> Columns { get; set; }
        public IList<IPrimaryKeySchema> PrimaryKeys { get; set; }
        public IList<IAssociationSchema> Associations { get; set; }

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