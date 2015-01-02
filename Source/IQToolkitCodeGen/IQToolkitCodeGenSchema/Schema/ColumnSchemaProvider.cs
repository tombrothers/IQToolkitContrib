using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class ColumnSchemaProvider {
        private readonly IUnityContainer _container;
        private readonly IDatabase _database;
        private readonly DbConnectionFactory _connectionFactory;

        public ColumnSchemaProvider(IUnityContainer container, IDatabase database, DbConnectionFactory connectionFactory) {
            ArgumentUtility.CheckNotNull("container", container);
            ArgumentUtility.CheckNotNull("database", database);
            ArgumentUtility.CheckNotNull("connectionFactory", connectionFactory);

            this._container = container;
            this._database = database;
            this._connectionFactory = connectionFactory;
        }

        public IList<ColumnSchema> GetSchema(string tableName) {
            ArgumentUtility.CheckNotNullOrEmpty("tableName", tableName);

            var provider = this.GetProvider();

            using (var connection = this._connectionFactory.Create()) {
                return provider.GetSchema(connection, tableName);
            }
        }

        private Provider GetProvider() {
            switch (this._database.Type) {
                case DatabaseType.Access:
                    return this._container.Resolve<OleDbProvider>();
                case DatabaseType.Oracle:
                    return this._container.Resolve<OracleProvider>();
                case DatabaseType.SQLite:
                    return this._container.Resolve<Provider>();
                case DatabaseType.Vfp:
                    return this._container.Resolve<VfpProvider>();
                case DatabaseType.MySql:
                case DatabaseType.SqlCe35:
                case DatabaseType.SqlCe40:
                case DatabaseType.SqlServer:
                    return this._container.Resolve<InformationSchemaProvider>();
                default:
                    throw new NotImplementedException(this._database.Type.ToString());
            }
        }
    }
}
