using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class TableSchemaProvider {
        private readonly IUnityContainer _container;
        private readonly IDatabase _database;
        private readonly DbConnectionFactory _connectionFactory;

        public TableSchemaProvider(IUnityContainer container, IDatabase database, DbConnectionFactory connectionFactory) {
            ArgumentUtility.CheckNotNull("container", container);
            ArgumentUtility.CheckNotNull("database", database);
            ArgumentUtility.CheckNotNull("connectionFactory", connectionFactory);

            this._container = container;
            this._database = database;
            this._connectionFactory = connectionFactory;
        }

        public IList<TableSchema> GetSchema() {
            var provider = this.GetProvider();

            using (var connection = this._connectionFactory.Create()) {
                return provider.GetSchema(connection);
            }
        }

        private Provider GetProvider() {
            switch (this._database.Type) {
                case DatabaseType.Access:
                    return this._container.Resolve<AccessProvider>();
                case DatabaseType.SQLite:
                    return this._container.Resolve<SQLiteProvider>();
                case DatabaseType.Oracle:
                case DatabaseType.Vfp:
                    return this._container.Resolve<Provider>();
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
