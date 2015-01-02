using System;

namespace IQToolkitCodeGenSchema.Models {
    internal interface IColumnSchema {
        string ColumnName { get; }
        string ColumnType { get; }
        string DefaultValue { get; }
        long? MaxLength { get; }
        bool Nullable { get; }
        short? Precision { get; }
        short? Scale { get; }
    }
}