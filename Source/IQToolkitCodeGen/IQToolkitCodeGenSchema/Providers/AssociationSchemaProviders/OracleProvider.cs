using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.AssociationSchemaProviders {
    internal class OracleProvider : Provider {
        private readonly IDataTableFactory _dataTableFactory;
        private readonly IOracleUserProvider _oracleUserProvider;

        public OracleProvider(IDataTableFactory dataTableFactory, IOracleUserProvider oracleUserProvider) {
            ArgumentUtility.CheckNotNull("dataTableFactory", dataTableFactory);
            ArgumentUtility.CheckNotNull("oracleUserProvider", oracleUserProvider);

            this._dataTableFactory = dataTableFactory;
            this._oracleUserProvider = oracleUserProvider;
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
            using (var command = connection.CreateCommand()) {
                command.CommandText = this.GetSelectStatement();

                return this._dataTableFactory.CreateDataTable(command);
            }
        }

        private string GetSelectStatement() {
            string sql = string.Format(@"
SELECT UC.CONSTRAINT_NAME AS FOREIGNKEY,
        UC.TABLE_NAME AS TABLENAME,
        UCC.COLUMN_NAME AS ColumnName,
        RT.TABLE_NAME AS RELATEDTABLENAME,
        RTC.column_name AS RelatedColumnName
    FROM SYS.ALL_CONSTRAINTS UC,
         SYS.ALL_CONSTRAINTS RT,
         SYS.ALL_CONS_COLUMNS RTC,
         SYS.ALL_CONS_COLUMNS UCC
    WHERE UC.R_CONSTRAINT_NAME = RT.CONSTRAINT_NAME
            AND UC.R_OWNER             = RT.OWNER
            AND UCC.constraint_name    = UC.CONSTRAINT_NAME
            AND RTC.constraint_name    = RT.CONSTRAINT_NAME
            AND (UC.CONSTRAINT_TYPE    = 'R')
            AND UC.OWNER = '{0}'", this._oracleUserProvider.User);

            return sql;
        }
    }
}

