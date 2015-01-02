using System.Collections.Generic;
using System.Data.Common;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.TableSchemaProviders {
    internal interface IProvider {
        IList<ITableSchema> GetSchema(DbConnection connection);
    }
}