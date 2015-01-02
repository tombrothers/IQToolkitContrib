using System.Collections.Generic;
using System.Data.Common;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.AssociationSchemaProviders {
    internal class EmptyProvider : Provider {
        public override IList<IAssociationSchema> GetSchema(DbConnection connection) {
            return new List<IAssociationSchema>();
        }
    }
}