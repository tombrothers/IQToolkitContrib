using System.Collections.Generic;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers;

namespace IQToolkitCodeGenSchema {
    public static class Schema {
        public static IEnumerable<ITable> GetSchema(ISchemaOptions schemaOptions) {
            var provider = SchemaProvider.Create(schemaOptions);

            return provider.GetSchema();
        }
    }
}
