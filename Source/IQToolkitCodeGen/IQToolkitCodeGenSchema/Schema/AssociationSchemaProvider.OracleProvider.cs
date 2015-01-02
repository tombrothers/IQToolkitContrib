using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class AssociationSchemaProvider {
        private class OracleProvider : Provider {
            private readonly DataTableFactory _dataTableFactory;

            public OracleProvider(DataTableFactory dataTableFactory) {
                ArgumentUtility.CheckNotNull("dataTableFactory", dataTableFactory);

                this._dataTableFactory = dataTableFactory;
            }

            public override IList<AssociationSchema> GetSchema(DbConnection connection) {
                var dataTable = this.GetDataTable(connection);
                var associations = new List<AssociationSchema>();

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
                using (var command = connection.CreateCommand()) {
                    command.CommandText = this.GetSelectStatement();

                    return this._dataTableFactory.CreateDataTable(command);
                }
            }
            
            private string GetSelectStatement() {
                string sql = string.Empty;

                //Properties.Settings config = new Properties.Settings();

                //if (config.Oracle_FKSql.Length > 0)
                //{
                //    sql = config.Oracle_FKSql;
                //}
                //else
                //{

                sql = @"SELECT UC.CONSTRAINT_NAME                     AS FOREIGNKEY,
                  concat(concat(UC.owner,'.'), UC.TABLE_NAME) AS TABLENAME,
                  UCC.COLUMN_NAME                             AS ColumnName,
                  concat(concat(UC.owner,'.'), RT.TABLE_NAME) AS RELATEDTABLENAME,
                  RTC.column_name                             AS RelatedColumnName
                FROM SYS.ALL_CONSTRAINTS UC,
                  SYS.ALL_CONSTRAINTS RT,
                  SYS.ALL_CONS_COLUMNS RTC,
                  SYS.ALL_CONS_COLUMNS UCC
                WHERE UC.R_CONSTRAINT_NAME = RT.CONSTRAINT_NAME
                AND UC.R_OWNER             = RT.OWNER
                AND UCC.constraint_name    = UC.CONSTRAINT_NAME
                AND RTC.constraint_name    = RT.CONSTRAINT_NAME
                AND (UC.CONSTRAINT_TYPE    = 'R')
                AND (UC.OWNER              in (select user from dual))";
                //}

                return sql;
            }
        }
    }
}
