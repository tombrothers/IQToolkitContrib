using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class AssociationSchemaProvider {
        private class MySqlProvider : Provider {
            public override IList<AssociationSchema> GetSchema(DbConnection connection) {
                DataTable columns = null;

                connection.DoConnected(() => {
                    columns = connection.GetSchema("Foreign Key Columns");
                });

                var associations = new List<AssociationSchema>();

                associations.AddRange(columns.AsEnumerable()
                                             .Select(column => new AssociationSchema(column.Field<string>("Constraint_Name"),
                                                                                     column.Field<string>("Table_Name"),
                                                                                     column.Field<string>("Column_Name"),
                                                                                     column.Field<string>("Referenced_Table_Name"),
                                                                                     column.Field<string>("Referenced_Column_Name"),
                                                                                     AssociationType.Item)));

                this.AddAssociationListItems(associations);

                return associations;
            }
        }
    }
}
