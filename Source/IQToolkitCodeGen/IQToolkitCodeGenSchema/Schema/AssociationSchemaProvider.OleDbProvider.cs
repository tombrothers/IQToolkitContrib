using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class AssociationSchemaProvider {
        private class OleDbProvider : Provider {
            public override IList<AssociationSchema> GetSchema(DbConnection connection) {
                var associations = new List<AssociationSchema>();

                DataTable foreignKeys = null;

                connection.DoConnected(() => {
                    foreignKeys = ((OleDbConnection)connection).GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, null);
                });

                associations.AddRange(foreignKeys.AsEnumerable()
                                                 .Select(row => new AssociationSchema(string.Format("FK_{0}_{1}",
                                                                                                row.Field<string>("FK_TABLE_NAME"),
                                                                                                row.Field<string>("PK_TABLE_NAME")),
                                                                                    row.Field<string>("FK_TABLE_NAME"),
                                                                                    row.Field<string>("FK_COLUMN_NAME"),
                                                                                    row.Field<string>("PK_TABLE_NAME"),
                                                                                    row.Field<string>("PK_COLUMN_NAME"),
                                                                                    AssociationType.Item)));

                var associations2 = new List<AssociationSchema>();
                associations2.AddRange(associations);
                this.AddAssociationListItems(associations2);
                associations2.RemoveRange(0, associations.Count);
                associations2.ForEach(x => x.AssociationName = string.Format("FK_{0}_{1}List",
                                                                             x.TableName,
                                                                             x.RelatedTableName));

                associations.AddRange(associations2);
                return associations;
            }
        }
    }
}
