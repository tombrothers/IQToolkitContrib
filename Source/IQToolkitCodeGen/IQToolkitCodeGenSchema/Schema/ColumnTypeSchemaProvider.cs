using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class ColumnTypeSchemaProvider {
        private readonly IUnityContainer _container;
        private readonly IDatabase _database;
        private readonly DbConnectionFactory _connectionFactory;

        public ColumnTypeSchemaProvider(IUnityContainer container, IDatabase database, DbConnectionFactory connectionFactory) {
            ArgumentUtility.CheckNotNull("container", container);
            ArgumentUtility.CheckNotNull("database", database);
            ArgumentUtility.CheckNotNull("connectionFactory", connectionFactory);

            this._container = container;
            this._database = database;
            this._connectionFactory = connectionFactory;
        }

        public IList<ColumnTypeSchema> GetSchema() {
            var provider = this.GetProvider();

            using (var connection = this._connectionFactory.Create()) {
                return provider.GetSchema(connection);
            }
        }

        private Provider GetProvider() {
            switch (this._database.Type) {
                case DatabaseType.Access:
                    return this._container.Resolve<AccessProvider>();
                case DatabaseType.Vfp:
                    return this._container.Resolve<VfpProvider>();
                case DatabaseType.SqlCe35:
                    return this._container.Resolve<SqlCe35Provider>();
                case DatabaseType.SQLite:
                case DatabaseType.Oracle:
                case DatabaseType.MySql:
                case DatabaseType.SqlCe40:
                case DatabaseType.SqlServer:
                    return this._container.Resolve<Provider>();
                default:
                    throw new NotImplementedException(this._database.Type.ToString());
            }
        }
    }
}
