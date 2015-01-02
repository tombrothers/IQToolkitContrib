namespace IQToolkitCodeGenSchema {
    public interface ISchemaOptions {
        string ConnectionString { get; }
        IDatabase Database { get; }
        bool ExcludeViews { get; }
        bool NoPluralization { get; }
    }
}
