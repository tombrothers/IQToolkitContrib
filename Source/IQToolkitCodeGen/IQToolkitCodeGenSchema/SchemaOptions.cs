namespace IQToolkitCodeGenSchema {
    public class SchemaOptions : ISchemaOptions {
        public IDatabase Database { get; private set; }
        public string ConnectionString { get; internal set; }
        public bool NoPluralization { get; private set; }
        public bool ExcludeViews { get; private set; }

        public SchemaOptions(IDatabase database, string connectionString, bool excludeViews = false, bool noPluralization = false) {
            this.Database = database;
            this.ConnectionString = connectionString;
            this.ExcludeViews = excludeViews;
            this.NoPluralization = noPluralization;
        }
    }
}