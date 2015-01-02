using System;
using System.Data;
using System.Data.Common;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Factories {
    internal class DataTableFactory : IDataTableFactory {
        private readonly IDatabase _database;

        public DataTableFactory(IDatabase database) {
            ArgumentUtility.CheckNotNull("database", database);

            this._database = database;
        }

        public DataTable CreateSchemaDataTable(DbCommand command) {
            return this.Create(command, this.FillSchemaDataTable);
        }

        private void FillSchemaDataTable(DbDataAdapter dataAdapter, DataTable dataTable) {
            dataAdapter.FillSchema(dataTable, SchemaType.Source);
        }

        public DataTable CreateDataTable(DbCommand command) {
            return this.Create(command, this.FillDataTable);
        }

        private void FillDataTable(DbDataAdapter dataAdapter, DataTable dataTable) {
            dataAdapter.Fill(dataTable);
        }

        private DataTable Create(DbCommand command, Action<DbDataAdapter, DataTable> action) {
            var factory = DbProviderFactories.GetFactory(this._database.ProviderName);
            var dataTable = new DataTable();

            using (var dataAdapter = factory.CreateDataAdapter()) {
                dataAdapter.SelectCommand = command;
                action(dataAdapter, dataTable);
            }

            return dataTable;
        }
    }
}