namespace IQToolkitCodeGenSchema.Models {
    public interface ISchemaOptions {
        string ConnectionString { get; }
        IDatabase Database { get; }
        bool ExcludeViews { get; }
        bool NoPluralization { get; }
        string TableSchemaSql { get; }
        string ColumnSchemaSql { get; }
        string AssociationSchemaSql { get; }
    }
}