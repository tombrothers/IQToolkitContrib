namespace IQToolkitCodeGenSchema {
    public interface IColumn {
        string ColumnName { get; }
        string PropertyName { get; }
        string PropertyType { get; }
        bool PrimaryKey { get; }
        bool Generated { get; }
        long? MaxLength { get; }
        short? Precision { get; }
        short? Scale { get; }
        string DbType { get; }
        bool Nullable { get; }
    }
}
