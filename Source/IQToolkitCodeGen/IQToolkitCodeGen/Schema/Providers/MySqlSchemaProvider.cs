using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Schema.Provider {
    [Export(typeof(ISchemaProvider))]
    internal class MySqlSchemaProvider : SchemaProviderBase {
        public MySqlSchemaProvider()
            : base(SchemaProviderName.MySql, SchemaProviderType.MySql) {
        }

        protected override string GetQuotedName(string name) {
            return name;
        }

        public override List<Association> GetAssociationList() {
            DataTable columns = null;

            this.Connection.DoConnected(() => {
                columns = this.Connection.GetSchema("Foreign Key Columns");
            });

            List<Association> associations = new List<Association>();

            associations.AddRange((from column in columns.AsEnumerable()
                                   select new Association {
                                       AssociationName = column.Field<string>("Constraint_Name"),
                                       TableName = column.Field<string>("Table_Name"),
                                       ColumnName = column.Field<string>("Column_Name"),
                                       RelatedTableName = column.Field<string>("Referenced_Table_Name"),
                                       RelatedColumnName = column.Field<string>("Referenced_Column_Name"),
                                       AssociationType = AssociationType.Item
                                   }).ToList());

            this.AddAssociationListItems(associations);

            return associations;
        }
    }
}
