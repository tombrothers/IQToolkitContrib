using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Schema.Provider {
    [Export(typeof(ISchemaProvider))]
    internal class SqlServerSchemaProvider : SchemaProviderBase {
        public SqlServerSchemaProvider()
            : base(SchemaProviderName.SqlServer, SchemaProviderType.SqlServer) {
        }

        public override List<Association> GetAssociationList() {
            string sql = @"
select object_name(f.constid) ForeignKey,
		t.TABLE_NAME TableName,
		sc.name ColumnName,
		rt.TABLE_NAME RelatedTableName,
		r.name RelatedColumnName
	from sysforeignkeys f
	inner join syscolumns sc on f.fkeyid = sc.id 
		and f.fkey = sc.colid
	inner join syscolumns r on f.rkeyid = r.id 
		and f.rkey = r.colid
	inner join INFORMATION_SCHEMA.TABLES t on object_name(f.fkeyid) = t.Table_Name
	inner join INFORMATION_SCHEMA.TABLES rt on object_name(f.rkeyid) = rt.Table_Name";

            DataTable dataTable = this.GetDataTable(sql);
            List<Association> associations = new List<Association>();

            associations.AddRange((from row in dataTable.AsEnumerable()
                                   select new Association {
                                       AssociationName = row.Field<string>("ForeignKey"),
                                       TableName = row.Field<string>("TableName"),
                                       ColumnName = row.Field<string>("ColumnName"),
                                       RelatedTableName = row.Field<string>("RelatedTableName"),
                                       RelatedColumnName = row.Field<string>("RelatedColumnName"),
                                       AssociationType = AssociationType.Item
                                   }).ToList());

            this.AddAssociationListItems(associations);

            return associations;
        }
    }
}
