using System.Diagnostics;
using System.Linq;

namespace IQToolkitCodeGenSchema.Models {
#if DEBUGGER_DISPLAY
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
#endif
    internal class Column : IColumn, IPropertyName {
        public string ColumnName { get; private set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; internal set; }
        public bool PrimaryKey { get; internal set; }
        public bool Generated { get; internal set; }
        public long? MaxLength { get; private set; }
        public short? Precision { get; private set; }
        public short? Scale { get; private set; }
        public string DbType { get; internal set; }
        public bool Nullable { get; private set; }
        public string DefaultValue { get; private set; }

        public Column(IColumnSchema schema) {
            ArgumentUtility.CheckNotNull("schema", schema);

            this.ColumnName = schema.ColumnName;
            this.PropertyName = schema.ColumnName.ToSafeClrName(schema.ColumnName.ShouldForceProperCase());
            this.MaxLength = schema.MaxLength;
            this.Precision = schema.Precision;
            this.Scale = schema.Scale;
            this.Nullable = schema.Nullable;
            this.DefaultValue = schema.DefaultValue;
        }

#if DEBUGGER_DISPLAY
        private string DebuggerDisplay() {
            return string.Format("ColumnName={0} | PropertyName={1} | PropertyType={2} | PrimaryKey={3} | Generated={4} | MaxLength={5} | Precision={6} | Scale={7} | DbType={8} | Nullable={9} | DefaultValue={10}",
                this.ColumnName, this.PropertyName, this.PropertyType, this.PrimaryKey, this.Generated, this.MaxLength, this.Precision, this.Scale, this.DbType, this.Nullable, this.DefaultValue);
        }
#endif
    }
}