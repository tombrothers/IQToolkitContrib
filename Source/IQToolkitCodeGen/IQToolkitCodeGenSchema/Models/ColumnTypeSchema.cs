using System;
using System.Diagnostics;

namespace IQToolkitCodeGenSchema.Models {
#if DEBUGGER_DISPLAY
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
#endif
    internal class ColumnTypeSchema : IColumnTypeSchema {
        public string ColumnType { get; private set; }
        public int ProviderDbType { get; private set; }
        public string CreateFormat { get; private set; }
        public Type Type { get; private set; }

        public ColumnTypeSchema(string columnType, int providerDbType, string createFormat, Type type) {
            ArgumentUtility.CheckNotNullOrEmpty("columnType", columnType);
            ArgumentUtility.CheckNotNull("createFormat", createFormat);
            ArgumentUtility.CheckNotNull("type", type);

            this.ColumnType = columnType;
            this.ProviderDbType = providerDbType;
            this.CreateFormat = createFormat;
            this.Type = type;
        }

#if DEBUGGER_DISPLAY
        private string DebuggerDisplay() {
            return string.Format("ColumnType={0} | ProviderDbType={1} | CreateFormat={2} | Type={3}",
                this.ColumnType, this.ProviderDbType, this.CreateFormat, this.Type);
        }
#endif
    }
}