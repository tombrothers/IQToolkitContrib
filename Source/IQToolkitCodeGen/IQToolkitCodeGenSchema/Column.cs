using System.Diagnostics;
using IQToolkitCodeGenSchema.Schema;

namespace IQToolkitCodeGenSchema {
#if DEBUGGER_DISPLAY
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
#endif
    internal class Column : IColumn, IPropertyName {
        public string ColumnName { get; private set; }
        public string PropertyName { get; private set; }
        public string PropertyType { get; internal set; }
        public bool PrimaryKey { get; internal set; }
        public bool Generated { get; internal set; }
        public long? MaxLength { get; private set; }
        public short? Precision { get; private set; }
        public short? Scale { get; private set; }
        public string DbType { get; internal set; }
        public bool Nullable { get; private set; }

        string IPropertyName.PropertyName {
            get { return this.PropertyName; }
            set { this.PropertyName = value; }
        }

        public Column(ColumnSchema schema) {
            ArgumentUtility.CheckNotNull("schema", schema);

            this.ColumnName = schema.ColumnName;
            this.PropertyName = schema.ColumnName.ToSafeClrName();
            this.MaxLength = schema.MaxLength;
            this.Precision = schema.Precision;
            this.Scale = schema.Scale;
            this.Nullable = schema.Nullable;
        }

#if DEBUGGER_DISPLAY
        private string DebuggerDisplay() {
            return string.Format("ColumnName={0} | PropertyName={1} | PropertyType={2} | PrimaryKey={3} | Generated={4} | MaxLength={5} | Precision={6} | Scale={7} | DbType={8} | Nullable={9}",
                this.ColumnName, this.PropertyName, this.PropertyType, this.PrimaryKey, this.Generated, this.MaxLength, this.Precision, this.Scale, this.DbType, this.Nullable);
        }
#endif
    }
}
