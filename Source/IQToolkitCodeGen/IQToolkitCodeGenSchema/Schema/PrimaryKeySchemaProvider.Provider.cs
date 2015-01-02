using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class PrimaryKeySchemaProvider {
        private class Provider : IProvider {
            protected readonly DataTableFactory _dataTableFactory;
            private readonly Func<string, string> _getQuotedName = x => x;

            public Provider(DataTableFactory dataTableFactory, Func<string, string> getQuotedName = null) {
                ArgumentUtility.CheckNotNull("dataTableFactory", dataTableFactory);

                this._dataTableFactory = dataTableFactory;

                if (getQuotedName != null) {
                    this._getQuotedName = getQuotedName;
                }
            }

            public IList<PrimaryKeySchema> GetSchema(DbConnection connection, string tableName) {
                var primaryKeys = new List<PrimaryKeySchema>();
                var schemaDataTable = this.GetDataTable(connection, tableName);

                for (int index = 0, total = schemaDataTable.PrimaryKey.Length; index < total; index++) {
                    primaryKeys.Add(new PrimaryKeySchema(schemaDataTable.PrimaryKey[index].ColumnName,
                                    schemaDataTable.PrimaryKey[index].AutoIncrement));
                }

                return primaryKeys;
            }

            private DataTable GetDataTable(DbConnection connection, string tableName) {
                using (var command = connection.CreateCommand()) {
                    command.CommandText = string.Format("select * from {0} where 1=2", this._getQuotedName(tableName));

                    return this._dataTableFactory.CreateSchemaDataTable(command);
                }
            }
        }
    }
}
