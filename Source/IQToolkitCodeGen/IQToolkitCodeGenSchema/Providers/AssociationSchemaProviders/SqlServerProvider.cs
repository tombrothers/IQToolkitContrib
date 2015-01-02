using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.AssociationSchemaProviders {
    internal class SqlServerProvider : Provider {
        private readonly IDataTableFactory _dataTableFactory;

        public SqlServerProvider(IDataTableFactory dataTableFactory) {
            ArgumentUtility.CheckNotNull("dataTableFactory", dataTableFactory);

            this._dataTableFactory = dataTableFactory;
        }

        public override IList<IAssociationSchema> GetSchema(DbConnection connection) {
            var dataTable = this.GetDataTable(connection);
            var associations = new List<IAssociationSchema>();

            associations.AddRange(dataTable.AsEnumerable()
                                           .Select(row => new AssociationSchema(row.Field<string>("ForeignKey"),
                                                                                row.Field<string>("TableName"),
                                                                                row.Field<string>("ColumnName"),
                                                                                row.Field<string>("RelatedTableName"),
                                                                                row.Field<string>("RelatedColumnName"),
                                                                                AssociationType.Item)));

            this.AddAssociationListItems(associations);

            return associations;
        }

        private DataTable GetDataTable(DbConnection connection) {
            var command = connection.CreateCommand();
            command.CommandText = this.GetSelectStatement();

            return this._dataTableFactory.CreateDataTable(command);
        }

        private string GetSelectStatement() {
            return @"
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
        }
    }
}