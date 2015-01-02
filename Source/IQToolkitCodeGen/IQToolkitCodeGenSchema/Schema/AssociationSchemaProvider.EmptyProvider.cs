using System.Collections.Generic;
using System.Data.Common;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class AssociationSchemaProvider {
        private class EmptyProvider : Provider {
            public override IList<AssociationSchema> GetSchema(DbConnection connection) {
                return new List<AssociationSchema>();
            }
        }
    }
}