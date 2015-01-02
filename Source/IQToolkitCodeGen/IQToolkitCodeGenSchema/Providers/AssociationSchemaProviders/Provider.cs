using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.AssociationSchemaProviders {
    internal abstract class Provider : IProvider {
        public abstract IList<IAssociationSchema> GetSchema(DbConnection connection);

        protected void AddAssociationListItems(List<IAssociationSchema> list) {
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