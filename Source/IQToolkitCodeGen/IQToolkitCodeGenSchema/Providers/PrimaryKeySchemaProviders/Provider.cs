using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.PrimaryKeySchemaProviders {
    internal class Provider : IProvider {
        protected readonly IDataTableFactory DataTableFactory;
        private readonly Func<string, string> _getQuotedName;

        public Provider(IDataTableFactory dataTableFactory, Func<string, string> getQuotedName = null) {
            ArgumentUtility.CheckNotNull("dataTableFactory", dataTableFactory);

            this.DataTableFactory = dataTableFactory;

            Func<string, string> defaultQuotedNameFunc = x => x;
            this._getQuotedName = getQuotedName ?? defaultQuotedNameFunc;
        }

        public IList<IPrimaryKeySchema> GetSchema(DbConnection connection, string tableName) {
            var primaryKeys = new List<IPrimaryKeySchema>();
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

                return this.DataTableFactory.CreateSchemaDataTable(command);
            }
        }
    }
}