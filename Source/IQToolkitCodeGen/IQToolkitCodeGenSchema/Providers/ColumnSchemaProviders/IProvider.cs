using System.Collections.Generic;
using System.Data.Common;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.ColumnSchemaProviders {
    internal interface IProvider {
        IList<IColumnSchema> GetSchema(DbConnection connection, string tableName);
    }
}
