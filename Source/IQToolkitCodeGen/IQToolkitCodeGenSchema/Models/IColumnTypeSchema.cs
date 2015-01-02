using System;

namespace IQToolkitCodeGenSchema.Models {
    internal interface IColumnTypeSchema {
        string ColumnType { get; }
        int ProviderDbType { get; }
        string CreateFormat { get; }
        Type Type { get; }
    }
}