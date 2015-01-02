using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class AssociationSchemaProvider {
        private abstract class Provider {
            public abstract IList<AssociationSchema> GetSchema(DbConnection connection);

            protected void AddAssociationListItems(List<AssociationSchema> list) {
                list.AddRange(list.Select(item => new AssociationSchema(item.AssociationName + "List",
                                                                      item.RelatedTableName,
                                                                      item.RelatedColumnName,
                                                                      item.TableName,
                                                                      item.ColumnName,
                                                                      AssociationType.List))
                                   .ToList());
            }
        }
    }
}
