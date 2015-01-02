using System.Collections.Generic;
using System.Data.Common;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class PrimaryKeySchemaProvider {
        private interface IProvider {
            IList<PrimaryKeySchema> GetSchema(DbConnection connection, string tableName);
        }
    }
}
