using System.Collections.Generic;
using System.Data.Common;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.PrimaryKeySchemaProviders {
    internal interface IProvider {
        IList<IPrimaryKeySchema> GetSchema(DbConnection connection, string tableName);
    }
}
