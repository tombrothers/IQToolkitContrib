using System.Collections.Generic;
using System.Data.Common;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.AssociationSchemaProviders {
    internal interface IProvider {
        IList<IAssociationSchema> GetSchema(DbConnection connection);
    }
}