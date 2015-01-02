namespace IQToolkitCodeGenSchema.Models {
    public interface IDatabase {
        string DisplayName { get; }
        string ProviderName { get; }
        DatabaseType Type { get; }
        bool AllowCustomSchemaSql { get; }
    }
}