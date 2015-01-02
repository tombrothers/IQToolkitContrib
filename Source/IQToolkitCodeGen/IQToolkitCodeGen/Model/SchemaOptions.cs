using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGen.Model {
    public class SchemaOptions : ISchemaOptions {
        public IDatabase Database { get; set; }
        public string ConnectionString { get; set; }
        public bool NoPluralization { get; set; }
        public bool ExcludeViews { get; set; }
        public string TableSchemaSql { get; set; }
        public string ColumnSchemaSql { get; set; }
        public string AssociationSchemaSql { get; set; }
    }
}
