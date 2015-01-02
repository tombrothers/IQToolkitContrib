namespace IQToolkitCodeGenSchema.Models {
    public interface IPrimaryKeySchema {
        string ColumnName { get; }
        bool AutoIncrement { get; }
    }
}