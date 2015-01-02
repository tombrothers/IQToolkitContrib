using System.Collections.Generic;
using System.Data.Common;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.ColumnTypeSchemaProviders {
    internal interface IProvider {
        IList<IColumnTypeSchema> GetSchema(DbConnection connection);
    }
}